using ClothesStoreAPI.Models;
using ClothesStoreAPI.Repository.DB.ClothesDeleted;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStoreAPI.Repository.DB
{
    public class ClothesDeletedRepository : IClothesDeletedRepository
    {
        private readonly IClothesStoreEntities db;

        public ClothesDeletedRepository(IClothesStoreEntities db)
        {
            this.db = db;
        }



        public IQueryable<Models.ClothesDeleted> GetClothesDeleted()
        {
            return db.ClothesDeleted;
        }


        /// <summary>
        /// get a list of clothesDeleted filtered by name
        /// </summary>
        /// <param name="name">string with the name to find</param>
        /// <returns>list of clothesDeleted</returns>
        public async Task<List<Models.ClothesDeleted>> FindClothesDeletedByName(string name)
        {
            return await db.ClothesDeleted
                .Where(c => c.name.Contains(name))
                .ToListAsync();
        }


        /// <summary>
        /// deletes the record from the clothesDeleted table based
        /// on the id and inserts it into the clothes table with a new id
        /// </summary>
        /// <param name="id">int with ID of the record</param>
        /// <returns>String with the msg resulting of the operation(Success, NotFound...)</returns>
        public async Task<String> RestoreClothesDeleted(int id)
        {
            Models.ClothesDeleted clothesDeleted = await db.ClothesDeleted.FindAsync(id);
            Models.Clothes clothes = new Models.Clothes
            {
                name = clothesDeleted.name,
                idColor = clothesDeleted.idColor,
                idSize = clothesDeleted.idSize,
                price = clothesDeleted.price,
                description = clothesDeleted.description,
            };

            db.Clothes.Add(clothes);
            db.ClothesDeleted.Remove(clothesDeleted);

            return await db.TryToSaveData();
        }


        /// <summary>
        /// Check if exists a ClothesDeleted object with the id passed by parameters
        /// </summary>
        /// <param name="id">int with id to check</param>
        /// <returns>true if exists, false if not</returns>
        public bool ClothesDeletedExists(int id)
        {
            return db.ClothesDeleted.Count(c => c.id == id) > 0;
        }


        public void DisposeDB()
        {
            db.Dispose();
        }

    }
}