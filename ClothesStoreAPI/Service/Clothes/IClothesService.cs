using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStoreAPI.Service.Clothes
{
    public interface IClothesService
    {
        IQueryable<Models.Clothes> GetClothes();
        Task<Models.Clothes> GetClothes(int id);
        Task<List<Models.Clothes>> FindClothesByName(string name);
        Task<String> UpdateClothes(int id, Models.Clothes clothes);
        Task<String> InsertClothes(Models.Clothes clothes);
        Task<String> DeleteClothes(int id);
        void DisposeDB();
    }
}
