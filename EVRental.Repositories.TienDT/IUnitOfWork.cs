using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVRental.Repositories.TienDT
{
    public interface IUnitOfWork
    {
        SystemUserAccountRepository UserAccountRepository { get; }
        RentalsTienDtRepository RentalsTienDtRepository { get; }
        RentalStatusesTienDtRepository RentalStatusesTienDtRepository { get; }
        int SaveChangesWithTransaction();
        Task<int> SaveChangesWithTransactionAsync();
    }
}
