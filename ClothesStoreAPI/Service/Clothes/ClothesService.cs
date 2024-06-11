using ClothesStoreAPI.Repository.DB.Clothes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStoreAPI.Service.Clothes
{
    public class ClothesService : IClothesService
    {
        readonly IClothesRepository repository;

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


        /// <summary>
        /// get a list of clothes filtered by name
        /// </summary>
        /// <param name="name">string with the name to find</param>
        /// <returns>list of clothes</returns>
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

                if (msg == "Success")
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

        /// <summary>
        /// check if the data of the Clothes object is valid
        /// to do the insert at DB
        /// </summary>
        /// <param name="clothes">Clothes object to check</param>
        /// <returns>string msg with the info to respond</returns>
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
            else if (nameClothToSave.Length > 20)
            {
                msg = "Name too long";
            }
            else if (descriptionClothToSave.Length > 256)
            {
                msg = "Description too long";
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