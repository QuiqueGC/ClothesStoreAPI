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
    public class ClothesController : ApiController
    {
        private ClothesStoreEntities db = new ClothesStoreEntities();

        // GET: api/Clothes
        public IQueryable<Clothes> GetClothes()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return db.Clothes;
        }

        // GET: api/Clothes/5
        [ResponseType(typeof(Clothes))]
        public async Task<IHttpActionResult> GetClothes(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            IHttpActionResult result;
            //Clothes clothes = await db.Clothes.FindAsync(id);

            Clothes clothes = await db.Clothes
                .Include("Colors")
                .Include("Size")
                .Where(c => c.id == id)
                .FirstOrDefaultAsync();

            if (clothes == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(clothes);
            }
            return result;
        }

        /// <summary>
        /// get a list of clothes filtered by name
        /// </summary>
        /// <param name="name">string with the name to find</param>
        /// <returns>list of clothes</returns>
        [HttpGet]
        [Route("api/clothes/name/{name}")]
        public async Task<IHttpActionResult> FindByName(string name)
        {
            db.Configuration.LazyLoadingEnabled = false;
            IHttpActionResult result;

            List<Clothes> clothes = await db.Clothes
                .Include(c => c.Colors)
                .Include(c => c.Size)
                .Where(c => c.name.Contains(name))
                .ToListAsync();


            clothes.ForEach(c =>
            {
                c.Colors = new Colors { id = c.Colors.id, name = c.Colors.name };
                c.Size = c.Size != null ? new Size { id = c.Size.id, value = c.Size.value } : null;
            });


            if (clothes.Count == 0)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(clothes);
            }
            return result;

        }


        // PUT: api/Clothes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutClothes(int id, Clothes clothes)
        {
            IHttpActionResult result;

            if (!ModelState.IsValid)
            {
                result = BadRequest(ModelState);
            }
            else
            {
                if (id != clothes.id)
                {
                    result = BadRequest();
                }
                else
                {
                    string msg = "";
                    db.Entry(clothes).State = EntityState.Modified;
                    try
                    {
                        await db.SaveChangesAsync();
                        result = StatusCode(HttpStatusCode.NoContent);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ClothesExists(id))
                        {
                            result = NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        SqlException sqlException = (SqlException)ex.InnerException.InnerException;
                        msg = MyUtils.ErrorMessage(sqlException);
                        result = BadRequest(msg);
                    }
                }
            }

            return result;
        }


        // POST: api/Clothes
        [ResponseType(typeof(Clothes))]
        public async Task<IHttpActionResult> PostClothes(Clothes clothes)
        {
            IHttpActionResult result;

            if (!ModelState.IsValid)
            {
                result = BadRequest(ModelState);
            }
            else
            {
                db.Clothes.Add(clothes);
                string msg = "";
                try
                {
                    await db.SaveChangesAsync();
                    result = CreatedAtRoute("DefaultApi", new { clothes.id }, clothes);
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


        // DELETE: api/Clothes/5
        [ResponseType(typeof(Clothes))]
        public async Task<IHttpActionResult> DeleteClothes(int id)
        {

            IHttpActionResult result;
            Clothes clothes = await db.Clothes.FindAsync(id);
            if (clothes == null)
            {
                result = NotFound();
            }
            else
            {
                string msg = "";
                try
                {
                    db.Clothes.Remove(clothes);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClothesExists(int id)
        {
            return db.Clothes.Count(e => e.id == id) > 0;
        }
    }
}