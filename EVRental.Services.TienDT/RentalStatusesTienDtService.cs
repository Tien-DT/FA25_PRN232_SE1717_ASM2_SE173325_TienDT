using EVRental.Repositories.TienDT;
using EVRental.Repositories.TienDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVRental.Services.TienDT
{
    public class RentalStatusesTienDtService
    {
        private readonly RentalStatusesTienDtRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public RentalStatusesTienDtService() => _repository = new RentalStatusesTienDtRepository();

        public async Task<List<Repositories.TienDT.Models.RentalStatusesTienDt>> GetAllAsync()
        {
            try
            {
                return await _unitOfWork.RentalStatusesTienDtRepository.GetAllAsync();
            }
            catch (Exception ex) { }
            return new List<RentalStatusesTienDt>();
        }


    }
}
