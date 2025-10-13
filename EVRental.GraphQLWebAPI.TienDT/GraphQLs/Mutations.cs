using EVRental.GraphQLWebAPI.TienDT.GraphQLs.InputTypes;
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

        public async Task<int> CreateRentalsTienDt(CreateRentalInput input)
        {
            var rentalsTienDt = new RentalsTienDt
            {
                UserAccountId = input.UserAccountId,
                VehicleId = input.VehicleId,
                StationId = input.StationId,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                PlannedEndTime = input.PlannedEndTime,
                TotalAmount = input.TotalAmount,
                SecurityDeposit = input.SecurityDeposit,
                Note = input.Note ?? string.Empty,
                RentalStatusTienDtid = input.RentalStatusTienDtid,
                IsCompleted = input.IsCompleted,
                IsActive = input.IsActive
            };
            
            return await serviceProviders.IRentalsTienDtService.CreateAsync(rentalsTienDt);
        }

        public async Task<int> UpdateRentalsTienDt(UpdateRentalInput input)
        {
            var rentalsTienDt = new RentalsTienDt
            {
                RentalTienDtid = input.RentalTienDtid,
                UserAccountId = input.UserAccountId,
                VehicleId = input.VehicleId,
                StationId = input.StationId,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                PlannedEndTime = input.PlannedEndTime,
                TotalAmount = input.TotalAmount,
                SecurityDeposit = input.SecurityDeposit,
                Note = input.Note ?? string.Empty,
                RentalStatusTienDtid = input.RentalStatusTienDtid,
                IsCompleted = input.IsCompleted,
                IsActive = input.IsActive
            };
            
            return await serviceProviders.IRentalsTienDtService.UpdateAsync(rentalsTienDt);
        }

        public async Task<bool> DeleteRentalsTienDt(int rentalTienDtid) 
        {
            return await serviceProviders.IRentalsTienDtService.DeleteAsync(rentalTienDtid);
        }
    }
}
