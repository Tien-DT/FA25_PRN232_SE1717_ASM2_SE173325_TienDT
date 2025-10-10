using EVRental.Repositories.TienDT.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EVRental.Services.TienDT
{
    public interface IRentalStatusesTienDtService
    {
        Task<List<RentalStatusesTienDt>> GetAllAsync();
    }
}
