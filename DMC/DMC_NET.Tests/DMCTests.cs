using System;
using DMC_NET.Tests.TestEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using DMC_NET.Exceptions;
using System.Threading.Tasks;

namespace DMC_NET.Tests
{
    [TestClass]
    public class DMCTests
    {
        private DMC _instance;

        private TVEntity _testTV1, _testTV2, _testTV3;
        private LaptopEntity _testLaptop1, _testLaptop2, _testLaptop3;
        private ToyEntity _testToy1, _testToy2, _testToy3;

        [TestInitialize]
        public void TestInitialize()
        {
            _instance = new DMC();

            _testTV1 = new TVEntity()
            {
                Price = 1000.00M,
                Model = "VERY COOL 1 !",
                Resolution = "4K"
            };

            _testTV2 = new TVEntity()
            {
                Price = 499.99M,
                Model = "Not so COOL 2 !",
                Resolution = "1920*1080"
            };

            _testTV3 = new TVEntity()
            {
                Price = 199.99M,
                Model = "Cheap one 3",
                Resolution = "1024*720"
            };

            _testLaptop1 = new LaptopEntity()
            {
                Price = 5000.00M,
                Model = "SUPER 1",
                RAM = 128,
                SSD = 2048,
                HDD = 4096
            };

            _testLaptop2 = new LaptopEntity()
            {
                Price = 1999.99M,
                Model = "Middle 2",
                RAM = 32,
                SSD = 512,
                HDD = 1096
            };

            _testLaptop3 = new LaptopEntity()
            {
                Price = 299.99M,
                Model = "Cheap 2",
                RAM = 8,
                SSD = 0,
                HDD = 512
            };

            _testToy1 = new ToyEntity()
            {
                Name = "Lego My Mind",
                MinAge = 12,
                Price = 500.00M
            };

            _testToy2 = new ToyEntity()
            {
                Name = "Lego Star Wars",
                MinAge = 6,
                Price = 120.00M
            };

            _testToy3 = new ToyEntity()
            {
                Name = "Ball",
                MinAge = 2,
                Price = 14.99M
            };
        }

        [TestMethod]
        public void RegisterEntity_Add_ReadById_NoException_Correct_Read()
        {
            // Arrange

            // Act
            var tvRepository = _instance.Register<TVEntity>();

            var id1 = tvRepository.Update(_testTV1);
            var id2 = tvRepository.Update(_testTV2);

            var readResult1 = tvRepository.ReadById(id1);
            var readResult2 = tvRepository.ReadById(id2);

            // Assert
            id1.Should().Be(0, "Identifier sarts from 0");
            id2.Should().Be(1, "Identifier increments by 1");

            readResult1.Should().BeEquivalentTo(_testTV1);
            readResult2.Should().BeEquivalentTo(_testTV2);
        }

        [TestMethod]
        public void RegisterEntities_Add_IdsForDifferentEntitiesStartFrom0()
        {
            // Arrange
            var tvRepository = _instance.Register<TVEntity>();
            var laptopRepository = _instance.Register<LaptopEntity>();
            var toyRepository = _instance.Register<ToyEntity>();

            // Act

            var idTv1 = tvRepository.Update(_testTV1);
            var idTv2 = tvRepository.Update(_testTV2);
            var idTv3 = tvRepository.Update(_testTV3);

            var idLaptop1 = laptopRepository.Update(_testLaptop1);
            var idLaptop2 = laptopRepository.Update(_testLaptop2);
            var idLaptop3 = laptopRepository.Update(_testLaptop3);

            var idToy1 = toyRepository.Update(_testToy1);
            var idToy2 = toyRepository.Update(_testToy2);
            var idToy3 = toyRepository.Update(_testToy3);

            // Assert
            idTv1.Should().Be(0, "Identifier sarts from 0");
            idTv2.Should().Be(1, "Identifier increments by 1");
            idTv3.Should().Be(2, "Identifier increments by 1");

            idLaptop1.Should().Be(0, "Identifier sarts from 0");
            idLaptop2.Should().Be(1, "Identifier increments by 1");
            idLaptop3.Should().Be(2, "Identifier increments by 1");

            idToy1.Should().Be(0, "Identifier sarts from 0");
            idToy2.Should().Be(1, "Identifier increments by 1");
            idToy3.Should().Be(2, "Identifier increments by 1");
        }

        [TestMethod]
        public void RegisterEntities_Add_Update_ReadById_CorrectReadResult()
        {
            // Arrange
            var tvRepository = _instance.Register<TVEntity>();
            var laptopRepository = _instance.Register<LaptopEntity>();
            var toyRepository = _instance.Register<ToyEntity>();

            // Act

            var idTv1 = tvRepository.Update(_testTV1);
            var idTv1AfterUpdate = tvRepository.Update(_testTV2, idTv1);

            var idLaptop1 = laptopRepository.Update(_testLaptop1);
            var idLaptop1AfterUpdate = laptopRepository.Update(_testLaptop2, idLaptop1);

            var idToy1 = toyRepository.Update(_testToy1);
            var idToy1AfterUpdate = toyRepository.Update(_testToy2, idToy1);

            var tvReadResult = tvRepository.ReadById(idTv1AfterUpdate);
            var laptopReadResult = laptopRepository.ReadById(idLaptop1AfterUpdate);
            var toyReadResult = toyRepository.ReadById(idToy1AfterUpdate);

            // Assert
            idTv1.Should().Be(idTv1AfterUpdate, "Id should not be changed after update");
            idLaptop1.Should().Be(idLaptop1AfterUpdate, "Id should not be changed after update");
            idToy1.Should().Be(idToy1AfterUpdate, "Id should not be changed after update");

            tvReadResult.Should().BeEquivalentTo(_testTV2, "Should be updated value");
            laptopReadResult.Should().BeEquivalentTo(_testLaptop2, "Should be updated value");
            toyReadResult.Should().BeEquivalentTo(_testToy2, "Should be updated value");
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void RegisterEntities_Add_UpdateWithIncorrectId_Exception()
        {
            // Arrange
            var tvRepository = _instance.Register<TVEntity>();

            // Act / Assert
            var idTv1 = tvRepository.Update(_testTV1);
            var idTv1AfterUpdate = tvRepository.Update(_testTV2, idTv1 + 1);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void RegisterEntities_Add_ReadWithIncorrectId_Exception()
        {
            // Arrange
            var tvRepository = _instance.Register<TVEntity>();

            // Act / Assert
            var idTv1 = tvRepository.Update(_testTV1);
            var readResult = tvRepository.ReadById(idTv1 + 1);
        }

        [TestMethod]
        public void RegisterTwice_SameRepository()
        {
            // Arrange

            // Act 
            var tvRepository1 = _instance.Register<TVEntity>();
            var tvRepository2 = _instance.Register<TVEntity>();

            //Assert
            tvRepository1.Should().Be(tvRepository2);
        }

        [TestMethod]
        public void RegisterFromDifferentThreads_AllRepositoriesAreTheSame()
        {
            // Arrange
            var tvRepository = _instance.Register<TVEntity>(16);
            var laptopRepository = _instance.Register<LaptopEntity>(16);
            var toyRepository = _instance.Register<ToyEntity>(16);

            // Act / Assert
            Task[] tasks = new Task[16];
            for (int i = 0; i < 16; ++i)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 100000; ++j)
                    {
                        var localTvRepository = _instance.Register<TVEntity>();
                        localTvRepository.Should().Be(tvRepository);

                        var localLaptopRepository = _instance.Register<LaptopEntity>();
                        localLaptopRepository.Should().Be(laptopRepository);

                        var localToyRepository = _instance.Register<ToyEntity>();
                        localToyRepository.Should().Be(toyRepository);
                    }
                });
            }


            Task.WaitAll(tasks);
        }

        [TestMethod]
        public void Register_Add_Update_ReadById_ReadAll_FromDifferentThreads_CorrectReadResult()
        {
            // Arrange

            // Act / Assert
            Task[] tasks = new Task[16];
            for (int i = 0; i < 16; ++i)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 100000; ++j)
                    {
                        // Register
                        var localToyRepository = _instance.Register<ToyEntity>(16, 16 * 2 * 100000);

                        // Add
                        var id1 = localToyRepository.Update(_testToy1);
                        var id2 = localToyRepository.Update(_testToy2);

                        // Update
                        localToyRepository.Update(_testToy3, id2);

                        // ReadById
                        var read1 = localToyRepository.ReadById(id1);
                        var read2 = localToyRepository.ReadById(id2);

                        // Assert
                        read1.Should().BeEquivalentTo(_testToy1);
                        read2.Should().BeEquivalentTo(_testToy3);
                    }
                });
            }


            Task.WaitAll(tasks);

            // Assert
            var toyRepository = _instance.Register<ToyEntity>();
            var readAllResult = toyRepository.ReadAll();

            readAllResult.Count.Should().Be(16 * 2 * 100000);
        }
    }
}
