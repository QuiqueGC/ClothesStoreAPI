using ClothesStoreAPI.Models;
using ClothesStoreAPI.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace ClothesStoreAPI.Repository.DB
{
    public class ClothesRepository
    {
        private ClothesStoreEntities db = new ClothesStoreEntities();

        public IQueryable<Clothes> GetClothes()
        {
            return db.Clothes;
        }


        public async Task<Clothes> GetClothes(int id)
        {
            return await db.Clothes
                .Include("Colors")
                .Include("Size")
                .Where(c => c.id == id)
                .FirstOrDefaultAsync();
        }


        public async Task<List<Clothes>> FindClothesByName(string name)
        {
            List<Clothes> clothes = await db.Clothes
                .Include(c => c.Colors)
                .Include(c => c.Size)
                .Where(c => c.name.Contains(name))
                .ToListAsync();


            clothes.ForEach(c =>
            {
                c.Colors = new Colors { id = c.Colors.id, name = c.Colors.name };
                c.Size = c.Size != null ? new Models.Size { id = c.Size.id, value = c.Size.value } : null;
            });

            return clothes;
        }

        public async Task<String> UpdateClothes(int id, Clothes clothes)
        {
            string msg = "Success";
            db.Entry(clothes).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClothesExists(id))
                {
                    msg = "NotFound";
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException ex)
            {
                SqlException sqlException = (SqlException)ex.InnerException.InnerException;
                msg = RepositoryUtils.ErrorMessage(sqlException);
            }

            return msg;

        }

        public async Task<String> InsertClothes(Clothes clothes)
        {
            string msg = "Success";

            if (ClothesAlreadyExist(clothes))
            {
                msg = "Duplicate record";
            }
            else
            {
                db.Clothes.Add(clothes);

                try
                {
                    await db.SaveChangesAsync();
                }

                catch (DbUpdateException ex)
                {
                    SqlException sqlException = (SqlException)ex.InnerException.InnerException;
                    msg = RepositoryUtils.ErrorMessage(sqlException);
                }
            }

            return msg;
        }

        public async Task<String> DeleteClothes(int id)
        {
            string msg = "Success";
            Clothes clothes = await db.Clothes.FindAsync(id);
            if (clothes == null)
            {
                msg = "NotFound";
            }
            else
            {
                try
                {
                    db.Clothes.Remove(clothes);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    SqlException sqlException = (SqlException)ex.InnerException.InnerException;
                    msg = RepositoryUtils.ErrorMessage(sqlException);
                }
            }
            return msg;
        }

        private bool ClothesAlreadyExist(Clothes clothes)
        {
            return db.Clothes.Count(
                c => c.name == clothes.name &&
                c.idColor == clothes.idColor &&
                c.idSize == clothes.idSize &&
                c.price == clothes.price &&
                c.description == clothes.description
                ) > 0;
        }

        private bool ClothesExists(int id)
        {
            return db.Clothes.Count(c => c.id == id) > 0;
        }

        public void DisposeDB()
        {
            db.Dispose();
        }
    }
}