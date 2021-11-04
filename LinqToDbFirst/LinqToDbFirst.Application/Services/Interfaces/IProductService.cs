using LinqToDbFirst.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinqToDbFirst.Application.Services.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDTO>> GetAllProducts();

        public Task<Dictionary<string, Dictionary<string, List<ProductSaleStatisticsDTO>>>> 
            GetAllProductSaleStatisticsGroupByCategories();

        public Task<IEnumerable<ParentCategoryWithProductStatisticsDTO>> 
            GetAllProductSaleStatisticsGroupByCategories1();


        public Task<IEnumerable<ParentCategoryWithProductStatisticsDTO>>
            GetAllProductSaleStatisticsGroupByCategories2();

        public Task<IEnumerable<ParentCategoryWithProductStatisticsDTO>>
            GetAllProductSaleStatisticsGroupByCategories3();
    }
}
