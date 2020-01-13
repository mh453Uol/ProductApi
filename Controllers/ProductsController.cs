using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RefactorThis.Core.Dtos;
using RefactorThis.Core.Models;
using RefactorThis.Peristence.Repositories.SQLite;

namespace RefactorThis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ILogger logger;

        public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger)
        {
            this.productRepository = productRepository;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult GetAllProduct([FromQuery] string name)
        {
            List<Product> products = new List<Product>();

            if (String.IsNullOrEmpty(name))
            {
                products = this.productRepository.GetAllProducts();
            }
            else
            {
                products = this.productRepository.GetProductsByName(name);
            }

            return Ok(new { Items = products });
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProductById(Guid id)
        {
            var product = this.productRepository.GetProductById(id);

            if (product == null)
            {
                return NotFound(new ErrorMessageDto(404, $"Cant find product {id}"));
            }

            return Ok(product);
        }

        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            product.Create();

            this.productRepository.AddProduct(product);

            return Created($"/{product.Id}", product);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateProduct(Guid id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productExists = this.productRepository.ProductExists(id);

            if (!productExists)
            {
                return NotFound(new ErrorMessageDto(404, $"Cant find product {id}"));
            }

            product.Id = id;

            this.productRepository.UpdateProduct(product);

            return Ok();
        }

        [HttpDelete("{productId}")]
        public ActionResult DeleteProduct(Guid productId)
        {
            var productExists = this.productRepository.ProductExists(productId);

            if (!productExists)
            {
                return NotFound(new ErrorMessageDto(404, $"Cant find product {productId}"));
            }

            this.productRepository.RemoveAllProductOptions(productId);

            this.productRepository.RemoveProduct(productId);

            this.logger.Log(LogLevel.Information, $"Deleting Product Id: {productId}", new { Now = DateTime.Now });

            return Ok();
        }

        [HttpGet("{productId}/options")]
        public ActionResult<List<ProductOption>> GetAllProductOptions(Guid productId)
        {
            var options = this.productRepository.GetAllProductOptions(productId);
            return Ok(new { Items = options });
        }

        [HttpGet("{productId}/options/{optionId}")]
        public ActionResult<ProductOption> GetProductOptionById(Guid productId, Guid optionId)
        {
            var option = this.productRepository.GetProductOptionById(productId, optionId);

            if (option == null)
            {
                return NotFound(new ErrorMessageDto(404, $"Cant find product option {optionId}"));
            }

            return Ok(option);
        }

        [HttpPost("{productId}/options")]
        public ActionResult CreateProductOption(Guid productId, ProductOption option)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productExists = this.productRepository.ProductExists(productId);

            if (!productExists)
            {
                BadRequest(new ErrorMessageDto(404, $"Cant find product {productId}"));
            }

            option.ProductId = productId;

            option.Create();

            this.productRepository.AddProductOption(option);

            return Created($"/{productId}/options/{option.Id}", option);
        }

        [HttpPut("{productId}/options/{id}")]
        public ActionResult UpdateOption(Guid id, ProductOption option)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productExists = this.productRepository.ProductExists(option.ProductId);

            if (!productExists)
            {
                return BadRequest(new ErrorMessageDto(404, $"Cant find product {option.ProductId}"));
            }

            option.Id = id;

            this.productRepository.UpdateProductOption(option);

            return Ok();
        }

        [HttpDelete("{productId}/options/{id}")]
        public ActionResult DeleteOption(Guid productId, Guid id)
        {
            var optionExists = this.productRepository.GetProductOptionById(productId, id);

            if (optionExists == null)
            {
                return BadRequest(new ErrorMessageDto(404, $"Cant find product option {id}"));
            }

            this.productRepository.RemoveProductOption(id);

            this.logger.Log(LogLevel.Information, 
                $"Deleting Product Option Id: {id}, Product Id {productId}", new { Now = DateTime.Now });

            return Ok();
        }

        [Route("/Error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error()
        {
            // Get the details of the exception that occurred
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                // Get which route the exception occurred at
                string routeWhereExceptionOccurred = exceptionFeature.Path;

                // Get the exception that occurred
                Exception exceptionThatOccurred = exceptionFeature.Error;

                logger.Log(LogLevel.Error, exceptionThatOccurred, routeWhereExceptionOccurred);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}