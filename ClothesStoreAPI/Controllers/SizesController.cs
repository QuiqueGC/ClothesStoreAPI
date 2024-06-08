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
using ClothesStoreAPI.Repository.DB;
using ClothesStoreAPI.Repository.DBManager;
using ClothesStoreAPI.Utils;

namespace ClothesStoreAPI.Controllers
{
    public class SizesController : ApiController
    {
        SizesRepository sizesRepository = new SizesRepository();

        // GET: api/Sizes
        public IQueryable<Size> GetSize()
        {
            return sizesRepository.GetSizes();
        }

        // GET: api/Sizes/5
        [ResponseType(typeof(Size))]
        public async Task<IHttpActionResult> GetSize(int id)
        {
            IHttpActionResult result;

            Size size = await sizesRepository.GetSizeById(id);

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
            IHttpActionResult result;
            Size size = await sizesRepository.GetClothesByIdSize(id);

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
                    result = await TryToUpdateAtDB(id, size);
                }
            }
            return result;
        }

        private async Task<IHttpActionResult> TryToUpdateAtDB(int id, Size size)
        {
            IHttpActionResult result;
            string msg = await sizesRepository.UpdateSize(id, size);
            switch (msg)
            {
                case "":
                    result = Ok(size);
                    break;
                case "NotFound":
                    result = NotFound();
                    break;
                default:
                    result = BadRequest(msg);
                    break;
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
                result = await TryToInsertAtDB(size);
            }
            return result;
        }


        private async Task<IHttpActionResult> TryToInsertAtDB(Size size)
        {
            IHttpActionResult result;
            string msg = await sizesRepository.InsertSize(size);
            switch (msg)
            {
                case "":
                    result = CreatedAtRoute("DefaultApi", new { size.id }, size);
                    break;
                default:
                    result = BadRequest(msg);
                    break;
            }
            return result;
        }


        // DELETE: api/Sizes/5
        [ResponseType(typeof(Size))]
        public async Task<IHttpActionResult> DeleteSize(int id)
        {
            IHttpActionResult result = await TryToDeleteAtDB(id);
            return result;
        }

        private async Task<IHttpActionResult> TryToDeleteAtDB(int id)
        {
            IHttpActionResult result;
            String msg = await sizesRepository.DeleteSize(id);
            switch (msg)
            {
                case "":
                    result = Ok();
                    break;
                case "NotFound":
                    result = NotFound();
                    break;
                default:
                    result = BadRequest(msg);
                    break;
            }
            return result;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                sizesRepository.DisposeDB();
            }
            base.Dispose(disposing);
        }

        
    }
}