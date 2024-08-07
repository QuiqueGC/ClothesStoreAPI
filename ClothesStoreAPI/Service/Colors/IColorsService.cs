﻿using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStoreAPI.Service.Colors
{
    public interface IColorsService
    {
        IQueryable<Models.Colors> GetColors();
        Task<Models.Colors> GetColorById(int id);
        Task<Models.Colors> GetClothesByIdColor(int id);
        Task<String> UpdateColor(int id, Models.Colors colors);
        Task<String> InsertColor(Models.Colors colors);
        Task<String> DeleteColor(int id);
        void DisposeDB();
    }
}
