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
    public class ProductRepository : SimplePrimaryKeyRepository<Product>, IProductRepository
    {

        public ProductRepository(AdventureWorksLT2019Context context) : base(context){}

        public async Task<IDictionary<int, (int TotalQty, decimal TotalCost)>> GetAllProductSaleStatistics()
        {
            var productSaleStatistics = await _context.Set<SalesOrderDetail>().Where(od => od.SalesOrder.Status == 5)
                .GroupBy(od => od.ProductId, od => od)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQty = g.Sum(od => od.OrderQty),
                    TotalCost = g.Sum(od => od.OrderQty * od.UnitPrice)
                }).ToDictionaryAsync(d=> d.ProductId, v=> (v.TotalQty, v.TotalCost));

            return productSaleStatistics;


            // TODO: вынести в репозиторий SalesOrderDetail использовать entities
        }
    }
}
