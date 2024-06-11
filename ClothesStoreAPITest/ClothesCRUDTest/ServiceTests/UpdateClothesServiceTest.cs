using ClothesStoreAPI.Models;
using ClothesStoreAPI.Repository.DB.Clothes;
using ClothesStoreAPI.Service.Clothes;
using Moq;

namespace ClothesStoreAPITest.ClothesCRUDTest.ServiceTests
{
    public class UpdateClothesServiceTest
    {


        /// <summary>
        /// Check if the service return the correctly answer in the case
        /// of successful update
        /// </summary>
        [Fact]
        public async void UpdateClothes_ShouldUpdateClothesCorrectly()
        {
            // Arrange
            int existentId = 9;
            Mock<IClothesRepository> fakeClothesRepository = new();
            ClothesService clothesService = new(fakeClothesRepository.Object);
            string expectedResult = "Success";
            Clothes newClothes = new()
            {
                id = existentId,
                name = "Lorem ipsum",
                idColor = 1,
                idSize = 1,
                price = 50.89,
                description = "Lorem ipsum"
            };

            fakeClothesRepository.Setup(r => r.UpdateClothes(It.IsAny<Clothes>())).ReturnsAsync(expectedResult);
            fakeClothesRepository.Setup(r => r.ClothesExists(existentId)).Returns(true);


            // Act
            string actualResult = await clothesService.UpdateClothes(existentId, newClothes);


            // Assert
            fakeClothesRepository.Verify(r => r.UpdateClothes(It.IsAny<Clothes>()), Times.Once);
            Assert.Equal(expectedResult, actualResult);
        }



        /// <summary>
        /// Check if the service return NotFound in case of find an inexistent
        /// id at the DB
        /// </summary>
        [Fact]
        public async void UpdateClothes_ShouldReturnNotFound()
        {
            // Arrange
            int inexistentId = 9;
            Mock<IClothesRepository> fakeClothesRepository = new();
            ClothesService clothesService = new(fakeClothesRepository.Object);
            string expectedResult = "NotFound";
            Clothes newClothes = new()
            {
                id = inexistentId,
                name = "Lorem ipsum",
                idColor = 1,
                idSize = 1,
                price = -88.8,
                description = "Lorem ipsum"
            };
            fakeClothesRepository.Setup(r => r.ClothesExists(inexistentId)).Returns(false);

            // Act
            string actualResult = await clothesService.UpdateClothes(inexistentId, newClothes);

            // Assert
            fakeClothesRepository.Verify(r => r.UpdateClothes(newClothes), Times.Never);
            Assert.Equal(expectedResult, actualResult);
        }



        /// <summary>
        /// Check different validations in the Clothes data
        /// and its responses when fail doing update
        /// </summary>
        /// <param name="name">string with the name of Clothes</param>
        /// <param name="idColor">int with the icColor in Clothes</param>
        /// <param name="idSize">int with the idSize in Clothes</param>
        /// <param name="price">double with the price of Clothes</param>
        /// <param name="description">string with the description of Clothes</param>
        /// <param name="expectedErrorMsg">string with the expected error msg</param>
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
        public async void UpdateClothes_ValidationFailed(string name, int idColor, int idSize, double price, string description, string expectedErrorMsg)
        {
            // Arrange
            int existentId = 9;
            Mock<IClothesRepository> fakeClothesRepository = new();
            ClothesService clothesService = new(fakeClothesRepository.Object);
            Clothes newClothes = new()
            {
                id = existentId,
                name = name,
                idColor = idColor,
                idSize = idSize,
                price = price,
                description = description
            };
            fakeClothesRepository.Setup(r => r.ClothesExists(existentId)).Returns(true);

            // Act
            string actualResult = await clothesService.UpdateClothes(existentId, newClothes);

            // Assert
            fakeClothesRepository.Verify(r => r.UpdateClothes(newClothes), Times.Never);
            Assert.Equal(expectedErrorMsg, actualResult);
        }
    }

}
