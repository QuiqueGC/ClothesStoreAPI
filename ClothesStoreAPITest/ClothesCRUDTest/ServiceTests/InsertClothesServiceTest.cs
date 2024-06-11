using ClothesStoreAPI.Models;
using ClothesStoreAPI.Repository.DB.Clothes;
using ClothesStoreAPI.Service.Clothes;
using Moq;

namespace ClothesStoreAPITest.ClothesCRUDTest.ServiceTests
{
    public class InsertClothesServiceTest
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
                name = "Lorem ipsum",
                idColor = 1,
                idSize = 1,
                price = 29.99,
                description = "Lorem ipsum dolor sit amet, consectetur"
            };
            fakeClothesRepository.Setup(r => r.InsertClothes(It.IsAny<Clothes>())).ReturnsAsync(expectedResult);

            // Act
            string actualResult = await clothesService.InsertClothes(newClothes);

            // Assert
            fakeClothesRepository.Verify(r => r.InsertClothes(newClothes), Times.Once);
            Assert.Equal(expectedResult, actualResult);
        }



        [Theory]
        [InlineData("Lorem ipsum", 1, 1, -88.88, "Lorem ipsum dolor sit amet, consectetur",
            "Invalid price")
            ]
        [InlineData("       ", 1, 1, 88.88, "         ",
            "Data incomplete")
            ]
        [InlineData("Lorem ipsum dolor sit amet, consectetur", 1, 1, 88.88, "Lorem ipsum dolor sit amet, consectetur",
            "Name too long")
            ]
        [InlineData(
            "Lorem ipsum", 1, 1, 88.88,
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit." +
            " Sed do eiusmod tempor incididunt ut labore et dolore magna" +
            " aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco" +
            " laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor" +
            " in reprehenderit.",
            "Description too long")
            ]
        [InlineData("Lorem ipsum", -1, 1, 88.88, "Lorem ipsum dolor sit amet, consectetur",
            "Invalid idColor")
            ]
        [InlineData("Lorem ipsum", 1, -1, 88.88, "Lorem ipsum dolor sit amet, consectetur",
            "Invalid idSize")
            ]
        public async void InsertClothes_ValidationFailed(string name, int idColor, int idSize, double price, string description, string expectedErrorMsg)
        {
            // Arrange
            Mock<IClothesRepository> fakeClothesRepository = new();
            ClothesService clothesService = new(fakeClothesRepository.Object);
            Clothes newClothes = new()
            {
                name = name,
                idColor = idColor,
                idSize = idSize,
                price = price,
                description = description
            };

            // Act
            string actualResult = await clothesService.InsertClothes(newClothes);

            // Assert
            fakeClothesRepository.Verify(r => r.InsertClothes(newClothes), Times.Never);
            Assert.Equal(expectedErrorMsg, actualResult);
        }
    }
}