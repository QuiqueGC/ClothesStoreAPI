using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ClothesStoreAPI.Models;

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

            Colors colors = await db.Colors
                .Where(c => c.id == id)
                .FirstOrDefaultAsync();

            if (colors == null)
            {
                return NotFound();
            }

            return Ok(colors);
        }

        // PUT: api/Colors/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutColors(int id, Colors colors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != colors.id)
            {
                return BadRequest();
            }

            db.Entry(colors).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColorsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Colors
        [ResponseType(typeof(Colors))]
        public async Task<IHttpActionResult> PostColors(Colors colors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Colors.Add(colors);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = colors.id }, colors);
        }

        // DELETE: api/Colors/5
        [ResponseType(typeof(Colors))]
        public async Task<IHttpActionResult> DeleteColors(int id)
        {
            Colors colors = await db.Colors.FindAsync(id);
            if (colors == null)
            {
                return NotFound();
            }

            db.Colors.Remove(colors);
            await db.SaveChangesAsync();

            return Ok(colors);
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