using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestWebApi.Controllers;
using Moq;
using UnitTestWebApi.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UnitTestWebApi.Repositories;
using System.Net;

namespace WebApi.Tests
{
    [TestClass]
    public class ProductsControllerTests
    {
        //Private variables that will be shared througout the class
        private ProductsController _controller;
        //Mocked interface that interacts with the DB, so we don't actually hit the DB
        private Mock<IProductRepository> _repositoryMock;

        //run before each test (ie BeforeEach)
        [TestInitialize]
        public void Setup()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _controller = new ProductsController(_repositoryMock.Object);
        }

        //run after each test (ie AfterEach)
        [TestCleanup]
        public void Cleanup()
        {
            //set variables to null and allow the garbage collector to clean these for us
            _repositoryMock = null;
            _controller = null;
        }
        /*
         * Basic procedure for these tests is going to be
         * 1. any setup that has to be done for Moq (ie manipulating return values so you can test what functionality you want)
         * 2. Call the controller and cast the response as a Object result so we can look at the status code returned
         * 3. Get the actual data from the response through the "Value" attribute
         * 4. All assertions (generally a status code test and tests to ensure the data is correct)
         */
        [TestMethod]
        public void GetAllProducts_WhenTheStoreIsEmpty_ReturnsAnEmptyList()
        {
            //return []
            _repositoryMock.Setup(repository => repository.getAllProducts()).Returns(new List<Product>());

            var expected = new List<Product>();
            ObjectResult response = _controller.GetAllProducts() as ObjectResult;
            List<Product> actual = response.Value as List<Product>;
            Assert.AreEqual(response.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void GetAllProducts_WhenTheStoreIsnotEmpty_ReturnsAListOfProducts()
        {
            //return []
            _repositoryMock.Setup(repository => repository.getAllProducts()).Returns(new List<Product>
            {
            new Product { ID = 1, Name = "Bananas", Price = 1.50M}
            });
            ObjectResult response = _controller.GetAllProducts() as ObjectResult;
            List<Product> actual = response.Value as List<Product>;
            Assert.AreEqual(response.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(1, actual.Count);
        }



        [TestMethod]
        public void GetProduct_WhenTheStoreIsnotEmpty_ReturnsTheProductIfIdIsValid()
        {
            //arrange
            _repositoryMock.Setup(repository => repository.getProduct(1)).Returns(new Product { ID = 1, Name = "Bananas", Price = 1.50M });
            //act
            ObjectResult response = _controller.GetProduct(1) as ObjectResult;
            Product actual = response.Value as Product;
            //assert
            Assert.AreEqual(response.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(1, actual.ID);
        }



        [TestMethod]
        public void GetProduct_WhenTheStoreIsEmpty_ReturnsNull()
        {
            //arrange
            // Do not need to setup mock repo since it will be empty should return null (no values to choose from)
            //act
            ObjectResult response = _controller.GetProduct(1) as ObjectResult;
            Product actual = response.Value as Product;
            //assert
            Assert.AreEqual(response.StatusCode, (int)HttpStatusCode.NotFound);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void UpdateProduct_WhenTheProductDoesNotExist_ReturnsNotFound()
        {
            //arrange
            // Do not need to setup mock repo since it will be empty should return null (no values to choose from)
            //act
            ObjectResult response = _controller.UpdateProduct(new Product { ID = 1 }) as ObjectResult;
            Product actual = response.Value as Product;

            //assert
            Assert.AreEqual(response.StatusCode, (int)HttpStatusCode.NotFound);
            Assert.AreEqual(actual.ID, 1);
        }

        [TestMethod]
        public void UpdateProduct_WhenTheProductDoesExist_ReturnsThatUpdatedProduct()
        {

            //arrange
            //pass any integer returns product
            _repositoryMock.Setup(repository => repository.getProduct(It.IsAny<int>())).Returns(new Product { ID = 1, Name = "Bananas", Price = 1.50M });

            // Do not need to setup mock repo since it will be empty should return null (no values to choose from)
            //act
            ObjectResult response = _controller.UpdateProduct(new Product { ID = 1, Name = "Apples", Price = 2.50M }) as ObjectResult;
            Product actual = response.Value as Product;

            //assert
            Assert.AreEqual(response.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(actual.Name, "Apples");
            Assert.AreEqual(actual.Price, 2.50M);
        }

        [TestMethod]
        public void CreateProduct_WhenTheProductDoesExist_ReturnsAConflict()
        {

            //arrange
            //pass any integer returns product
            _repositoryMock.Setup(repository => repository.getProduct(It.IsAny<int>())).Returns(new Product { ID = 1, Name = "Bananas", Price = 1.50M });

            // Do not need to setup mock repo since it will be empty should return null (no values to choose from)
            //act
            ObjectResult response = _controller.CreateProduct(new Product { ID = 1, Name = "Apples", Price = 2.50M }) as ObjectResult;
            Product actual = response.Value as Product;

            //assert
            //returns the original object as a conflict (object already in db with id)
            Assert.AreEqual(response.StatusCode, (int)HttpStatusCode.Conflict);
            Assert.AreEqual("Bananas", actual.Name);
            Assert.AreEqual(1.50M, actual.Price);
        }

        [TestMethod]
        public void CreateProduct_WhenTheProductDoesNotExist_ReturnsTheNewProduct()
        {

            //arrange
            //pass any integer returns product

            // Do not need to setup mock repo since it will be empty should return null (no values to choose from)
            //act
            ObjectResult response = _controller.CreateProduct(new Product { ID = 1, Name = "Apples", Price = 2.50M }) as ObjectResult;
            Product actual = response.Value as Product;

            //assert
            Assert.AreEqual(response.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(actual.Name, "Apples");
            Assert.AreEqual(actual.Price, 2.50M);
        }

        [TestMethod]
        public void DeleteProduct_WhenTheProductDoesExist_ReturnTheRemovedProduct()
        {
            Product expectedProduct = new Product { ID = 1, Name = "Bananas", Price = 1.50M };
            //arrange
            //pass any integer returns product
            _repositoryMock.Setup(repository => repository.getProduct(It.IsAny<int>())).Returns(expectedProduct);

            // Do not need to setup mock repo since it will be empty should return null (no values to choose from)
            //act
            ObjectResult response = _controller.DeleteProduct(1) as ObjectResult;
            Product actual = response.Value as Product;

            //assert
            Assert.AreEqual(response.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(actual.Name, expectedProduct.Name);
            Assert.AreEqual(actual.Price, expectedProduct.Price);
        }

        [TestMethod]
        public void DeleteProduct_WhenTheProductDoesNotExist_ReturnsNotFound()
        {

            //arrange
            //pass any integer returns product

            // Do not need to setup mock repo since it will be empty should return null (no values to choose from)
            //act
            
            ObjectResult response = _controller.DeleteProduct(1) as ObjectResult;
            Product actual = response.Value as Product;
            //assert
            Assert.AreEqual(response.StatusCode, (int)HttpStatusCode.NotFound);
            Assert.IsNull(actual);
        }
    }
}
