using ClothesStoreAPI.Repository.DB.Colors;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStoreAPI.Service.Colors
{
    public class ColorsService : IColorsService
    {

        private readonly IColorsRepository repository;

        public ColorsService(IColorsRepository colorsRepository)
        {
            repository = colorsRepository;
        }



        public IQueryable<Models.Colors> GetColors()
        {
            return repository.GetColors();
        }


        public Task<Models.Colors> GetColorById(int id)
        {
            return repository.GetColorById(id);
        }


        /// <summary>
        /// get the color and a list of clothes filtered by its id
        /// </summary>
        /// <param name="id">int with idColor to filter</param>
        /// <returns>Color with list of Clothes</returns>
        public Task<Models.Colors> GetClothesByIdColor(int id)
        {
            return repository.GetClothesByIdColor(id);
        }


        public async Task<string> UpdateColor(int id, Models.Colors colors)
        {
            string msg;

            if (!repository.ColorsExists(id))
            {
                msg = "NotFound";
            }
            else
            {
                msg = CheckColorData(colors);
                if (msg == "Success")
                {
                    msg = await repository.UpdateColor(colors);
                }

            }
            return msg;
        }


        public async Task<string> InsertColor(Models.Colors colors)
        {
            string msg = CheckColorData(colors);

            if (repository.ColorNameAlreadyExist(colors.name))
            {
                msg = "Duplicate record";
            }
            else if (msg == "Success")
            {
                msg = await repository.InsertColor(colors);
            }

            return msg;
        }


        public async Task<string> DeleteColor(int id)
        {
            string msg;

            if (!repository.ColorsExists(id))
            {
                msg = "NotFound";
            }
            else
            {
                msg = await repository.DeleteColor(id);
            }
            return msg;
        }


        public void DisposeDB()
        {
            repository.DisposeDB();
        }


        /// <summary>
        /// Check if the data of Colors object is valid to do the insert
        /// </summary>
        /// <param name="colors">Colors object to check</param>
        /// <returns>string msg with the response to give</returns>
        private string CheckColorData(Models.Colors colors)
        {
            string msg = "Success";
            string nameColorToSave = colors.name.Trim().ToLower();

            if (nameColorToSave == "" || nameColorToSave == null)
            {
                msg = "Empty name";
            }
            else if (nameColorToSave.Length > 50)
            {
                msg = "Name too long";
            }
            else
            {
                colors.name = nameColorToSave;
            }
            return msg;
        }


    }
}