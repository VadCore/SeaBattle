using LinqToDbFirst.Domain.Entities;
using LinqToDbFirst.Domain.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable


namespace LinqToDbFirst.Domain.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; init; }
        public string Name { get; init; }
        public string ProductNumber { get; init; }
        public string Color { get; init; }
        public decimal StandardCost { get; init; }
        public decimal ListPrice { get; init; }
        public string Size { get; init; }
        public decimal? Weight { get; init; }
        public int? ProductCategoryId { get; init; }
        public int? ProductModelId { get; init; }
        public DateTime SellStartDate { get; init; }
        public DateTime? SellEndDate { get; init; }
        public DateTime? DiscontinuedDate { get; init; }
        public byte[] ThumbNailPhoto { get; init; }
        public string ThumbnailPhotoFileName { get; init; }
        public DateTime ModifiedDate { get; init; }

    }
}
