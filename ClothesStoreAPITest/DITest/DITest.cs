using ClothesStoreAPI.Models;
using ClothesStoreAPI.Repository.DB.Clothes;
using ClothesStoreAPI.Repository.DB.ClothesDeleted;
using ClothesStoreAPI.Repository.DB.Colors;
using ClothesStoreAPI.Repository.DB.Sizes;
using ClothesStoreAPI.Repository.DB;
using ClothesStoreAPI.Repository.DBManager;
using ClothesStoreAPI.Service.Clothes;
using ClothesStoreAPI.Service.ClothesDeleted;
using ClothesStoreAPI.Service.Colors;
using ClothesStoreAPI.Service.Sizes;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStoreAPITest.DITest
{
    public class DITest
    {
        [Fact]
        public void TestDependencyInjection()
        {

            // Arrange
            var mockClothesStoreEntites = new Mock<IClothesStoreEntities>();
            var mockColorsRepository = new Mock<IColorsRepository>();
            var mockSizesRepository = new Mock<ISizesRepository>();
            var mockClothesRepository = new Mock<IClothesRepository>();
            var mockClothesDeletedRepository = new Mock<IClothesDeletedRepository>();
            var mockColorsService = new Mock<IColorsService>();
            var mockClothesService = new Mock<IClothesService>();
            var mockSizesService = new Mock<ISizesService>();
            var mockClothesDeletedService = new Mock<IClothesDeletedService>();
            // Configura cualquier comportamiento necesario en el mock

            // Act
            IClothesStoreEntities clothesStoreEntities = mockClothesStoreEntites.Object;
            IColorsRepository colorsRepo = mockColorsRepository.Object;
            ISizesRepository sizesRepository = mockSizesRepository.Object;
            IClothesRepository clothesRepository = mockClothesRepository.Object;
            IClothesDeletedRepository clothesDeletedRepository = mockClothesDeletedRepository.Object;
            IColorsService colorsService = mockColorsService.Object;
            IClothesService clothesService = mockClothesService.Object;
            ISizesService sizesService = mockSizesService.Object;
            IClothesDeletedService clothesDeletedService = mockClothesDeletedService.Object;

            // Assert
            Assert.NotNull(clothesStoreEntities);
            Assert.NotNull(colorsRepo);
            Assert.NotNull(sizesRepository);
            Assert.NotNull(clothesRepository);
            Assert.NotNull(clothesDeletedRepository);
            Assert.NotNull(colorsService);
            Assert.NotNull(clothesService);
            Assert.NotNull(sizesService);
            Assert.NotNull(clothesDeletedService);
        }
    }
}
