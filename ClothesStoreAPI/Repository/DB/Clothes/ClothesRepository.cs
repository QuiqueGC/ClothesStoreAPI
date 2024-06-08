using ClothesStoreAPI.Models;
using ClothesStoreAPI.Repository.DB.Clothes;
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
    public class ClothesRepository : IClothesRepository
    {
        private readonly IClothesStoreEntities db;
        public ClothesRepository(IClothesStoreEntities db)
        {
            this.db = db;
        }


        public IQueryable<Models.Clothes> GetClothes()
        {
            return db.Clothes;
        }


        public async Task<Models.Clothes> GetClothes(int id)
        {
            return await db.Clothes
                .Include("Colors")
                .Include("Size")
                .Where(c => c.id == id)
                .FirstOrDefaultAsync();
        }


        public async Task<List<Models.Clothes>> FindClothesByName(string name)
        {
            List<Models.Clothes> clothes = await db.Clothes
                .Where(c => c.name.Contains(name))
                .ToListAsync();

            return clothes;
        }


        public async Task<String> UpdateClothes(int id, Models.Clothes clothes)
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
                msg = ErrorMessageManager.GetErrorMessage(sqlException);
            }
            return msg;
        }


        public async Task<String> InsertClothes(Models.Clothes  clothes)
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
                    msg = ErrorMessageManager.GetErrorMessage(sqlException);
                }
            }
            return msg;
        }


        public async Task<String> DeleteClothes(int id)
        {
            string msg = "Success";

            Models.Clothes clothes = await db.Clothes.FindAsync(id);
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
                    msg = ErrorMessageManager.GetErrorMessage(sqlException);
                }
            }
            return msg;
        }


        private bool ClothesAlreadyExist(Models.Clothes clothes)
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