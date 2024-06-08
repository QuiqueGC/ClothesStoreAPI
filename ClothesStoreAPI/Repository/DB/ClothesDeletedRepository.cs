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
    public class ClothesDeletedRepository
    {
        private ClothesStoreEntities db = new ClothesStoreEntities();

        public IQueryable<ClothesDeleted> GetClothesDeleted()
        {
            return db.ClothesDeleted;
        }


        public async Task<List<ClothesDeleted>> FindClothesDeletedByName(string name)
        {
            List<ClothesDeleted> clothesDeleted = await db.ClothesDeleted
                .Where(c => c.name.Contains(name))
                .ToListAsync();

            return clothesDeleted;
        }




        public async Task<String> RestoreClothes(int id)
        {
            string msg = "Success";
            ClothesDeleted clothesDeleted = await db.ClothesDeleted.FindAsync(id);

            if (clothesDeleted == null)
            {
                msg = "NotFound";
            }
            else
            {
                try
                {
                    Clothes clothes = new Clothes
                    {
                        name = clothesDeleted.name,
                        idColor = clothesDeleted.idColor,
                        idSize = clothesDeleted.idSize,
                        price = clothesDeleted.price,
                        description = clothesDeleted.description,
                    };

                    db.Clothes.Add(clothes);
                    db.ClothesDeleted.Remove(clothesDeleted);
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
    }
}