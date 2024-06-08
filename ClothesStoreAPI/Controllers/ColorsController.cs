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
using ClothesStoreAPI.Repository.DBManager;
using ClothesStoreAPI.Utils;

namespace ClothesStoreAPI.Controllers
{
    public class ColorsController : ApiController
    {
        private ColorsRepository colorsRepository = new ColorsRepository();

        // GET: api/Colors
        public IQueryable<Colors> GetColors()
        {
            return colorsRepository.GetColors();
        }

        // GET: api/Colors/5
        [ResponseType(typeof(Colors))]
        public async Task<IHttpActionResult> GetColors(int id)
        {
            IHttpActionResult result;
            Colors colors = await colorsRepository.GetColorById(id);

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
            Colors colors = await colorsRepository.GetClothesByIdColor(id);

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
                    result = await TryToUpdateAtDB(id, colors);
                }
            }
            return result;
        }

        private async Task<IHttpActionResult> TryToUpdateAtDB(int id, Colors colors)
        {
            IHttpActionResult result;
            string msg = await colorsRepository.UpdateColor(id, colors);
            switch (msg)
            {
                case "":
                    result = Ok(colors);
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
                result = await TryToInsertAtDB(colors);
            }
            return result;
        }


        private async Task<IHttpActionResult> TryToInsertAtDB(Colors colors)
        {
            IHttpActionResult result;
            string msg = await colorsRepository.InsertColor(colors);
            switch (msg)
            {
                case "":
                    result = CreatedAtRoute("DefaultApi", new { colors.id }, colors);
                    break;
                default:
                    result = BadRequest(msg);
                    break;
            }
            return result;
        }



        // DELETE: api/Colors/5
        [ResponseType(typeof(Colors))]
        public async Task<IHttpActionResult> DeleteColors(int id)
        {
            IHttpActionResult result = await TryToDeleteAtDB(id);
            return result;
        }


        private async Task<IHttpActionResult> TryToDeleteAtDB(int id)
        {
            IHttpActionResult result;
            String msg = await colorsRepository.DeleteColor(id);
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
               colorsRepository.DisposeDB();
            }
            base.Dispose(disposing);
        }


    }
}