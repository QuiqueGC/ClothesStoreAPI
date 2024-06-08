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
using ClothesStoreAPI.Utils;

namespace ClothesStoreAPI.Controllers
{
    public class ClothesController : ApiController
    {
        ClothesRepository clothesRepository = new ClothesRepository();

        // GET: api/Clothes
        public IQueryable<Clothes> GetClothes()
        {
            return clothesRepository.GetClothes();
        }

        // GET: api/Clothes/5
        [ResponseType(typeof(Clothes))]
        public async Task<IHttpActionResult> GetClothes(int id)
        {
            IHttpActionResult result;

            Clothes clothes = await clothesRepository.GetClothes(id);

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

            List<Clothes> clothes = await clothesRepository.FindClothesByName(name);

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
                    string msg = await clothesRepository.UpdateClothes(id, clothes);
                    result = setResultFromMsg(msg);
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
                string msg = await clothesRepository.InsertClothes(clothes);
                result = setResultFromMsg(msg);
            }
            return result;
        }


        // DELETE: api/Clothes/5
        [ResponseType(typeof(Clothes))]
        public async Task<IHttpActionResult> DeleteClothes(int id)
        {
            string msg = await clothesRepository.DeleteClothes(id);
            return setResultFromMsg(msg);
        }


        private IHttpActionResult setResultFromMsg(String msg)
        {
            IHttpActionResult result;
            switch (msg)
            {
                case "Success":
                    result = Ok(new SuccessResponse(msg));
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
                clothesRepository.DisposeDB();
            }
            base.Dispose(disposing);
        }
    }
}