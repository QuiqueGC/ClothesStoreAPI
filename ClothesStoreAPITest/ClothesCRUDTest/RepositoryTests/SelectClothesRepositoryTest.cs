using ClothesStoreAPI.Models;
using ClothesStoreAPI.Repository.DB;
using ClothesStoreAPI.Repository.DB.Clothes;
using ClothesStoreAPI.Service.Clothes;
using ClothesStoreAPITest.TestUtils;
using Moq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ClothesStoreAPITest.ClothesCRUDTest.RepositoryTests
{
    public class SelectClothesRepositoryTest
    {
        [Fact]
        public void GetAllClothes_ShouldReturnAllClothes()
        {
            // Arrange
            Mock<IClothesRepository> fakeClothesRepository = new();
            ClothesService clothesService = new(fakeClothesRepository.Object);

            List<Clothes> expectedClothesList = new()
            {
            new Clothes { id = 1, name = "lore ipsum", idColor = 1, idSize = 1, price = 29.99, description = "lore ipsum" },
            new Clothes { id = 2, name = "lore ipsum", idColor = 2, idSize = 1, price = 29.99, description = "lore ipsum" },
            new Clothes { id = 3, name = "lore ipsum", idColor = 3, idSize = 1, price = 29.99, description = "lore ipsum" }
        };

            IQueryable<Clothes> queryableClothesList = expectedClothesList.AsQueryable();
            fakeClothesRepository.Setup(r => r.GetClothes()).Returns(queryableClothesList);


            // Act
            IQueryable<Clothes> queryableActualClothesList = clothesService.GetClothes();
            List<Clothes> actualClothesList = queryableActualClothesList.ToList();


            // Assert
            Assert.Equal(expectedClothesList.Count, actualClothesList.Count);
            Assert.Equal(queryableClothesList, queryableActualClothesList);
        }


        [Theory]
        [InlineData("a", 1)]
        [InlineData("sh", 2)]
        [InlineData("z", 0)]
        [InlineData("shoes", 1)]
        public async Task FindClothesByName_ShouldReturnJustCLothesWithNameContained(string nameToFind, int expectedQuantity)
        {
            // Arrange
            Mock<IClothesStoreEntities> fakeDB = new();
            ClothesRepository clothesRepository = new(fakeDB.Object);

            IQueryable<Clothes> clothesAtDB = SetupCLothesAtDB();

            Mock<DbSet<Clothes>> mockSet = new();
            SetupMockSet(clothesAtDB, mockSet);

            fakeDB.Setup(c => c.Clothes).Returns(mockSet.Object);


            // Act
            List<Clothes> actualClothesList = await clothesRepository.FindClothesByName(nameToFind);


            // Assert
            Assert.Equal(expectedQuantity, actualClothesList.Count);
        }


        private static void SetupMockSet(IQueryable<Clothes> clothesAtDB, Mock<DbSet<Clothes>> mockSet)
        {
            mockSet.As<IDbAsyncEnumerable<Clothes>>()
            .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Clothes>(clothesAtDB.GetEnumerator()));

            mockSet.As<IQueryable<Clothes>>()
            .Setup(m => m.Provider)
            .Returns(new TestDbAsyncQueryProvider<Clothes>(clothesAtDB.Provider));

            mockSet.As<IQueryable<Clothes>>().Setup(m => m.Expression).Returns(clothesAtDB.Expression);
            mockSet.As<IQueryable<Clothes>>().Setup(m => m.ElementType).Returns(clothesAtDB.ElementType);
            mockSet.As<IQueryable<Clothes>>().Setup(m => m.GetEnumerator()).Returns(() => clothesAtDB.GetEnumerator());
        }


        private static IQueryable<Clothes> SetupCLothesAtDB()
        {
            return new List<Clothes>
            {
                new Clothes { id = 1, name = "Hat", idColor = 1, idSize = 1, price = 29.99, description = "lore ipsum" },
                new Clothes { id = 2, name = "Shoes", idColor = 2, idSize = 1, price = 29.99, description = "lore ipsum" },
                new Clothes { id = 3, name = "Shirt", idColor = 3, idSize = 1, price = 29.99, description = "lore ipsum" }
            }.AsQueryable();
        }
    }
}
