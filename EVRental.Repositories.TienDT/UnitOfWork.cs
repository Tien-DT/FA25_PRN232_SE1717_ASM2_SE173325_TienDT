using EVRental.Repositories.TienDT.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVRental.Repositories.TienDT
{

    public class UnitOfWork : IUnitOfWork

    {
        
        private readonly FA25_PRN232_SE1717_G6_EVRentalContext _context;
        private readonly RentalsTienDtRepository _rentalsTienDtRepository;
        private readonly RentalStatusesTienDtRepository _rentalStatusesTienDtRepository;
        private readonly SystemUserAccountRepository _systemUserAccountRepository;
        public UnitOfWork() => _context??= new FA25_PRN232_SE1717_G6_EVRentalContext();
        public RentalsTienDtRepository RentalsTienDtRepository {
            get { return _rentalsTienDtRepository ?? new RentalsTienDtRepository(_context); }
        }       

        public RentalStatusesTienDtRepository RentalStatusesTienDtRepository
        {
            get { return _rentalStatusesTienDtRepository ?? new RentalStatusesTienDtRepository(_context); }
        }

        public SystemUserAccountRepository UserAccountRepository { get { return _systemUserAccountRepository ?? new SystemUserAccountRepository(_context); } }

        public int SaveChangesWithTransaction()
        {
            int result = -1;

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = _context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }
        public async Task<int> SaveChangesWithTransactionAsync()
        {
            int result = -1;

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

    }
}
