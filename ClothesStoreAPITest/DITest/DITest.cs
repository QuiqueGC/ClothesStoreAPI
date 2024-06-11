using ClothesStoreAPI.Models;
using ClothesStoreAPI.Repository.DB.Clothes;
using ClothesStoreAPI.Repository.DB.ClothesDeleted;
using ClothesStoreAPI.Repository.DB.Colors;
using ClothesStoreAPI.Repository.DB.Sizes;
using ClothesStoreAPI.Service.Clothes;
using ClothesStoreAPI.Service.ClothesDeleted;
using ClothesStoreAPI.Service.Colors;
using ClothesStoreAPI.Service.Sizes;
using Moq;

namespace ClothesStoreAPITest.DITest
{
    public class DITest
    {

        /// <summary>
        /// Check if the DI is configured correctly
        /// </summary>
        [Fact]
        public void DependenciesTest_ShouldBeInjected()
        {
            // Arrange
            Mock<IClothesStoreEntities> mockClothesStoreEntites = new();
            Mock<IColorsRepository> mockColorsRepository = new();
            Mock<ISizesRepository> mockSizesRepository = new();
            Mock<IClothesRepository> mockClothesRepository = new();
            Mock<IClothesDeletedRepository> mockClothesDeletedRepository = new();
            Mock<IColorsService> mockColorsService = new();
            Mock<IClothesService> mockClothesService = new();
            Mock<ISizesService> mockSizesService = new();
            Mock<IClothesDeletedService> mockClothesDeletedService = new();

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
