using ClothesStoreAPI.Models;
using ClothesStoreAPI.Repository.DB.Sizes;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStoreAPI.Service.Sizes
{
    public class SizesService : ISizesService
    {

        private readonly ISizesRepository repository;

        public SizesService(ISizesRepository repository)
        {
            this.repository = repository;
        }



        public IQueryable<Size> GetSizes()
        {
            return repository.GetSizes();
        }


        public Task<Size> GetSizeById(int id)
        {
            return repository.GetSizeById(id);
        }


        /// <summary>
        /// get the size and a list of clothes filtered by its id
        /// </summary>
        /// <param name="id">int with idSize to filter</param>
        /// <returns>Size with list of Clothes</returns>
        public Task<Size> GetClothesByIdSize(int id)
        {
            return repository.GetClothesByIdSize(id);
        }


        public async Task<string> UpdateSize(int id, Size size)
        {
            string msg;

            if (!repository.SizeExists(id))
            {
                msg = "NotFound";
            }
            else
            {
                msg = CheckSizeData(size);
                if (msg == "Success")
                {
                    msg = await repository.UpdateSize(size);
                }

            }
            return msg;
        }


        public async Task<string> InsertSize(Size size)
        {
            string msg = CheckSizeData(size);

            if (repository.SizeValueAlreadyExist(size.value))
            {
                msg = "Duplicate record";
            }
            else if (msg == "Success")
            {
                msg = await repository.InsertSize(size);
            }

            return msg;
        }


        public async Task<string> DeleteSize(int id)
        {
            string msg;

            if (!repository.SizeExists(id))
            {
                msg = "NotFound";
            }
            else
            {
                msg = await repository.DeleteSize(id);
            }
            return msg;
        }


        public void DisposeDB()
        {
            repository.DisposeDB();
        }


        /// <summary>
        /// Check if the data of Size object is valid to do the insert
        /// </summary>
        /// <param name="size">Size object to check</param>
        /// <returns>string msg with the response to give</returns>
        private string CheckSizeData(Size size)
        {
            string msg = "Success";
            string valueSizeToSave = size.value.Trim().ToUpper();

            if (valueSizeToSave == "" || valueSizeToSave == null)
            {
                msg = "Empty name";
            }
            else
            {
                size.value = valueSizeToSave;
            }
            return msg;
        }
    }
}