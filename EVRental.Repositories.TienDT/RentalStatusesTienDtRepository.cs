using EVRental.Repositories.TienDT.Basic;
using EVRental.Repositories.TienDT.DBContext;
using EVRental.Repositories.TienDT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVRental.Repositories.TienDT
{
    public class RentalStatusesTienDtRepository : GenericRepository<RentalStatusesTienDt>
    {
        public RentalStatusesTienDtRepository() { }
        public RentalStatusesTienDtRepository(FA25_PRN232_SE1717_G6_EVRentalContext context) => _context = context;

        public new async Task<List<RentalStatusesTienDt>> GetAllAsync()
        {
            return await _context.RentalStatusesTienDt.ToListAsync();
        }
    }
}
