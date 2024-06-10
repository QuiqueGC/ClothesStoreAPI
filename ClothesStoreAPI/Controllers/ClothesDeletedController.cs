using ClothesStoreAPI.Models;
using ClothesStoreAPI.Service.ClothesDeleted;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClothesStoreAPI.Controllers
{
    public class ClothesDeletedController : ApiController
    {
        private readonly IClothesDeletedService service;


        public ClothesDeletedController(IClothesDeletedService service)
        {
            this.service = service;
        }


        // GET: api/ClothesDeleted
        public IQueryable<ClothesDeleted> GetClothesDeleted()
        {
            return service.GetClothesDeleted();
        }


        [HttpGet]
        [Route("api/clothesDeleted/name/{name}")]
        public async Task<IHttpActionResult> FindClothesByName(string name)
        {
            IHttpActionResult result;

            List<ClothesDeleted> clothesDeleted = await service.FindClothesDeletedByName(name);

            if (clothesDeleted.Count == 0)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(clothesDeleted);
            }
            return result;
        }


        [HttpDelete]
        [Route("api/clothesDeleted/restore/{id}")]
        public async Task<IHttpActionResult> RestoreClothes(int id)
        {
            string msg = await service.RestoreClothesDeleted(id);
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