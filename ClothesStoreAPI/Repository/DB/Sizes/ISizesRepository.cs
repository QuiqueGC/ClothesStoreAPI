using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStoreAPI.Repository.DB.Sizes
{
    public interface ISizesRepository
    {
        IQueryable<Models.Size> GetSizes();
        Task<Models.Size> GetSizeById(int id);
        Task<Models.Size> GetClothesByIdSize(int id);
        Task<String> UpdateSize(Models.Size size);
        Task<String> InsertSize(Models.Size size);
        Task<String> DeleteSize(int id);
        bool SizeValueAlreadyExist(string sizeValue);
        bool SizeExists(int id);
        void DisposeDB();
    }
}
