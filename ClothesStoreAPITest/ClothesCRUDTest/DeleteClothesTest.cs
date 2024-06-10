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
    public class DeleteClothesTest
    {
        [Fact]
        public async void DeleteClothes_ShouldDeleteClothesCorrectly()
        {
            // Arrange
            int existentId = 9;
            Mock<IClothesRepository> fakeClothesRepository = new();
            ClothesService clothesService = new(fakeClothesRepository.Object);
            string expectedResult = "Success";

            fakeClothesRepository.Setup(r => r.DeleteClothes(existentId)).ReturnsAsync(expectedResult);
            fakeClothesRepository.Setup(r => r.ClothesExists(existentId)).Returns(true);


            // Act
            string actualResult = await clothesService.DeleteClothes(existentId);


            // Assert
            fakeClothesRepository.Verify(r => r.DeleteClothes(existentId), Times.Once);
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async void DeleteClothes_ShouldReturnNotFound()
        {
            // Arrange
            int inexistentId = 9;
            Mock<IClothesRepository> fakeClothesRepository = new();
            ClothesService clothesService = new(fakeClothesRepository.Object);
            string expectedResult = "NotFound";

            fakeClothesRepository.Setup(r => r.ClothesExists(inexistentId)).Returns(false);


            // Act
            string actualResult = await clothesService.DeleteClothes(inexistentId);


            // Assert
            fakeClothesRepository.Verify(r => r.DeleteClothes(inexistentId), Times.Never);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
