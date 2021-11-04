using LinqToDbFirst.Domain.Entities;
using LinqToDbFirst.Application.Services.Interfaces;
using LinqToDbFirst.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinqToDbFirst.Domain.DTOs;

namespace LinqToDbFirst.Application.Services
{
    public class ProductCategoryService : Service, IProductCategoryService
    {
        private IProductCategoryRepository _productCategory;

        public ProductCategoryService(IProductCategoryRepository productCategory, IMapper mapper) : base(mapper)
        {
            _productCategory = productCategory;
        }


        public async Task<IEnumerable<ProductCategoryDTO>> GetAllProductCategories()
        {


            var productCategories = await _productCategory.GetAll();

            return _mapper.Map<IEnumerable<ProductCategory>, List<ProductCategoryDTO>>(productCategories);
        }

        public async Task<IEnumerable<ProductCategoryDTO>> GetAllProductsWithTotalQtyAndTotalCost()
        {
            var productCategories = await _productCategory.GetAll();

            return _mapper.Map<IEnumerable<ProductCategory>, List<ProductCategoryDTO>>(productCategories);
        }
       
        public IEnumerable<ParentCategoryWithProductStatisticsDTO> 
            GetAllProductsWithTotalQtyAndTotalCostGroupByCategory()
        {
            return _productCategory.GetAllProductsWithTotalQtyAndTotalCostGroupByCategory();
        }
    }
}
