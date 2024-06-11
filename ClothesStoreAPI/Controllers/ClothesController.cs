using ClothesStoreAPI.Models;
using ClothesStoreAPI.Service.Clothes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ClothesStoreAPI.Controllers
{
    public class ClothesController : ApiController
    {
        private readonly IClothesService service;

        public ClothesController(IClothesService service)
        {
            this.service = service;
        }


        // GET: api/Clothes
        public IQueryable<Clothes> GetClothes()
        {
            return service.GetClothes();
        }


        // GET: api/Clothes/5
        [ResponseType(typeof(Clothes))]
        public async Task<IHttpActionResult> GetClothes(int id)
        {
            IHttpActionResult result;

            Clothes clothes = await service.GetClothes(id);

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
        public async Task<IHttpActionResult> FindClothesByName(string name)
        {
            IHttpActionResult result;

            List<Clothes> clothes = await service.FindClothesByName(name);

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
                    string msg = await service.UpdateClothes(id, clothes);
                    result = SetResultFromMsg(msg, clothes);
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
                string msg = await service.InsertClothes(clothes);
                result = SetResultFromMsg(msg, clothes);
            }
            return result;
        }


        // DELETE: api/Clothes/5
        [ResponseType(typeof(Clothes))]
        public async Task<IHttpActionResult> DeleteClothes(int id)
        {
            string msg = await service.DeleteClothes(id);
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