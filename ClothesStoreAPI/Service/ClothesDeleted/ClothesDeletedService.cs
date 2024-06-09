using ClothesStoreAPI.Repository.DB.Clothes;
using ClothesStoreAPI.Repository.DB.ClothesDeleted;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ClothesStoreAPI.Service.ClothesDeleted
{
    public class ClothesDeletedService : IClothesDeletedService
    {

        IClothesDeletedRepository repository;


        public ClothesDeletedService(IClothesDeletedRepository clothesDeletedRepository)
        {
            repository = clothesDeletedRepository;
        }


        public IQueryable<Models.ClothesDeleted> GetClothesDeleted()
        {
            return repository.GetClothesDeleted();
        }


        public Task<List<Models.ClothesDeleted>> FindClothesDeletedByName(string name)
        {
            return repository.FindClothesDeletedByName(name);
        }


        public async Task<string> RestoreClothesDeleted(int id)
        {
            string msg;

            if (!repository.ClothesDeletedExists(id))
            {
                msg = "NotFound";
            }
            else
            {
                msg = await repository.RestoreClothesDeleted(id);
            }
            return msg;
        }


        public void DisposeDB()
        {
            repository.DisposeDB();
        }
    }
}