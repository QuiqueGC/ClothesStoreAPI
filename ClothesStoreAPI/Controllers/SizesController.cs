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
    public class SizesController : ApiController
    {
        private ClothesStoreEntities db = new ClothesStoreEntities();

        // GET: api/Sizes
        public IQueryable<Size> GetSize()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return db.Size;
        }

        // GET: api/Sizes/5
        [ResponseType(typeof(Size))]
        public async Task<IHttpActionResult> GetSize(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            IHttpActionResult result;

            Size size = await db.Size.FindAsync(id);

            if (size == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(size);
            }

            return result;
        }

        /// <summary>
        /// get the size and a list of clothes filtered by its id
        /// </summary>
        /// <param name="id">int with idSize to filter</param>
        /// <returns>Size with list of Clothes</returns>
        [HttpGet]
        [Route("api/Sizes/{id}/Clothes")]
        public async Task<IHttpActionResult> GetClothesByIdSize(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            IHttpActionResult result;

            Size size = await db.Size
                .Include("Clothes")
                .Where(c => c.id == id)
                .FirstOrDefaultAsync();

            if (size == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(size);
            }

            return result;
        }


        // PUT: api/Sizes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSize(int id, Size size)
        {
            IHttpActionResult result;

            if (!ModelState.IsValid)
            {
                result = BadRequest(ModelState);
            }
            else
            {
                if (id != size.id)
                {
                    result = BadRequest();
                }
                else
                {
                    string msg = "";
                    db.Entry(size).State = EntityState.Modified;

                    try
                    {
                        await db.SaveChangesAsync();
                        result = Ok(size);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SizeExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        SqlException sqlException = (SqlException)ex.InnerException.InnerException;
                        msg = RepositoryUtils.ErrorMessage(sqlException);
                        result = BadRequest(msg);
                    }
                }
            }

            return result;
        }

        // POST: api/Sizes
        [ResponseType(typeof(Size))]
        public async Task<IHttpActionResult> PostSize(Size size)
        {
            IHttpActionResult result;

            if (!ModelState.IsValid)
            {
                result = BadRequest(ModelState);
            }
            else
            {

                db.Size.Add(size);
                String msg = "";
                try
                {
                    await db.SaveChangesAsync();
                    result = CreatedAtRoute("DefaultApi", new { size.id }, size);
                }
                catch (DbUpdateException ex)
                {
                    SqlException sqlException = (SqlException)ex.InnerException.InnerException;
                    msg = RepositoryUtils.ErrorMessage(sqlException);
                    result = BadRequest(msg);
                }
                
            }
            return result;
        }


        // DELETE: api/Sizes/5
        [ResponseType(typeof(Size))]
        public async Task<IHttpActionResult> DeleteSize(int id)
        {
            IHttpActionResult result;
            Size size = await db.Size.FindAsync(id);
            if (size == null)
            {
                result = NotFound();
            }
            else
            {
                string msg = "";
                db.Size.Remove(size);
                try
                {
                    await db.SaveChangesAsync();
                    result = Ok(size);
                }
                catch (DbUpdateException ex)
                {
                    SqlException sqlException = (SqlException)ex.InnerException.InnerException;
                    msg = RepositoryUtils.ErrorMessage(sqlException);
                    result = BadRequest(msg);
                }

            }
            return Ok(size);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SizeExists(int id)
        {
            return db.Size.Count(e => e.id == id) > 0;
        }
    }
}