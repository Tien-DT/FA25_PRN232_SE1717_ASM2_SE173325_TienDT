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
    public class SystemUserAccountRepository : GenericRepository<SystemUserAccount>
    {
        public SystemUserAccountRepository() { }

        public SystemUserAccountRepository(FA25_PRN232_SE1717_G6_EVRentalContext context) => _context = context;

        public async Task<SystemUserAccount> GetUserAccount(string userName, string password)
        {
            return await _context.SystemUserAccounts.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password && u.IsActive == true);

            //return await _context.SystemUserAccounts.FirstOrDefaultAsync(u => u.Phone == phoneNumber && u.Password == password && u.IsActive == true);

            //return await _context.SystemUserAccounts.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password && u.IsActive == true);
        }
    }
}
