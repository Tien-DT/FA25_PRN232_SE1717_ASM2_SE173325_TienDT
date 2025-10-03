using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVRental.Services.TienDT
{
    public interface IServiceProviders
    {
        IRentalsTienDtService IRentalsTienDtService { get; }
        RentalStatusesTienDtService RentalStatusesTienDtService { get; }
        SystemUserAccountService SystemUserAccountService { get; }
    }
}
