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
            db.Configuration.LazyLoadingEnabled = false;
            return db.Clothes;
        }

        // GET: api/Clothes/5
        [ResponseType(typeof(Clothes))]
        public async Task<IHttpActionResult> GetClothes(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            //Clothes clothes = await db.Clothes.FindAsync(id);

            Clothes _clothes = await db.Clothes
                .Include("Colors")
                .Include("Size")
                .Include("SizeNumeric")
                .Where(c => c.id == id)
                .FirstOrDefaultAsync();

            IHttpActionResult result;

            if (_clothes == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(_clothes);
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

            List<Clothes> _clothes = await db.Clothes
                .Include(c => c.Colors)
                .Include(c => c.Size)
                .Include(c => c.SizeNumeric)
                .Where(c => c.name.Contains(name))
                .ToListAsync();

            
            _clothes.ForEach(c =>
            {
                c.Colors = new Colors { id = c.Colors.id, name = c.Colors.name };
                c.Size = c.Size != null ? new Size { id = c.Size.id, value = c.Size.value } : null;
                c.SizeNumeric = c.Size != null ? new SizeNumeric { id = c.SizeNumeric.id, value = c.SizeNumeric.value } : null;

            });




            IHttpActionResult result;

            if (_clothes.Count == 0)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(_clothes);
            }
            return result;

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