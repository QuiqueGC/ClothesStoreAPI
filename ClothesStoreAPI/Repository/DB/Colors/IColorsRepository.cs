﻿using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStoreAPI.Repository.DB.Colors
{
    public interface IColorsRepository
    {
        IQueryable<Models.Colors> GetColors();
        Task<Models.Colors> GetColorById(int id);
        Task<Models.Colors> GetClothesByIdColor(int id);
        Task<String> UpdateColor(Models.Colors colors);
        Task<String> InsertColor(Models.Colors colors);
        Task<String> DeleteColor(int id);
        bool ColorNameAlreadyExist(String colorName);
        bool ColorsExists(int id);
        void DisposeDB();
    }
}
