using LinqToDbFirst.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinqToDbFirst.Application.Services.Interfaces
{
    public interface IProductCategoryService
    {
        public Task<IEnumerable<ProductCategoryDTO>> GetAllProductCategories();

        //public Task<IEnumerable<ProductCategoryDTO>> GetAllProductsWithTotalQtyAndTotalCost();
        public IEnumerable<ParentCategoryWithProductStatisticsDTO>
            GetAllProductsWithTotalQtyAndTotalCostGroupByCategory();
    }
}
