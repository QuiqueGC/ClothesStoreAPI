using ClothesStoreAPI.Models;
using ClothesStoreAPI.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace ClothesStoreAPI.Repository.DB
{
    public class SizesRepository
    {
        private ClothesStoreEntities db = new ClothesStoreEntities();

        public IQueryable<Size> GetSizes()
        {
            return db.Size;
        }

        public async Task<Size> GetSizeById(int id)
        {
            return await db.Size.FindAsync(id);
        }

        public async Task<Size> GetClothesByIdSize(int id)
        {
            return await db.Size
                .Include("Clothes")
                .Where(c => c.id == id)
                .FirstOrDefaultAsync();
        }


        public async Task<String> UpdateSize(int id, Size size)
        {
            string msg = "Success";
            db.Entry(size).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SizeExists(id))
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

        public async Task<String> InsertSize(Size size)
        {
            String msg = "Success";

            if (SizeValueAlreadyExist(size.value))
            {
                msg = "Duplicate record";
            }
            else
            {
                db.Size.Add(size);

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


        public async Task<String> DeleteSize(int id)
        {
            Size size = await db.Size.FindAsync(id);
            string msg = "Success";

            if (size == null)
            {
                msg = "NotFound";
            }
            else
            {
                db.Size.Remove(size);

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



        public void DisposeDB()
        {
            db.Dispose();
        }

        private bool SizeValueAlreadyExist(string sizeValue)
        {
            return db.Size.Count(c => c.value == sizeValue) > 0;
        }

        private bool SizeExists(int id)
        {
            return db.Size.Count(e => e.id == id) > 0;
        }

    }
}