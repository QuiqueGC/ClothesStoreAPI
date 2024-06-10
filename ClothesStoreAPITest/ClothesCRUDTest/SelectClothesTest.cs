using ClothesStoreAPI.Models;
using ClothesStoreAPI.Repository.DB;
using ClothesStoreAPI.Repository.DB.Clothes;
using ClothesStoreAPI.Service.Clothes;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStoreAPITest.ClothesCRUDTest
{
    public class SelectClothesTest
    {
        [Fact]
        public void GetAllClothes_ShouldReturnAllClothes()
        {
            // Arrange
            var fakeClothesRepository = new Mock<IClothesRepository>();
            var clothesService = new ClothesService(fakeClothesRepository.Object);

            var expectedClothesList = new List<Clothes>
        {
            new Clothes { id = 1, name = "lore ipsum", idColor = 1, idSize = 1, price = 29.99, description = "lore ipsum" },
            new Clothes { id = 2, name = "lore ipsum", idColor = 2, idSize = 1, price = 29.99, description = "lore ipsum" },
            new Clothes { id = 3, name = "lore ipsum", idColor = 3, idSize = 1, price = 29.99, description = "lore ipsum" }
        };

            IQueryable<Clothes> queryableClothesList = expectedClothesList.AsQueryable();
            fakeClothesRepository.Setup(r => r.GetClothes()).Returns(queryableClothesList);


            // Act
            var queryableActualClothesList = clothesService.GetClothes();
            var actualClothesList = queryableActualClothesList.ToList();


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
            var fakeDB = new Mock<IClothesStoreEntities>();
            var clothesRepository = new ClothesRepository(fakeDB.Object);

            IQueryable<Clothes> clothesAtDB = setupCLothesAtDB();
            


            var mockSet = new Mock<DbSet<Clothes>>();

            setupMockSet(clothesAtDB, mockSet);
            

            
            fakeDB.Setup(c => c.Clothes).Returns(mockSet.Object);


            // Act
            var actualClothesList = await clothesRepository.FindClothesByName(nameToFind);


            // Assert
            Assert.Equal(expectedQuantity, actualClothesList.Count);
        }



        private void setupMockSet(IQueryable<Clothes> clothesListAtDB, Mock<DbSet<Clothes>> mockSet)
        {
            mockSet.As<IDbAsyncEnumerable<Clothes>>()
            .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Clothes>(clothesListAtDB.GetEnumerator()));

            mockSet.As<IQueryable<Clothes>>()
            .Setup(m => m.Provider)
            .Returns(new TestDbAsyncQueryProvider<Clothes>(clothesListAtDB.Provider));

            mockSet.As<IQueryable<Clothes>>().Setup(m => m.Expression).Returns(clothesListAtDB.Expression);
            mockSet.As<IQueryable<Clothes>>().Setup(m => m.ElementType).Returns(clothesListAtDB.ElementType);
            mockSet.As<IQueryable<Clothes>>().Setup(m => m.GetEnumerator()).Returns(() => clothesListAtDB.GetEnumerator());
        }


        private IQueryable<Clothes> setupCLothesAtDB()
        {
            IQueryable<Clothes> clothesAtDB = new List<Clothes>
            {
                new Clothes { id = 1, name = "Hat", idColor = 1, idSize = 1, price = 29.99, description = "lore ipsum" },
                new Clothes { id = 2, name = "Shoes", idColor = 2, idSize = 1, price = 29.99, description = "lore ipsum" },
                new Clothes { id = 3, name = "Shirt", idColor = 3, idSize = 1, price = 29.99, description = "lore ipsum" }
            }.AsQueryable();

            return clothesAtDB;
        }
    }
}
