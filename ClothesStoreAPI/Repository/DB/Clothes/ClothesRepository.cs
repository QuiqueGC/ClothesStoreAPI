using ClothesStoreAPI.Models;
using ClothesStoreAPI.Repository.DB.Clothes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

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


        /// <summary>
        /// get a list of clothes filtered by name
        /// </summary>
        /// <param name="name">string with the name to find</param>
        /// <returns>list of clothes</returns>
        public async Task<List<Models.Clothes>> FindClothesByName(string name)
        {
            return await db.Clothes
                .Where(c => c.name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }


        public async Task<String> UpdateClothes(Models.Clothes clothes)
        {
            db.Entry(clothes).State = EntityState.Modified;
            return await db.TryToSaveData();
        }


        public async Task<String> InsertClothes(Models.Clothes clothes)
        {
            db.Clothes.Add(clothes);
            return await db.TryToSaveData();
        }


        public async Task<String> DeleteClothes(int id)
        {
            Models.Clothes clothes = await db.Clothes.FindAsync(id);
            db.Clothes.Remove(clothes);
            return await db.TryToSaveData();
        }


        /// <summary>
        /// Check if there are a record of Clothes
        /// with exactly the same data at DB
        /// </summary>
        /// <param name="clothes">CLothes object to check</param>
        /// <returns>true if found, false if dont</returns>
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


        /// <summary>
        /// Check if exists a Clothes object with the id passed by parameters
        /// </summary>
        /// <param name="id">int with id to check</param>
        /// <returns>true if exists, false if not</returns>
        public bool ClothesExists(int id)
        {
            return db.Clothes.Count(c => c.id == id) > 0;
        }


        public void DisposeDB()
        {
            db.Dispose();
        }


    }
}