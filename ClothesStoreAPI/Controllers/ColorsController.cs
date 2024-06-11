using ClothesStoreAPI.Models;
using ClothesStoreAPI.Service.Colors;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ClothesStoreAPI.Controllers
{
    public class ColorsController : ApiController
    {
        private readonly IColorsService service;

        public ColorsController(IColorsService service)
        {
            this.service = service;
        }


        // GET: api/Colors
        public IQueryable<Colors> GetColors()
        {
            return service.GetColors();
        }


        // GET: api/Colors/5
        [ResponseType(typeof(Colors))]
        public async Task<IHttpActionResult> GetColors(int id)
        {
            IHttpActionResult result;
            Colors colors = await service.GetColorById(id);

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
            Colors colors = await service.GetClothesByIdColor(id);

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
                    string msg = await service.UpdateColor(id, colors);
                    result = SetResult(msg, colors);
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
                string msg = await service.InsertColor(colors);
                result = SetResult(msg, colors);
            }
            return result;
        }


        // DELETE: api/Colors/5
        [ResponseType(typeof(Colors))]
        public async Task<IHttpActionResult> DeleteColors(int id)
        {
            String msg = await service.DeleteColor(id);
            return SetResult(msg, new SuccessResponse(msg));
        }


        /// <summary>
        /// sets the variable IHttpActionResult that the function will return
        /// depending on the msg provided by the service
        /// </summary>
        /// <param name="msg">String with the msg (Success, NotFound...)</param>
        /// <param name="objectResult">Object that will be at the body of the response</param>
        /// <returns>IHttpActionResult response with its body (in the chase of having it)</returns>
        private IHttpActionResult SetResult(String msg, Object objectResult)
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