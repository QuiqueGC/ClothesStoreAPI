using ClothesStoreAPI.Models;
using ClothesStoreAPI.Repository.DB.Sizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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


        private string CheckSizeData(Size size)
        {
            string msg = "Success";
            string valueSizeToSave = size.value.Trim().ToUpper();

            if (valueSizeToSave == "")
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