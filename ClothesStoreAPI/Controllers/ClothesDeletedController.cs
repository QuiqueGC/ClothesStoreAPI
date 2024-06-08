using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ClothesStoreAPI.Models;
using ClothesStoreAPI.Utils;

namespace ClothesStoreAPI.Controllers
{
    public class ClothesDeletedController : ApiController
    {
        private ClothesStoreEntities db = new ClothesStoreEntities();

        // GET: api/ClothesDeleted
        public IQueryable<ClothesDeleted> GetClothesDeleted()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return db.ClothesDeleted;
        }


        /// <summary>
        /// Restore clothes from the clothesDeleted table
        /// to the clothes table (id will change)
        /// </summary>
        /// <param name="id">int with id of the ClothesDeleted</param>
        /// <returns>the element restored with it's new id </returns>
        [HttpDelete]
        [Route("api/clothesDeleted/restore/{id}")]
        public async Task<IHttpActionResult> RestoreClothes(int id)
        {
            IHttpActionResult result;
            ClothesDeleted clothesDeleted = await db.ClothesDeleted.FindAsync(id);
            if (clothesDeleted == null)
            {
                result = NotFound();
            }
            else
            {
                string msg = "";
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
                    result = Ok(clothes);
                }
                catch (DbUpdateException ex)
                {
                    SqlException sqlException = (SqlException)ex.InnerException.InnerException;
                    msg = MyUtils.ErrorMessage(sqlException);
                    result = BadRequest(msg);
                }
            }

            return result;
        }
    }
}