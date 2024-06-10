using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStoreAPI.Repository.DB.ClothesDeleted
{
    public interface IClothesDeletedRepository
    {
        IQueryable<Models.ClothesDeleted> GetClothesDeleted();
        Task<List<Models.ClothesDeleted>> FindClothesDeletedByName(string name);
        bool ClothesDeletedExists(int id);
        Task<String> RestoreClothesDeleted(int id);
        void DisposeDB();
    }
}
