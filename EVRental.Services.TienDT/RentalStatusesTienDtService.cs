using EVRental.Repositories.TienDT;
using EVRental.Repositories.TienDT.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EVRental.Services.TienDT
{
    public class RentalStatusesTienDtService : IRentalStatusesTienDtService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentalStatusesTienDtService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public async Task<List<RentalStatusesTienDt>> GetAllAsync()
        {
            try
            {
                return await _unitOfWork.RentalStatusesTienDtRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting all rental statuses: " + ex.Message);
            }
        }
    }
}
