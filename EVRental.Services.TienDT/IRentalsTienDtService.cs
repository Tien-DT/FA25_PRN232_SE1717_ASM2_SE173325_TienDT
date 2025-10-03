using EVRental.Repositories.TienDT.ModelExtensions;
using EVRental.Repositories.TienDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVRental.Services.TienDT
{
    public interface IRentalsTienDtService
    {
        Task<List<RentalsTienDt>> GetAllAsync();
        Task<RentalsTienDt> GetByIdAsync(int id);

        Task<List<RentalsTienDt>> SearchAsync(string note, decimal securityDeposit, string statusName);
        Task<PaginationResult<List<RentalsTienDt>>> SearchWithPaginationAsync(RentalsTienDtSearchRequest searchRequest);

        Task<int> CreateAsync(RentalsTienDt rentalsTienDt);
        Task<int> UpdateAsync(RentalsTienDt rentalsTienDt);
        Task<bool> DeleteAsync(int id);
        Task<bool> SoftDeleteAsync(int id);

        // Additional business methods
        Task<List<RentalsTienDt>> GetRentalsByUserAsync(int userAccountId);
        Task<List<Vehicle>> GetAvailableVehiclesAsync();
        Task<List<Station>> GetActiveStationsAsync();
        Task<List<RentalStatusesTienDt>> GetRentalStatusesAsync();
        Task<List<SystemUserAccount>> GetUserAccountsAsync();

        // Business validation methods
        Task<bool> ValidateRentalDataAsync(RentalsTienDt rental);
        Task<bool> IsVehicleAvailableAsync(int vehicleId);
        Task<bool> IsStationActiveAsync(int stationId);
        Task<bool> IsUserAccountValidAsync(int userAccountId);
    }
}
