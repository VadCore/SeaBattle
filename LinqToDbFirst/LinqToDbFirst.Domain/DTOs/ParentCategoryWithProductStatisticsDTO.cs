using LinqToDbFirst.Domain.DTOs;
using LinqToDbFirst.Domain.Entities;
using LinqToDbFirst.Domain.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable


namespace LinqToDbFirst.Domain.DTOs
{
    public class ParentCategoryWithProductStatisticsDTO
    {
        public string Name { get; init; }
        public IEnumerable<SubCategoryWithProductStatisticsDTO> SubCategoryWithProductStatisticsDTOs { get; init; }
    }
}
