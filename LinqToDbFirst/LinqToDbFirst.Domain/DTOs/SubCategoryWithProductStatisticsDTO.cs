using LinqToDbFirst.Domain.Entities;
using LinqToDbFirst.Domain.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable


namespace LinqToDbFirst.Domain.DTOs
{
    public class SubCategoryWithProductStatisticsDTO
    {
        public string Name { get; init; }
        public IEnumerable<ProductSaleStatisticsDTO> ProductSaleStatisticsDTOs { get; init; }
    }
}
