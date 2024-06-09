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


        public async Task<String> UpdateClothes(Models.Clothes clothes)
        {
            db.Entry(clothes).State = EntityState.Modified;
            return await TryToSaveAtDB();
        }


        public async Task<String> InsertClothes(Models.Clothes  clothes)
        {
            db.Clothes.Add(clothes);
            return await TryToSaveAtDB();
        }


        public async Task<String> DeleteClothes(int id)
        {
            Models.Clothes clothes = await db.Clothes.FindAsync(id);
            db.Clothes.Remove(clothes);
            return await TryToSaveAtDB();
        }


        public bool ClothesAlreadyExist(Models.Clothes clothes)
        {
            return db.Clothes.Count(
                c => c.name == clothes.name &&
                c.idColor == clothes.idColor &&
                c.idSize == clothes.idSize &&
                c.price == clothes.price &&
                c.description == clothes.description
                ) > 0;
        }


        public bool ClothesExists(int id)
        {
            return db.Clothes.Count(c => c.id == id) > 0;
        }


        public void DisposeDB()
        {
            db.Dispose();
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