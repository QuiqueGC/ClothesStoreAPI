using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStoreAPI.Repository.DB.Clothes
{
    public interface IClothesRepository
    {
        IQueryable<Models.Clothes> GetClothes();
        Task<Models.Clothes> GetClothes(int id);
        Task<List<Models.Clothes>> FindClothesByName(string name);
        Task<String> UpdateClothes(Models.Clothes clothes);
        Task<String> InsertClothes(Models.Clothes clothes);
        Task<String> DeleteClothes(int id);
        bool ClothesAlreadyExist(Models.Clothes clothes);
        bool ClothesExists(int id);
        void DisposeDB();
    }
}
