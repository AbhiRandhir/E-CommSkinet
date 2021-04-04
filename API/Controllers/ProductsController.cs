using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productsBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo,
            IGenericRepository<ProductBrand> productsBrandRepo,
            IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productTypeRepo = productTypeRepo;
            _productsBrandRepo = productsBrandRepo;
            _productsRepo = productsRepo;


        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductsToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();

            var products = await _productsRepo.ListAsync(spec);

            //var products = await _productsRepo.ListAllAsync();

            //return Ok(products);

            // return products.Select(product => new ProductsToReturnDto
            // {
            //     Id = product.Id,
            //     Name = product.Name,
            //     Description = product.Description,
            //     PictureUrl = product.PictureUrl,
            //     Price = product.Price,
            //     ProductBrand = product.ProductBrand.Name,
            //     ProductType = product.ProductType.Name
            // }).ToList();

            return Ok(_mapper
                .Map<IReadOnlyList<Product>, IReadOnlyList<ProductsToReturnDto>>(products));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductsToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            //var products = await _productsRepo.GetByIdAsync(id);
            //return Ok(products);
            var product = await _productsRepo.GetEntityWithSpec(spec);

            // return new ProductsToReturnDto
            // {
            //     Id = product.Id,
            //     Name = product.Name,
            //     Description = product.Description,
            //     PictureUrl = product.PictureUrl,
            //     Price = product.Price,
            //     ProductBrand = product.ProductBrand.Name,
            //     ProductType = product.ProductType.Name
            // };

            return _mapper.Map<Product,ProductsToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<ProductBrand>> GetProductsBrands()
        {
            return Ok(await _productsBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }
    }
}