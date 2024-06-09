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
using System.Web.Http.Results;
using ClothesStoreAPI.Models;
using ClothesStoreAPI.Repository.DB.Colors;
using ClothesStoreAPI.Repository.DBManager;
using ClothesStoreAPI.Utils;

namespace ClothesStoreAPI.Controllers
{
    public class ColorsController : ApiController
    {
        private readonly IColorsRepository repository;

        public ColorsController(IColorsRepository colorsRepository)
        {
            repository = colorsRepository;
        }

        // GET: api/Colors
        public IQueryable<Colors> GetColors()
        {
            return repository.GetColors();
        }

        // GET: api/Colors/5
        [ResponseType(typeof(Colors))]
        public async Task<IHttpActionResult> GetColors(int id)
        {
            IHttpActionResult result;
            Colors colors = await repository.GetColorById(id);

            if (colors == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(colors);
            }

            return result;
        }


        /// <summary>
        /// get the color and a list of clothes filtered by its id
        /// </summary>
        /// <param name="id">int with idColor to filter</param>
        /// <returns>Color with list of Clothes</returns>
        [HttpGet]
        [Route("api/Colors/{id}/Clothes")]
        public async Task<IHttpActionResult> GetClothesByIdColor(int id)
        {
            IHttpActionResult result;
            Colors colors = await repository.GetClothesByIdColor(id);

            if (colors == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(colors);
            }

            return result;
        }


        // PUT: api/Colors/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutColors(int id, Colors colors)
        {
            IHttpActionResult result;

            if (!ModelState.IsValid)
            {
                result = BadRequest(ModelState);
            }
            else
            {
                if (id != colors.id)
                {
                    result = BadRequest();
                }
                else
                {
                    string msg = await repository.UpdateColor(id, colors);
                    result = setResult(msg, colors);
                }
            }
            return result;
        }

        



        // POST: api/Colors
        [ResponseType(typeof(Colors))]
        public async Task<IHttpActionResult> PostColors(Colors colors)
        {
            IHttpActionResult result;

            if (!ModelState.IsValid)
            {
                result = BadRequest(ModelState);
            }
            else
            {
                string msg = await repository.InsertColor(colors);
                result = setResult(msg, colors);
            }
            return result;
        }




        // DELETE: api/Colors/5
        [ResponseType(typeof(Colors))]
        public async Task<IHttpActionResult> DeleteColors(int id)
        {
            String msg = await repository.DeleteColor(id);
            return setResult(msg, new SuccessResponse(msg));
        }



        private IHttpActionResult setResult(String msg, Object objectResult)
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
               repository.DisposeDB();
            }
            base.Dispose(disposing);
        }


    }
}