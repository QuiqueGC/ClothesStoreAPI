using ClothesStoreAPI.Models;
using ClothesStoreAPI.Repository.DB.Sizes;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

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


        /// <summary>
        /// get the size and a list of clothes filtered by its id
        /// </summary>
        /// <param name="id">int with idSize to filter</param>
        /// <returns>Size object with list of Clothes</returns>
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
            return await db.TryToSaveData();
        }


        public async Task<String> InsertSize(Size size)
        {
            db.Size.Add(size);
            return await db.TryToSaveData();
        }


        public async Task<String> DeleteSize(int id)
        {
            Size size = await db.Size.FindAsync(id);
            db.Size.Remove(size);
            return await db.TryToSaveData();
        }


        public void DisposeDB()
        {
            db.Dispose();
        }


        /// <summary>
        /// Check if there are a record of Size
        /// with exactly the same value at DB
        /// </summary>
        /// <param name="sizeValue">Size object to check</param>
        /// <returns>true if found, false if dont</returns>
        public bool SizeValueAlreadyExist(string sizeValue)
        {
            return db.Size.Count(c => c.value == sizeValue) > 0;
        }


        /// <summary>
        /// Check if exists a Size object with the id passed by parameters
        /// </summary>
        /// <param name="id">int with id to check</param>
        /// <returns>true if exists, false if not</returns>
        public bool SizeExists(int id)
        {
            return db.Size.Count(e => e.id == id) > 0;
        }
    }
}