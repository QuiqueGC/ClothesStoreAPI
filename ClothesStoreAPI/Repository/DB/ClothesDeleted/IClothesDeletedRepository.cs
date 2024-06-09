using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStoreAPI.Repository.DB.ClothesDeleted
{
    public interface IClothesDeletedRepository
    {
        IQueryable<Models.ClothesDeleted> GetClothesDeleted();
        Task<List<Models.ClothesDeleted>> FindClothesDeletedByName(string name);
        Task<Models.Clothes> GetRestoredClothes();
        Task<String> RestoreClothes(int id);
        void DisposeDB();
    }
}
