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
using System.Xml.Linq;

namespace ClothesStoreAPI.Repository.DBManager
{
    public class ColorsRepository : DB.Colors.IColorsRepository
    {
        private readonly IClothesStoreEntities db;

        public ColorsRepository(IClothesStoreEntities db)
        {
            this.db = db;
        }


        public IQueryable<Colors> GetColors()
        {
            return db.Colors;
        }


        public async Task<Colors> GetColorById(int id)
        {
            return await db.Colors.FindAsync(id);
        }


        public async Task<Colors> GetClothesByIdColor(int id)
        {
            return await db.Colors
                .Include("Clothes")
                .Where(c => c.id == id)
                .FirstOrDefaultAsync();
        }


        public async Task<String> UpdateColor(Colors colors)
        {
            db.Entry(colors).State = EntityState.Modified;
            return await TryToSaveAtDB();
        }


        public async Task<String> InsertColor(Colors colors)
        {
            db.Colors.Add(colors);
            return await TryToSaveAtDB();
        }


        public async Task<String> DeleteColor(int id)
        {
            Colors colors = await db.Colors.FindAsync(id);
            db.Colors.Remove(colors);
            return await TryToSaveAtDB();
        }


        public void DisposeDB()
        {
            db.Dispose();
        }


        public bool ColorsExists(int id)
        {
            return db.Colors.Count(c => c.id == id) > 0;
        }


        public bool ColorNameAlreadyExist(String colorName)
        {
            return db.Colors.Count(c => c.name == colorName) > 0;
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