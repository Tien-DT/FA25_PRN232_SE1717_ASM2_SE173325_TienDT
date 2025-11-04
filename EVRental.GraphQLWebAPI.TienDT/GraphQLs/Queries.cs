using EVRental.Repositories.TienDT.Models;
using EVRental.Repositories.TienDT.ModelExtensions;
using EVRental.Services.TienDT;
using HotChocolate;

namespace EVRental.GraphQLWebAPI.TienDT.GraphQLs
{
    public class Queries
    {
        private readonly IServiceProviders _serviceProviders;
        public Queries(IServiceProviders serviceProviders) => _serviceProviders = serviceProviders ?? new ServiceProviders();

        public async Task<List<RentalsTienDt>> GetRentalsTienDts()
        {
            return await _serviceProviders.IRentalsTienDtService.GetAllAsync();
        }

        [GraphQLName("rentalById")]
        public async Task<RentalsTienDt> GetRentalByIdAsync(int id)
        {
            return await _serviceProviders.IRentalsTienDtService.GetByIdAsync(id);
        }

        public async Task<PaginationResult<List<RentalsTienDt>>> SearchWithPaging(RentalsTienDtSearchRequest searchRequest)
        {
            return await _serviceProviders.IRentalsTienDtService.SearchWithPaginationAsync(searchRequest);
        }
        
        public async Task<List<RentalStatusesTienDt>> GetRentalStatusesTienDts()
        {
            return await _serviceProviders.RentalStatusesTienDtService.GetAllAsync();
        }

        [GraphQLName("login")]
        public async Task<SystemUserAccount> Login(string username, string password)
        {
            return await _serviceProviders.SystemUserAccountService.GetUserAccount(username, password);
        }

    }

}
