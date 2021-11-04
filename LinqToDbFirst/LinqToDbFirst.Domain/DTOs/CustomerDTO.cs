using LinqToDbFirst.Domain.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable


namespace LinqToDbFirst.Domain.DTOs
{
    public partial class CustomerDTO
    {
        public int CustomerId { get; init; }
        public bool NameStyle { get; init; }
        public string Title { get; init; }
        public string FirstName { get; init; }
        public string MiddleName { get; init; }
        public string LastName { get; init; }
        public string Suffix { get; init; }
        public string CompanyName { get; init; }
        public string SalesPerson { get; init; }
        public string EmailAddress { get; init; }
        public string Phone { get; init; }
        public string PasswordHash { get; init; }
        public string PasswordSalt { get; init; }
        public DateTime ModifiedDate { get; init; }
    }
}
