using LinqToDbFirst.Domain.DTOs;
using LinqToDbFirst.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinqToDbFirst.Domain.Interfaces
{
    public interface IProductCategoryRepository : ISimplePrimaryKeyRepository<ProductCategory>
    {
        public IEnumerable<ParentCategoryWithProductStatisticsDTO>
            GetAllProductsWithTotalQtyAndTotalCostGroupByCategory();

        public Task<Dictionary<string, Dictionary<string, List<Product>>>> GetCategoryTreeWithProducts2();

        public Task<IEnumerable<ParentCategoryWithProductStatisticsDTO>> GetCategoryTreeWithProducts3();

        public Task<IEnumerable<ProductCategory>> GetCategoryTreeWithProducts1();
    }
}
