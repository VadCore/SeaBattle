using LinqToDbFirst.Domain.DTOs;
using LinqToDbFirst.Domain.Entities;
using LinqToDbFirst.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinqToDbFirst.Infrostructure.Repositories
{
    public class ProductCategoryRepository : SimplePrimaryKeyRepository<ProductCategory>, IProductCategoryRepository
    {

        public ProductCategoryRepository(AdventureWorksLT2019Context context) : base(context){}

        public async Task<IEnumerable<ProductCategory>> GetCategoryTreeWithProducts1()
        {
            return await entities
                .Where(pc => pc.ParentProductCategoryId == null)
                .Include(pc => pc.InverseParentProductCategory)
                .ThenInclude(sc => sc.Products)
                .ToListAsync();
        }

        public async Task<Dictionary<string, Dictionary<string, List<Product>>>> GetCategoryTreeWithProducts2()
        {
            var result = await entities.Where(pc => pc.ParentProductCategoryId == null).Select(pc =>
                new
                {
                    pc.Name,
                    SubCategories = pc.InverseParentProductCategory.Select(sc =>
                        new
                        {
                            sc.Name,
                            sc.Products
                        })
                }).ToDictionaryAsync(pc=> pc.Name, pc=> pc.SubCategories.ToDictionary(sc=> sc.Name, sc=> sc.Products.ToList()));

            return result;
        }

        public async Task<IEnumerable<ParentCategoryWithProductStatisticsDTO>> GetCategoryTreeWithProducts3()
        {
            var result = await entities.Where(pc => pc.ParentProductCategoryId == null).Select(pc =>
                new ParentCategoryWithProductStatisticsDTO
                {
                    Name = pc.Name,
                    SubCategoryWithProductStatisticsDTOs = pc.InverseParentProductCategory.Select(sc =>
                        new SubCategoryWithProductStatisticsDTO
                        {
                            Name = sc.Name,
                            ProductSaleStatisticsDTOs = sc.Products.Select(p =>
                                new ProductSaleStatisticsDTO
                                {
                                    Id = p.ProductId,
                                    Name = p.Name,
                                    Color = p.Color,
                                    ProductNumber = p.ProductNumber,
                                    Size = p.Size,
                                    Weight = p.Weight,
                                }
                            )
                        })
                }).ToListAsync();

            return result;
        }


        public IEnumerable<ParentCategoryWithProductStatisticsDTO>
            GetAllProductsWithTotalQtyAndTotalCostGroupByCategory() // Here try get by one query groupById
        {

            var result = entities.Where(pc => pc.ParentProductCategoryId == null).Select(pc =>
                new ParentCategoryWithProductStatisticsDTO
                {
                    Name = pc.Name,
                     SubCategoryWithProductStatisticsDTOs = pc.InverseParentProductCategory.Select(c => 
                        new SubCategoryWithProductStatisticsDTO
                        {
                            Name = c.Name,
                            ProductSaleStatisticsDTOs = c.Products.Select(p => 
                                new ProductSaleStatisticsDTO
                                {
                                    Name = p.Name,
                                    ProductNumber = p.ProductNumber,
                                    Color = p.Color,
                                    Size = p.Size,
                                    Weight = p.Weight,

                                    TotalQty = p.SalesOrderDetails.Where(od => od.SalesOrder.Status == 5)
                                        .Select(od => (int)od.OrderQty).Sum(),

                                    TotalCost = p.SalesOrderDetails.Where(od => od.SalesOrder.Status == 5)
                                        .Select(od => od.OrderQty * od.UnitPrice).Sum()

                                    //TotalQtyAndCost = p.SalesOrderDetails.Where(od => od.SalesOrder.Status == 5)
                                    //    .GroupBy(od => od.ProductId, od=> od)
                                    //    .Select(g => new 
                                    //    {
                                    //        TotalQty = g.Sum(od=> od.OrderQty), 
                                    //        TotalCost = g.Sum(od => od.OrderQty * od.UnitPrice) 
                                    //    }), 
                                    // GroupBy dont work correctly in EF Core 5
                                    // https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-5.0/breaking-changes#collection-distinct-groupby
                                })
                        })
                });

            var sql = result.ToQueryString(); // 2 times one query OrderQty, no so good

            return result.ToList();
        }
    }
}