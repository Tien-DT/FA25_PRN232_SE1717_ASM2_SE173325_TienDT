using EVRental.Repositories.TienDT;
using EVRental.Repositories.TienDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVRental.Services.TienDT
{
    public class SystemUserAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SystemUserAccountService() => _unitOfWork ??= new UnitOfWork();
        
        //public SystemUserAccountService() => _userAccountRepository = new SystemUserAccountRepository();
        
        //public SystemUserAccountService(SystemUserAccountRepository userAccountRepository)
        //{
        //    _userAccountRepository = userAccountRepository;
        //}

        public async Task<SystemUserAccount> GetUserAccount(string userName, string password)
        {
            try
            {
                return await _unitOfWork.UserAccountRepository.GetUserAccount(userName, password);

            }
            catch (Exception ex) {}
            return null;
        }
    }
}
