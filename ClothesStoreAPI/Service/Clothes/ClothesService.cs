using ClothesStoreAPI.Repository.DB.Clothes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ClothesStoreAPI.Service.Clothes
{
    public class ClothesService : IClothesService
    {
        IClothesRepository repository;


        public ClothesService(IClothesRepository repository)
        {
            this.repository = repository;
        }


        public IQueryable<Models.Clothes> GetClothes()
        {
            return repository.GetClothes();
        }


        public Task<Models.Clothes> GetClothes(int id)
        {
            return repository.GetClothes(id);
        }


        public Task<List<Models.Clothes>> FindClothesByName(string name)
        {
            return repository.FindClothesByName(name);
        }


        public async Task<string> UpdateClothes(int id, Models.Clothes clothes)
        {
            string msg;

            if (!repository.ClothesExists(id))
            {
                msg = "NotFound";
            }
            else
            {
                msg = CheckClothesData(clothes);

                if(msg == "Success")
                {
                    msg = await repository.UpdateClothes(clothes);
                }
            }
            return msg;

        }
        

        public async Task<string> InsertClothes(Models.Clothes clothes)
        {
            string msg = CheckClothesData(clothes);

            if (repository.ClothesAlreadyExist(clothes))
            {
                msg = "Duplicate record";
            }
            else if (msg == "Success")
            {
                msg = await repository.InsertClothes(clothes);
            }

            return msg;
        }

        public async Task<string> DeleteClothes(int id)
        {
            string msg;

            if (!repository.ClothesExists(id))
            {
                msg = "NotFound";
            }
            else
            {
                msg = await repository.DeleteClothes(id);
            }
            return msg;
        }

        public void DisposeDB()
        {
            repository.DisposeDB();
        }


        private string CheckClothesData(Models.Clothes clothes)
        {
            string msg = "Success";
            string nameClothToSave = clothes.name.Trim();
            string descriptionClothToSave = clothes.description.Trim();
            bool dataIncomplete =
                nameClothToSave == "" ||
                nameClothToSave == null ||
                descriptionClothToSave == "" ||
                descriptionClothToSave == null;
           
            if (dataIncomplete)
            {
                msg = "Data incomplete";
            }
            else if (clothes.idColor <= 0)
            {
                msg = "Invalid idColor";
            }
            else if (clothes.idSize <= 0)
            {
                msg = "Invalid idSize";
            }
            else if (clothes.price <= 0)
            {
                msg = "Invalid price";
            }
            else
            {
                clothes.name = nameClothToSave;
                clothes.description = descriptionClothToSave;
            }
            return msg;
        }





    }
}