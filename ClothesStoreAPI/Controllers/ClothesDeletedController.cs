﻿using System;
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
using ClothesStoreAPI.Repository.DB.ClothesDeleted;
using ClothesStoreAPI.Utils;

namespace ClothesStoreAPI.Controllers
{
    public class ClothesDeletedController : ApiController
    {
        private readonly IClothesDeletedRepository repository;

        public ClothesDeletedController(IClothesDeletedRepository repository)
        {
            this.repository = repository;
        }



        // GET: api/ClothesDeleted
        public IQueryable<ClothesDeleted> GetClothesDeleted()
        {
            return repository.GetClothesDeleted();
        }



        [HttpGet]
        [Route("api/clothesDeleted/name/{name}")]
        public async Task<IHttpActionResult> FindClothesByName(string name)
        {
            IHttpActionResult result;

            List<ClothesDeleted> clothesDeleted = await repository.FindClothesDeletedByName(name);

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
            string msg = await repository.RestoreClothes(id);
            return SetResultFromMsg(msg);
        }




        private IHttpActionResult SetResultFromMsg(String msg)
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
                repository.DisposeDB();
            }
            base.Dispose(disposing);
        }
    }
}