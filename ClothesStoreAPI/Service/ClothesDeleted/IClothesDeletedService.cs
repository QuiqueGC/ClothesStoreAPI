using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStoreAPI.Service.ClothesDeleted
{
    public interface IClothesDeletedService
    {
        IQueryable<Models.ClothesDeleted> GetClothesDeleted();
        Task<List<Models.ClothesDeleted>> FindClothesDeletedByName(string name);
        Task<String> RestoreClothesDeleted(int id);
        void DisposeDB();
    }
}
