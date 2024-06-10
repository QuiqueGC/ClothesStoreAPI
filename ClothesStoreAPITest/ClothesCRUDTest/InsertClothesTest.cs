using ClothesStoreAPI.Repository.DB.Clothes;
using ClothesStoreAPI.Service.Clothes;
using ClothesStoreAPI.Models;
using System.Diagnostics;
using Moq;

namespace ClothesStoreAPITest.ClothesCRUDTest
{
    public class InsertClothesTest
    {
        [Fact]
        public async void InsertClothes_ShouldInsertClothesCorrectly()
        {
            // Arrange
            Mock<IClothesRepository> fakeClothesRepository = new();
            ClothesService clothesService = new(fakeClothesRepository.Object);
            string expectedResult = "Success";
            Clothes newClothes = new()
            {
                name = "Test",
                idColor = 1,
                idSize = 1,
                price = 29.99,
                description = "Abrígate bien, que te va a coger el frío"
            };
            fakeClothesRepository.Setup(r => r.InsertClothes(It.IsAny<Clothes>())).ReturnsAsync(expectedResult);



            // Act
            string actualResult = await clothesService.InsertClothes(newClothes);


            // Assert
            fakeClothesRepository.Verify(r => r.InsertClothes(newClothes), Times.Once);
            Assert.Equal(expectedResult, actualResult);
        }


        [Fact]
        public async void InsertClothes_ShouldReturnInvalidPrice()
        {
            // Arrange
            var fakeClothesRepository = new Mock<IClothesRepository>();
            var clothesService = new ClothesService(fakeClothesRepository.Object);
            string expectedResult = "Invalid price";
            Clothes newClothes = new()
            {
                name = "Test",
                idColor = 1,
                idSize = 1,
                price = -88.8,
                description = "Abrígate bien, que te va a coger el frío"
            };
            fakeClothesRepository.Setup(r => r.InsertClothes(It.IsAny<Clothes>())).ReturnsAsync(expectedResult);


            // Act
            string actualResult = await clothesService.InsertClothes(newClothes);


            // Assert
            fakeClothesRepository.Verify(r => r.InsertClothes(newClothes), Times.Never);
            Assert.Equal(expectedResult, actualResult);
        }


        [Fact]
        public async void InsertClothes_ShouldReturnDataIncomplete()
        {
            // Arrange
            var fakeClothesRepository = new Mock<IClothesRepository>();
            var clothesService = new ClothesService(fakeClothesRepository.Object);
            string expectedResult = "Data incomplete";
            Clothes newClothes = new()
            {
                name = "       ",
                idColor = 1,
                idSize = 1,
                price = 99.99,
                description = "             "
            };
            fakeClothesRepository.Setup(r => r.InsertClothes(It.IsAny<Clothes>())).ReturnsAsync(expectedResult);


            // Act
            string actualResult = await clothesService.InsertClothes(newClothes);


            // Assert
            fakeClothesRepository.Verify(r => r.InsertClothes(newClothes), Times.Never);
            Assert.Equal(expectedResult, actualResult);
        }


    }
}