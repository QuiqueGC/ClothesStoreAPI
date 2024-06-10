using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStoreAPI.Service.Sizes
{
    public interface ISizesService
    {
        IQueryable<Models.Size> GetSizes();
        Task<Models.Size> GetSizeById(int id);
        Task<Models.Size> GetClothesByIdSize(int id);
        Task<String> UpdateSize(int id, Models.Size size);
        Task<String> InsertSize(Models.Size size);
        Task<String> DeleteSize(int id);
        void DisposeDB();
    }
}
