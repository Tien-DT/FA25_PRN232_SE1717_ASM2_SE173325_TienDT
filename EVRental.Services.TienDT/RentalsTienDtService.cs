using EVRental.Repositories.TienDT;
using EVRental.Repositories.TienDT.ModelExtensions;
using EVRental.Repositories.TienDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVRental.Services.TienDT
{
    public class RentalsTienDtService : IRentalsTienDtService

    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly RentalsTienDtRepository _unitOfWork.RentalsTienDtRepository;
        
        public RentalsTienDtService() =>_unitOfWork = new UnitOfWork();
        
        //public RentalsTienDtService(RentalsTienDtRepository repository)
        //{
        //    _unitOfWork.RentalsTienDtRepository = repository;
        //}

        public async Task<int> CreateAsync(RentalsTienDt rentalsTienDt)
        {
            try
            {
                // Business logic validation before creating
                if (!await ValidateRentalDataAsync(rentalsTienDt))
                {
                    throw new Exception("Invalid rental data");
                }

                // Set default values
                rentalsTienDt.CreatedDate = DateTime.Now;
                rentalsTienDt.UpdatedDate = DateTime.Now;
                rentalsTienDt.IsActive = true;
                rentalsTienDt.IsCompleted = false;

                return await _unitOfWork.RentalsTienDtRepository.CreateAsync(rentalsTienDt);
            }
            catch (Exception ex) 
            {
                throw new Exception($"Error creating rental: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var item = await _unitOfWork.RentalsTienDtRepository.GetByIdAsync(id);
                if (item != null)
                {
                    return await _unitOfWork.RentalsTienDtRepository.RemoveAsync(item);
                }
            }
            catch (Exception ex) 
            {
                throw new Exception($"Error deleting rental: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                return await _unitOfWork.RentalsTienDtRepository.SoftDeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error soft deleting rental: {ex.Message}");
            }
        }

        public async Task<List<RentalsTienDt>> GetAllAsync()
        {
            try
            {
                return await _unitOfWork.RentalsTienDtRepository.GetAllAsync();
            }
            catch (Exception ex) 
            {
                throw new Exception($"Error getting all rentals: {ex.Message}");
            }
        }

        public async Task<RentalsTienDt> GetByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.RentalsTienDtRepository.GetByIdAsync(id);
            }
            catch (Exception ex) 
            {
                throw new Exception($"Error getting rental by id: {ex.Message}");
            }
        }

        public async Task<List<RentalsTienDt>> SearchAsync(string note, decimal securityDeposit, string statusName)
        {
            return await _unitOfWork.RentalsTienDtRepository.SearchAsync(note, securityDeposit, statusName);
        }

        public async Task<PaginationResult<List<RentalsTienDt>>> SearchWithPaginationAsync(RentalsTienDtSearchRequest searchRequest)
        {
            try
            {
                return await _unitOfWork.RentalsTienDtRepository.SearchWithPagingAsync(searchRequest);
            }
            catch (Exception ex) 
            {
                throw new Exception($"Error searching with pagination: {ex.Message}");
            }
        }


        public async Task<int> UpdateAsync(RentalsTienDt rentalsTienDt)
        {
            try
            {
                // Business logic validation before updating
                if (!await ValidateRentalDataAsync(rentalsTienDt))
                {
                    throw new Exception("Invalid rental data");
                }

                rentalsTienDt.UpdatedDate = DateTime.Now;
                return await _unitOfWork.RentalsTienDtRepository.UpdateAsync(rentalsTienDt);
            }
            catch (Exception ex) 
            {
                throw new Exception($"Error updating rental: {ex.Message}");
            }
        }

        // Additional business methods implementation
        public async Task<List<RentalsTienDt>> GetRentalsByUserAsync(int userAccountId)
        {
            try
            {
                return await _unitOfWork.RentalsTienDtRepository.GetRentalsByUserAsync(userAccountId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting rentals by user: {ex.Message}");
            }
        }

        public async Task<List<Vehicle>> GetAvailableVehiclesAsync()
        {
            try
            {
                return await _unitOfWork.RentalsTienDtRepository.GetAvailableVehiclesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting available vehicles: {ex.Message}");
            }
        }

        public async Task<List<Station>> GetActiveStationsAsync()
        {
            try
            {
                return await _unitOfWork.RentalsTienDtRepository.GetActiveStationsAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting active stations: {ex.Message}");
            }
        }

        public async Task<List<RentalStatusesTienDt>> GetRentalStatusesAsync()
        {
            try
            {
                return await _unitOfWork.RentalsTienDtRepository.GetRentalStatusesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting rental statuses: {ex.Message}");
            }
        }

        public async Task<List<SystemUserAccount>> GetUserAccountsAsync()
        {
            try
            {
                return await _unitOfWork.RentalsTienDtRepository.GetUserAccountsAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user accounts: {ex.Message}");
            }
        }

        // Business validation methods
        public async Task<bool> ValidateRentalDataAsync(RentalsTienDt rental)
        {
            try
            {
                // Validate required fields
                if (rental.UserAccountId <= 0 || rental.VehicleId <= 0 || rental.StationId <= 0)
                    return false;

                // Validate dates
                if (rental.StartTime == default(DateTime))
                    return false;

                if (rental.PlannedEndTime.HasValue && rental.PlannedEndTime <= rental.StartTime)
                    return false;

                // Validate amounts
                if (rental.SecurityDeposit < 0 || (rental.TotalAmount.HasValue && rental.TotalAmount < 0))
                    return false;

                // Validate note is not empty
                if (string.IsNullOrWhiteSpace(rental.Note))
                    return false;

                // Validate foreign key relationships
                if (!await IsUserAccountValidAsync(rental.UserAccountId))
                    return false;

                if (!await IsVehicleAvailableAsync(rental.VehicleId))
                    return false;

                if (!await IsStationActiveAsync(rental.StationId))
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> IsVehicleAvailableAsync(int vehicleId)
        {
            try
            {
                var vehicles = await _unitOfWork.RentalsTienDtRepository.GetAvailableVehiclesAsync();
                return vehicles.Any(v => v.VehicleId == vehicleId);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> IsStationActiveAsync(int stationId)
        {
            try
            {
                var stations = await _unitOfWork.RentalsTienDtRepository.GetActiveStationsAsync();
                return stations.Any(s => s.StationId == stationId);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> IsUserAccountValidAsync(int userAccountId)
        {
            try
            {
                var users = await _unitOfWork.RentalsTienDtRepository.GetUserAccountsAsync();
                return users.Any(u => u.UserAccountId == userAccountId);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
