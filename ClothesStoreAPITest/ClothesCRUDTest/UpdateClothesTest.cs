using ClothesStoreAPI.Models;
using ClothesStoreAPI.Repository.DB.Clothes;
using ClothesStoreAPI.Service.Clothes;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStoreAPITest.ClothesCRUDTest
{
    public class UpdateClothesTest
    {
        [Fact]
        public async void UpdateClothes_ShouldUpdateClothesCorrectly()
        {
            // Arrange
            var fakeClothesRepository = new Mock<IClothesRepository>();
            var clothesService = new ClothesService(fakeClothesRepository.Object);
            string expectedResult = "Success";
            Clothes newClothes = new()
            {
                id = 9,
                name = "Abrigo de plumas",
                idColor = 1,
                idSize = 1,
                price = 50.89,
                description = "The winter is coming"
            };

            fakeClothesRepository.Setup(r => r.UpdateClothes(It.IsAny<Clothes>())).ReturnsAsync(expectedResult);
            fakeClothesRepository.Setup(r => r.ClothesExists(newClothes.id)).Returns(true);


            // Act
            string actualResult = await clothesService.UpdateClothes(newClothes.id, newClothes);


            // Assert
            fakeClothesRepository.Verify(r => r.UpdateClothes(It.IsAny<Clothes>()), Times.Once);
            Assert.Equal(expectedResult, actualResult);
        }


        [Fact]
        public async void UpdateClothes_ShouldReturnInvalidPrice()
        {
            // Arrange
            var fakeClothesRepository = new Mock<IClothesRepository>();
            var clothesService = new ClothesService(fakeClothesRepository.Object);
            string expectedResult = "Invalid price";
            Clothes newClothes = new()
            {
                id = 9,
                name = "Test",
                idColor = 1,
                idSize = 1,
                price = -88.8,
                description = "Abrígate bien, que te va a coger el frío"
            };
            fakeClothesRepository.Setup(r => r.UpdateClothes(It.IsAny<Clothes>())).ReturnsAsync(expectedResult);
            fakeClothesRepository.Setup(r => r.ClothesExists(newClothes.id)).Returns(true);


            // Act
            string actualResult = await clothesService.UpdateClothes(newClothes.id, newClothes);


            // Assert
            fakeClothesRepository.Verify(r => r.UpdateClothes(newClothes), Times.Never);
            Assert.Equal(expectedResult, actualResult);
        }


        [Fact]
        public async void UpdateClothes_ShouldReturnDataIncomplete()
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
            fakeClothesRepository.Setup(r => r.UpdateClothes(It.IsAny<Clothes>())).ReturnsAsync(expectedResult);
            fakeClothesRepository.Setup(r => r.ClothesExists(newClothes.id)).Returns(true);


            // Act
            string actualResult = await clothesService.UpdateClothes(newClothes.id, newClothes);


            // Assert
            fakeClothesRepository.Verify(r => r.InsertClothes(newClothes), Times.Never);
            Assert.Equal(expectedResult, actualResult);
        }
    }

}
