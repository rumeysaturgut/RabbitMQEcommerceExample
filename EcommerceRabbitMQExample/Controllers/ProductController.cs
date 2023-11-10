using EcommerceRabbitMQExample.Events.Interface;
using EcommerceRabbitMQExample.Models;
using EcommerceRabbitMQExample.RabbitMQ;
using EcommerceRabbitMQExample.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceRabbitMQExample.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IRabbitMQProducer _rabbitMQProducer;

        public ProductController(IProductService _productService, IRabbitMQProducer rabbitMQProducer)
        {
            productService = _productService;
            _rabbitMQProducer = rabbitMQProducer;
        }

        [HttpGet("productlist")]
        public IEnumerable<Product> ProductList()
        {
            var productList = productService.GetProductList();
            return productList;
        }

        [HttpGet("getproductbyid")]
        public Product GetProductById(int Id)
        {
            return productService.GetProductById(Id);
        }

        [HttpPost("addproduct")]
        public Product AddProduct(Product product)
        {
            var productData = productService.AddProduct(product);
            //send the inserted product data to the queue and consumer will listening this data from queue
            _rabbitMQProducer.SendProductMessage(productData);
            return productData;
        }

        [HttpPut("updateproduct")]
        public Product UpdateProduct(Product product)
        {
            var productData = productService.UpdateProduct(product);
            _rabbitMQProducer.SendProductMessage(productData);
            return productData;
        }
        [HttpDelete("deleteproduct")]
        public bool DeleteProduct(int Id)
        {
            var productData = productService.GetProductById(Id);
            bool result = productService.DeleteProduct(Id);
            _rabbitMQProducer.SendProductMessage(productData);
            return result;
        }
    }
}
