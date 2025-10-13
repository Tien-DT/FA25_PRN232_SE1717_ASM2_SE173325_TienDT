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
            var rentalsTienDt = await serviceProviders.IRentalsTienDtService.GetByIdAsync(input.RentalTienDtid);
            if (rentalsTienDt == null)
            {
                throw new Exception("Rental not found");
            }

            rentalsTienDt.UserAccountId = input.UserAccountId;
            rentalsTienDt.VehicleId = input.VehicleId;
            rentalsTienDt.StationId = input.StationId;
            rentalsTienDt.StartTime = input.StartTime;
            rentalsTienDt.EndTime = input.EndTime;
            rentalsTienDt.PlannedEndTime = input.PlannedEndTime;
            rentalsTienDt.TotalAmount = input.TotalAmount;
            rentalsTienDt.SecurityDeposit = input.SecurityDeposit;
            rentalsTienDt.Note = input.Note ?? string.Empty;
            rentalsTienDt.RentalStatusTienDtid = input.RentalStatusTienDtid;
            rentalsTienDt.IsCompleted = input.IsCompleted;
            rentalsTienDt.IsActive = input.IsActive;
            
            return await serviceProviders.IRentalsTienDtService.UpdateAsync(rentalsTienDt);
        }

        public async Task<bool> DeleteRentalsTienDt(int rentalTienDtid) 
        {
            return await serviceProviders.IRentalsTienDtService.DeleteAsync(rentalTienDtid);
        }
    }
}
