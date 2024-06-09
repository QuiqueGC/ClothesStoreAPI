using ClothesStoreAPI.Models;
using ClothesStoreAPI.Repository.DB.Sizes;
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
    public class SizesRepository : ISizesRepository
    {
        private readonly IClothesStoreEntities db;

        public SizesRepository(IClothesStoreEntities db)
        {
            this.db = db;
        }


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


        public async Task<String> UpdateSize(Size size)
        {
            db.Entry(size).State = EntityState.Modified;
            return await TryToSaveAtDB();
        }


        public async Task<String> InsertSize(Size size)
        {
            db.Size.Add(size);
            return await TryToSaveAtDB();
        }


        public async Task<String> DeleteSize(int id)
        {
            Size size = await db.Size.FindAsync(id);
            db.Size.Remove(size);
            return await TryToSaveAtDB();
        }


        public void DisposeDB()
        {
            db.Dispose();
        }


        public bool SizeValueAlreadyExist(string sizeValue)
        {
            return db.Size.Count(c => c.value == sizeValue) > 0;
        }


        public bool SizeExists(int id)
        {
            return db.Size.Count(e => e.id == id) > 0;
        }


        private async Task<String> TryToSaveAtDB()
        {
            string msg = "Success";
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                SqlException sqlException = (SqlException)ex.InnerException.InnerException;
                msg = ErrorMessageManager.GetErrorMessage(sqlException);
            }
            return msg;
        }
    }
}