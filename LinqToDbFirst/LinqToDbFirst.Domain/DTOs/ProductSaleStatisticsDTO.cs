using LinqToDbFirst.Domain.Entities;
using LinqToDbFirst.Domain.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable


namespace LinqToDbFirst.Domain.DTOs
{
    public class ProductSaleStatisticsDTO
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string ProductNumber { get; init; }
        public string Color { get; init; }
        public string Size { get; init; }
        public decimal? Weight { get; init; }
        public int TotalQty { get; set; }
        public decimal TotalCost { get; set; }
    }
}
