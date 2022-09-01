using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using UnitTestWebApi.Models;
using UnitTestWebApi.Repositories;

namespace UnitTestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //TODO inject DB into controller
        private IProductRepository _repository;


        //TODO: Add DI into constructor
        public ProductsController(IProductRepository productRepository)
        {
            _repository = productRepository;
        }

        //All basic CRUD (create,read,update,delete) operations
        //TODO: Implement endpoints
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            //returns 200 Status code with body being the result
            return Ok(_repository.getAllProducts());
        }


        [HttpGet]
        [Route("{id}")]
        public IActionResult GetProduct(int id)
        {
            Product product = _repository.getProduct(id);
            //Notfound's base class (with object) is ObjectResult
            //okobjectresult's base class is ObjectResult (OK with an object parameter is an okobjectresult)
            return product == null ? NotFound(product) : Ok(product);
        }

        [HttpPut]
        public IActionResult UpdateProduct(Product product)
        {
            Product existingProduct = _repository.getProduct(product.ID);
            if(existingProduct != null)
            {
                _repository.updateProduct(product);
                //okobjectresult's base class is ObjectResult
                return Ok(product);
            }
            //Notfound's base class is ObjectResult
            return NotFound(product);
        }


        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            Product existingProduct = _repository.getProduct(product.ID);
            if (existingProduct == null)
            {
                _repository.createProduct(product);
                //return product;
                return Ok(product);
            }
            //conflict's base class is ObjectResult
            return Conflict(existingProduct);
        }

        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
            Product product = _repository.getProduct(id);
            if (product != null)
            {
                _repository.deleteProduct(id);
                //return product;
                //okobjectresult's base class is ObjectResult
                return Ok(product);
                
            }
            //Notfound's base class (with object) is ObjectResult
            return NotFound(product);
        }

    }
}
