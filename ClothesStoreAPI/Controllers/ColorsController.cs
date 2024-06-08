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
    public class ColorsController : ApiController
    {
        private ClothesStoreEntities db = new ClothesStoreEntities();

        // GET: api/Colors
        public IQueryable<Colors> GetColors()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return db.Colors;
        }

        // GET: api/Colors/5
        [ResponseType(typeof(Colors))]
        public async Task<IHttpActionResult> GetColors(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            IHttpActionResult result;

            Colors colors = await db.Colors
                .Where(c => c.id == id)
                .FirstOrDefaultAsync();

            if (colors == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(colors);
            }

            return result;
        }


        /// <summary>
        /// get the color and a list of clothes filtered by its id
        /// </summary>
        /// <param name="id">int with idColor to filter</param>
        /// <returns>Color with list of Clothes</returns>
        [HttpGet]
        [Route("api/Colors/{id}/Clothes")]
        public async Task<IHttpActionResult> GetClothesByIdColor(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            IHttpActionResult result;

            Colors colors = await db.Colors
                .Include("Clothes")
                .Where(c => c.id == id)
                .FirstOrDefaultAsync();

            if (colors == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(colors);
            }

            return result;
        }


        // PUT: api/Colors/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutColors(int id, Colors colors)
        {
            IHttpActionResult result;

            if (!ModelState.IsValid)
            {
                result = BadRequest(ModelState);
            }
            else
            {
                if (id != colors.id)
                {
                    result = BadRequest();
                }
                else
                {
                    string msg = "";
                    db.Entry(colors).State = EntityState.Modified;

                    try
                    {
                        await db.SaveChangesAsync();
                        result = CreatedAtRoute("DefaultApi", new { id = colors.id }, colors);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ColorsExists(id))
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

        // POST: api/Colors
        [ResponseType(typeof(Colors))]
        public async Task<IHttpActionResult> PostColors(Colors colors)
        {
            IHttpActionResult result;

            if (!ModelState.IsValid)
            {
                result = BadRequest(ModelState);
            }
            else
            {
                db.Colors.Add(colors);
                String msg = "";
                try
                {
                    await db.SaveChangesAsync();
                    result = CreatedAtRoute("DefaultApi", new { id = colors.id }, colors);
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

        // DELETE: api/Colors/5
        [ResponseType(typeof(Colors))]
        public async Task<IHttpActionResult> DeleteColors(int id)
        {
            IHttpActionResult result;
            Colors colors = await db.Colors.FindAsync(id);
            if (colors == null)
            {
                result = NotFound();
            }
            else
            {
                db.Colors.Remove(colors);
                
                string msg = "";
                try
                {
                    await db.SaveChangesAsync();
                    result = Ok(colors);

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

        private bool ColorsExists(int id)
        {
            return db.Colors.Count(e => e.id == id) > 0;
        }
    }
}