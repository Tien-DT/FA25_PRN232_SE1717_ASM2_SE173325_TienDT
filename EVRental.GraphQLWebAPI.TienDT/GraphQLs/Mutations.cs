using EVRental.Repositories.TienDT.Models;
using EVRental.Services.TienDT;

namespace EVRental.GraphQLWebAPI.TienDT.GraphQLs
{
    public class Mutations
    {
        private readonly IServiceProviders serviceProviders;

        public Mutations(IServiceProviders serviceProviders)
        {
            this.serviceProviders = serviceProviders;
        }

        public async Task<int> CreateRentalsTienDt(RentalsTienDt rentalsTienDt)
        {
            return await serviceProviders.IRentalsTienDtService.CreateAsync(rentalsTienDt);
        }

        public async Task<int> UpdateRentalsTienDt(RentalsTienDt rentalsTienDt)
        {
            return await serviceProviders.IRentalsTienDtService.UpdateAsync(rentalsTienDt);
        }

        public async Task<bool> DeleteRentalsTienDt(int id) 
        {
            return await serviceProviders.IRentalsTienDtService.DeleteAsync(id);
        }
    }
}
