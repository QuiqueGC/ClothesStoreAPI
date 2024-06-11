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


        /// <summary>
        /// get a list of clothesDeleted filtered by name
        /// </summary>
        /// <param name="name">string with the name to find</param>
        /// <returns>list of clothesDeleted</returns>
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


        /// <summary>
        /// deletes the record from the clothesDeleted table based
        /// on the id and inserts it into the clothes table with a new id
        /// </summary>
        /// <param name="id">int with ID of the record</param>
        /// <returns>String with the msg resulting of the operation(Success, NotFound...)</returns>
        [HttpDelete]
        [Route("api/clothesDeleted/restore/{id}")]
        public async Task<IHttpActionResult> RestoreClothes(int id)
        {
            string msg = await service.RestoreClothesDeleted(id);
            return SetResultFromMsg(msg, new SuccessResponse(msg));
        }


        /// <summary>
        /// sets the variable IHttpActionResult that the function will return
        /// depending on the msg provided by the service
        /// </summary>
        /// <param name="msg">String with the msg (Success, NotFound...)</param>
        /// <param name="objectResult">Object that will be at the body of the response</param>
        /// <returns>IHttpActionResult response with its body (in the chase of having it)</returns>
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