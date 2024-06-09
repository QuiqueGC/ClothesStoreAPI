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
using ClothesStoreAPI.Repository.DB.Sizes;
using ClothesStoreAPI.Repository.DBManager;
using ClothesStoreAPI.Service.Sizes;
using ClothesStoreAPI.Utils;

namespace ClothesStoreAPI.Controllers
{
    public class SizesController : ApiController
    {
        private readonly ISizesService service;


        public SizesController(ISizesService service)
        {
            this.service = service;
        }


        // GET: api/Sizes
        public IQueryable<Size> GetSize()
        {
            return service.GetSizes();
        }


        // GET: api/Sizes/5
        [ResponseType(typeof(Size))]
        public async Task<IHttpActionResult> GetSize(int id)
        {
            IHttpActionResult result;

            Size size = await service.GetSizeById(id);

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
        /// <returns>Size object with list of Clothes</returns>
        [HttpGet]
        [Route("api/Sizes/{id}/Clothes")]
        public async Task<IHttpActionResult> GetClothesByIdSize(int id)
        {
            IHttpActionResult result;
            Size size = await service.GetClothesByIdSize(id);

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
                    string msg = await service.UpdateSize(id, size);
                    result = SetResultFromMsg(msg, size);
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
                string msg = await service.InsertSize(size);
                result = SetResultFromMsg(msg, size);
            }
            return result;
        }


        // DELETE: api/Sizes/5
        [ResponseType(typeof(Size))]
        public async Task<IHttpActionResult> DeleteSize(int id)
        {
            String msg = await service.DeleteSize(id);
            return SetResultFromMsg(msg, new SuccessResponse(msg));
        }


        private IHttpActionResult SetResultFromMsg(String msg, Object objectResult)
        {
            IHttpActionResult result;
            switch (msg)
            {
                case "Success":
                    result = Ok(objectResult);
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
                service.DisposeDB();
            }
            base.Dispose(disposing);
        }

        
    }
}