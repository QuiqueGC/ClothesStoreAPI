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
    public class ClothesController : ApiController
    {
        private ClothesStoreEntities db = new ClothesStoreEntities();

        // GET: api/Clothes
        public IQueryable<Clothes> GetClothes()
        {
            return db.Clothes;
        }

        // GET: api/Clothes/5
        [ResponseType(typeof(Clothes))]
        public async Task<IHttpActionResult> GetClothes(int id)
        {
            Clothes clothes = await db.Clothes.FindAsync(id);
            if (clothes == null)
            {
                return NotFound();
            }

            return Ok(clothes);
        }

        // PUT: api/Clothes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutClothes(int id, Clothes clothes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clothes.id)
            {
                return BadRequest();
            }

            db.Entry(clothes).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClothesExists(id))
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

        // POST: api/Clothes
        [ResponseType(typeof(Clothes))]
        public async Task<IHttpActionResult> PostClothes(Clothes clothes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Clothes.Add(clothes);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = clothes.id }, clothes);
        }

        // DELETE: api/Clothes/5
        [ResponseType(typeof(Clothes))]
        public async Task<IHttpActionResult> DeleteClothes(int id)
        {
            Clothes clothes = await db.Clothes.FindAsync(id);
            if (clothes == null)
            {
                return NotFound();
            }

            db.Clothes.Remove(clothes);
            await db.SaveChangesAsync();

            return Ok(clothes);
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