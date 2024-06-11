using ClothesStoreAPI.Repository.DB.ClothesDeleted;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStoreAPI.Service.ClothesDeleted
{
    public class ClothesDeletedService : IClothesDeletedService
    {

        IClothesDeletedRepository repository;

        public ClothesDeletedService(IClothesDeletedRepository clothesDeletedRepository)
        {
            repository = clothesDeletedRepository;
        }



        public IQueryable<Models.ClothesDeleted> GetClothesDeleted()
        {
            return repository.GetClothesDeleted();
        }


        /// <summary>
        /// get a list of clothesDeleted filtered by name
        /// </summary>
        /// <param name="name">string with the name to find</param>
        /// <returns>list of clothesDeleted</returns>
        public Task<List<Models.ClothesDeleted>> FindClothesDeletedByName(string name)
        {
            return repository.FindClothesDeletedByName(name);
        }


        /// <summary>
        /// deletes the record from the clothesDeleted table based
        /// on the id and inserts it into the clothes table with a new id
        /// </summary>
        /// <param name="id">int with ID of the record</param>
        /// <returns>String with the msg resulting of the operation(Success, NotFound...)</returns>
        public async Task<string> RestoreClothesDeleted(int id)
        {
            string msg;

            if (!repository.ClothesDeletedExists(id))
            {
                msg = "NotFound";
            }
            else
            {
                msg = await repository.RestoreClothesDeleted(id);
            }
            return msg;
        }


        public void DisposeDB()
        {
            repository.DisposeDB();
        }
    }
}