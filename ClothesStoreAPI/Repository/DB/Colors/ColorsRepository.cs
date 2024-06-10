using ClothesStoreAPI.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

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
            return await db.TryToSaveData();
        }


        public async Task<String> InsertColor(Colors colors)
        {
            db.Colors.Add(colors);
            return await db.TryToSaveData();
        }


        public async Task<String> DeleteColor(int id)
        {
            Colors colors = await db.Colors.FindAsync(id);
            db.Colors.Remove(colors);
            return await db.TryToSaveData();
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
    }
}