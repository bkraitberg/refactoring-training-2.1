namespace UnitTestProject
{
    using NUnit.Framework;
    using Refactoring;
    using RefactoringServiceInterface;
    using RefactoringServices;
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;

    [TestFixture]
    public class UnitTests
    {
        private IUserRepository users;
        private IUserRepository originalUsers;
        private IProductRepository products;
        private IProductRepository originalProducts;

        [SetUp]
        public void Test_Initialize()
        {
            // Load users from data file
            originalUsers = new UserRepository(@"Data/Users.json");
            users = new UserRepository(@"Data/Users.json");

            originalProducts = new ProductRepository(@"Data/Products.json");
            products = new ProductRepository(@"Data/Products.json");
        }

        [TearDown]
        public void Test_Cleanup()
        {
            originalUsers.SaveChanges();
            users = new UserRepository(@"Data/Users.json");

            originalProducts.SaveChanges();
            products = new ProductRepository(@"Data/Products.json");
        }

        [Test]
        public void Test_StartingTuscFromMainDoesNotThrowAnException()
        {
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfa\r\n1\r\n1\r\n8\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    Program.Main(new string[] { });
                }
            }
        }

        [Test]
        public void Test_TuscDoesNotThrowAnException()
        {
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfa\r\n1\r\n1\r\n8\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    Tusc tusc = new Tusc(users, products);
                    tusc.Start();
                }
            }
        }

        [Test]
        public void Test_InvalidUserIsNotAccepted()
        {
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Joel\r\n"))
                {
                    Console.SetIn(reader);

                    Tusc tusc = new Tusc(users, products);
                    tusc.Start();
                }

                Assert.IsTrue(writer.ToString().Contains("You entered an invalid user"));
            }
        }

        [Test]
        public void Test_EmptyUserDoesNotThrowAnException()
        {
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    Tusc tusc = new Tusc(users, products);
                    tusc.Start();
                }
            }
        }

        [Test]
        public void Test_InvalidPasswordIsNotAccepted()
        {
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfb\r\n"))
                {
                    Console.SetIn(reader);

                    Tusc tusc = new Tusc(users, products);
                    tusc.Start();
                }

                Assert.IsTrue(writer.ToString().Contains("You entered an invalid password"));
            }
        }

        [Test]
        public void Test_UserCanCancelPurchase()
        {
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfa\r\n1\r\n0\r\n8\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    Tusc tusc = new Tusc(users, products);
                    tusc.Start();
                }

                Assert.IsTrue(writer.ToString().Contains("Purchase cancelled"));

            }
        }

        [Test]
        public void Test_ErrorOccursWhenBalanceLessThanPrice()
        {
            users.GetAllUsers().Where(u => u.Name == "Jason").Single().Balance = 0.0;

            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfa\r\n1\r\n1\r\n8\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    Tusc tusc = new Tusc(users, products);
                    tusc.Start();
                }

                Assert.IsTrue(writer.ToString().Contains("You do not have enough money to buy that"));
            }
        }

        [Test]
        public void Test_ErrorOccursWhenProductOutOfStock()
        {
            products.GetAllProducts().Where(u => u.Name == "Chips").Single().Quantity = 0;

            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfa\r\n1\r\n1\r\n8\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    Tusc tusc = new Tusc(users, products);
                    tusc.Start();
                }

                Assert.IsTrue(writer.ToString().Contains("is out of stock"));
            }
        }

        [Test]
        public void Test_ProductListContainsExitItem()
        {
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfa\r\n1\r\n1\r\n8\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    Tusc tusc = new Tusc(users, products);
                    tusc.Start();
                }

                Assert.IsTrue(writer.ToString().Contains("8: Exit"));
            }
        }

        private static T DeepCopy<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;

                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
