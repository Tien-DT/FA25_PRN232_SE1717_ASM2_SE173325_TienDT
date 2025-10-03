using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVRental.Repositories.TienDT.ModelExtensions
{
    public class SearchRequest
    {
        public int? CurrentPage { get; set; }
        public int? PageSize { get; set; }
    }

    public class RentalsTienDtSearchRequest : SearchRequest
    {
        public string? note { get; set; }
        public decimal? securityDeposit { get; set; }
        public string? statusName { get; set; }
    }
}
