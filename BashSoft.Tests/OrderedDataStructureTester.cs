namespace BashSoft.Tests
{
    using System;
    using System.Collections.Generic;
    using BashSoft.Contracts;
    using BashSoft.DataStructures;
    using NUnit.Framework;

    [TestFixture]
    public class OrderedDataStructureTester
    {
        private ISimpleOrderedBag<string> names;

        [SetUp]
        public void SetUp()
        {
            this.names = new SimpleSortedList<string>();
        }

        [Test]
        public void TestEmptyConstructor()
        {
            //Arrange
            this.names = new SimpleSortedList<string>();

            //Assert
            Assert.AreEqual(this.names.Capacity, 16);
            Assert.AreEqual(this.names.Size, 0);
        }

        [Test]
        public void TestConstructorWithInitialCapacity()
        {
            //Arrange
            this.names = new SimpleSortedList<string>(20);

            //Assert
            Assert.AreEqual(this.names.Capacity, 20);
            Assert.AreEqual(this.names.Size, 0);
        }

        [Test]
        public void TestConstructorWithAllParameters()
        {
            //Arrange
            this.names = new SimpleSortedList<string>(StringComparer.OrdinalIgnoreCase, 30);

            //Assert
            Assert.AreEqual(this.names.Capacity, 30);
            Assert.AreEqual(this.names.Size, 0);
        }

        [Test]
        public void TestConstructorWithInitialComparer()
        {
            //Arrange
            this.names = new SimpleSortedList<string>(StringComparer.OrdinalIgnoreCase);

            //Assert
            Assert.AreEqual(this.names.Capacity, 16);
            Assert.AreEqual(this.names.Size, 0);
        }

        [Test]
        public void TestIncreaseSize()
        {
            //Act
            this.names.Add("Pesho");

            //Assert
            Assert.AreEqual(this.names.Size, 1);
        }

        [Test]
        public void TestAddNullThrowsException()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() => this.names.Add(null));
        }

        [Test]
        public void TestAddUnsortedDataIsHeldSorted()
        {
            //Arrange
            int indexCounter = 0;

            //Act
            this.names.Add("Rosen");
            this.names.Add("Georgi");
            this.names.Add("Balkan");

            //Assert
            foreach (var name in this.names)
            {
                if (indexCounter == 0)
                {
                    Assert.AreEqual(name, "Balkan");
                }

                if (indexCounter == 1)
                {
                    Assert.AreEqual(name, "Georgi");
                }

                if (indexCounter == 2)
                {
                    Assert.AreEqual(name, "Rosen");
                }

                indexCounter++;
            }
        }

        [Test]
        public void TestAddingMoreThanInitialCapacity()
        {
            //Act
            for (int i = 0; i < 17; i++)
            {
                this.names.Add("Test");
            }

            //Assert
            Assert.AreEqual(this.names.Size, 17);
            Assert.AreNotEqual(this.names.Capacity, 16);
        }

        [Test]
        public void TestAddingAllFromCollectionIncreasesSize()
        {
            //Arrange
            List<string> list = new List<string>() {"Test1", "Test2"};

            //Act
            this.names.AddAll(list);

            //Assert
            Assert.AreEqual(this.names.Size, 2);
        }

        [Test]
        public void TestAddingAllFromNullThrowsException()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() => this.names.AddAll(null));
        }

        [Test]
        public void TestAddAllKeepsSorted()
        {
            //Arrange
            List<string> list = new List<string>() { "Test3", "Test5", "Test1", "Test0" };
            int indexCounter = 0;

            //Act
            this.names.AddAll(list);

            //Assert
            foreach (var name in this.names)
            {
                if (indexCounter == 0)
                {
                    Assert.AreEqual(name, "Test0");
                }

                if (indexCounter == 1)
                {
                    Assert.AreEqual(name, "Test1");
                }

                if (indexCounter == 2)
                {
                    Assert.AreEqual(name, "Test3");
                }

                if (indexCounter == 3)
                {
                    Assert.AreEqual(name, "Test5");
                }

                indexCounter++;
            }
        }

        [Test]
        public void TestRemoveValidElementDecreasesSize()
        {
            //Act
            this.names.Add("Pesho");
            this.names.Remove("Pesho");

            //Assert
            Assert.AreEqual(this.names.Size, 0);
        }

        [Test]
        public void TestRemoveValidElementRemovesSelectedOne()
        {
            //Act
            this.names.Add("Pesho");
            this.names.Add("Gosho");
            this.names.Remove("Pesho");

            //Assert
            foreach (var name in this.names)
            {
                Assert.AreNotEqual(name, "Pesho");
            }
        }

        [Test]
        public void TestRemovingNullThrowsException()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() => this.names.Remove(null));
        }

        [Test]
        public void TestJoinWithNull()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() => this.names.JoinWith(null));
        }

        [Test]
        public void TestJoinWorksFine()
        {
            //Act
            this.names.Add("Pesho");
            this.names.Add("Gosho");
            this.names.Add("Tosho");

            //Assert
            Assert.AreEqual(this.names.JoinWith(", "), "Gosho, Pesho, Tosho");
        }
    }
}