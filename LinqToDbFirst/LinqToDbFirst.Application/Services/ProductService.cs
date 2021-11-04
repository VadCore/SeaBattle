using LinqToDbFirst.Domain.Entities;
using LinqToDbFirst.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDbFirst.Domain.Interfaces;
using AutoMapper;
using LinqToDbFirst.Domain.DTOs;

namespace LinqToDbFirst.Application.Services
{
    public class ProductService : Service, IProductService
    {
        private IProductRepository _products;
        private IProductCategoryRepository _productCategories;

        public ProductService(IProductRepository products, IMapper mapper, IProductCategoryRepository productCategories) : base(mapper)
        {
            _products = products;
            _productCategories = productCategories;
        }


        public async Task<IEnumerable<ProductDTO>> GetAllProducts()
        {
        
            var products = await _products.GetAll();

            return _mapper.Map<IEnumerable<Product>, List<ProductDTO>>(products);
        }



        public async Task<Dictionary<string, Dictionary<string, List<ProductSaleStatisticsDTO>>>>
            GetAllProductSaleStatisticsGroupByCategories()
        {
            var productSaleStatistics = await _products.GetAllProductSaleStatistics();

            var categoryTree = await _productCategories.GetCategoryTreeWithProducts2();

            var result = categoryTree
                .ToDictionary(pc => pc.Key, pc => pc.Value
                     .ToDictionary(sc => sc.Key, sc => sc.Value
                         .Select(p => new ProductSaleStatisticsDTO
                         {
                             Name = p.Name,
                             Color = p.Color,
                             ProductNumber = p.ProductNumber,
                             Size = p.Size,
                             Weight = p.Weight,
                             TotalQty = productSaleStatistics.TryGetValue(p.ProductId, out (int TotalQty, decimal TotalCost) statistics)
                                        ? statistics.TotalQty : 0,
                             TotalCost = statistics.TotalCost,
                         }).ToList()));

            return result;
        }


        public async Task<IEnumerable<ParentCategoryWithProductStatisticsDTO>> GetAllProductSaleStatisticsGroupByCategories1()
        {
            var productSaleStatistics = await _products.GetAllProductSaleStatistics();

            var categoryTree = await _productCategories.GetCategoryTreeWithProducts1();

            var result = categoryTree
                .Select(pc => new ParentCategoryWithProductStatisticsDTO
                {
                    Name = pc.Name,
                    SubCategoryWithProductStatisticsDTOs = pc.InverseParentProductCategory
                     .Select(sc => new SubCategoryWithProductStatisticsDTO
                     {
                         Name = sc.Name,
                         ProductSaleStatisticsDTOs = sc.Products
                         .Select(p => new ProductSaleStatisticsDTO
                         {
                             Name = p.Name,
                             Color = p.Color,
                             ProductNumber = p.ProductNumber,
                             Size = p.Size,
                             Weight = p.Weight,
                             TotalQty = productSaleStatistics.TryGetValue(p.ProductId, out (int TotalQty, decimal TotalCost) statistics)
                                        ? statistics.TotalQty : 0,
                             TotalCost = statistics.TotalCost,
                         })
                     })
                });

            return result;
        }


        public async Task<IEnumerable<ParentCategoryWithProductStatisticsDTO>> GetAllProductSaleStatisticsGroupByCategories2()
        {
            var productSaleStatistics = await _products.GetAllProductSaleStatistics();

            var categoryTree = await _productCategories.GetCategoryTreeWithProducts2();

            var result = categoryTree
                .Select(pc => new ParentCategoryWithProductStatisticsDTO
                {
                    Name = pc.Key,
                    SubCategoryWithProductStatisticsDTOs = pc.Value
                     .Select(sc => new SubCategoryWithProductStatisticsDTO
                     {
                         Name = sc.Key,
                         ProductSaleStatisticsDTOs = sc.Value
                         .Select(p => new ProductSaleStatisticsDTO
                         {
                             Name = p.Name,
                             Color = p.Color,
                             ProductNumber = p.ProductNumber,
                             Size = p.Size,
                             Weight = p.Weight,
                             TotalQty = productSaleStatistics.TryGetValue(p.ProductId, out (int TotalQty, decimal TotalCost) statistics)
                                        ? statistics.TotalQty : 0,
                             TotalCost = statistics.TotalCost,
                         })
                     })
                });

            return result;
        }

        public async Task<IEnumerable<ParentCategoryWithProductStatisticsDTO>> GetAllProductSaleStatisticsGroupByCategories3()
        {
            var productSaleStatistics = await _products.GetAllProductSaleStatistics();

            var categoryTree = await _productCategories.GetCategoryTreeWithProducts3();

            foreach (var pc in categoryTree)
            {
                foreach (var sc in pc.SubCategoryWithProductStatisticsDTOs)
                {
                    foreach (var ps in sc.ProductSaleStatisticsDTOs)
                    {
                        if (productSaleStatistics.TryGetValue(ps.Id, out (int TotalQty, decimal TotalCost) statistics))
                        {
                            ps.TotalQty = statistics.TotalQty;
                            ps.TotalCost = statistics.TotalCost;
                        }
                    }
                }
            }

            return categoryTree;
        }
    }
}
