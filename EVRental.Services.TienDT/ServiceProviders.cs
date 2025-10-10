using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVRental.Services.TienDT
{
    public class ServiceProviders : IServiceProviders
    {
        SystemUserAccountService _systemUserAccountService;
        RentalsTienDtService _rentalsTienDtService;
        RentalStatusesTienDtService _rentalStatusesTienDtService;

        public ServiceProviders()
        {

        }

        public SystemUserAccountService SystemUserAccountService { get { return new SystemUserAccountService()?? _systemUserAccountService; } }

        public IRentalsTienDtService IRentalsTienDtService {
            get{return new RentalsTienDtService()?? new RentalsTienDtService();
            }
        }


        public IRentalStatusesTienDtService RentalStatusesTienDtService
        {
            get { return new RentalStatusesTienDtService(); }
        }


       
    }
}
