using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStoreAPI.Repository.DB.Sizes
{
    public interface ISizesRepository
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
