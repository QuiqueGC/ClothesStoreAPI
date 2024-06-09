﻿using ClothesStoreAPI.Models;
using ClothesStoreAPI.Repository.DB.ClothesDeleted;
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


        public async Task<List<Models.ClothesDeleted>> FindClothesDeletedByName(string name)
        {
            List<Models.ClothesDeleted> clothesDeleted = await db.ClothesDeleted
                .Where(c => c.name.Contains(name))
                .ToListAsync();

            return clothesDeleted;
        }


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

            return await TryToSaveAtDB();
        }


        public bool ClothesDeletedExists(int id)
        {
            return db.ClothesDeleted.Count(c => c.id == id) > 0;
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