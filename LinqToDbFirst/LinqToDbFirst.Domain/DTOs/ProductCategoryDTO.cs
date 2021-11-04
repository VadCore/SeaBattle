using LinqToDbFirst.Domain.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable

namespace LinqToDbFirst.Domain.DTOs
{
    public partial class ProductCategoryDTO
    {
        

        public int ProductCategoryId { get; set; }
        public int? ParentProductCategoryId { get; set; }
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
