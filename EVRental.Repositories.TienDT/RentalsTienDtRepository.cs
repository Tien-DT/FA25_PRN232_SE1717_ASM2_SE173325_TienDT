using EVRental.Repositories.TienDT.Basic;
using EVRental.Repositories.TienDT.DBContext;
using EVRental.Repositories.TienDT.ModelExtensions;
using EVRental.Repositories.TienDT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVRental.Repositories.TienDT
{
    public class RentalsTienDtRepository : GenericRepository<RentalsTienDt>
    {
        public RentalsTienDtRepository() { }
        public RentalsTienDtRepository(FA25_PRN232_SE1717_G6_EVRentalContext context) => _context = context;

        public new async Task<List<RentalsTienDt>> GetAllAsync()
        {
            var items = await _context.RentalsTienDts
                .Include(c => c.RentalStatusTienDt)
                .Include(c => c.UserAccount)
                .Include(c => c.Vehicle)
                .Include(c => c.Station)
                .Where(c => c.IsActive == true)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
            return items ?? new List<RentalsTienDt>();
        }

        public new async Task<RentalsTienDt> GetByIdAsync(string code)
        {
            var rental = await _context.RentalsTienDts
                .Include(c => c.RentalStatusTienDt)
                .Include(c => c.UserAccount)
                .Include(c => c.Vehicle)
                .Include(c => c.Station)
                .FirstOrDefaultAsync(r => r.RentalTienDtid.ToString() == code && r.IsActive == true);
            return rental!;
        }

        public new async Task<RentalsTienDt> GetByIdAsync(int id)
        {
            var rental = await _context.RentalsTienDts
                .Include(c => c.RentalStatusTienDt)
                .Include(c => c.UserAccount)
                .Include(c => c.Vehicle)
                .Include(c => c.Station)
                .FirstOrDefaultAsync(r => r.RentalTienDtid == id && r.IsActive == true);
            return rental!;
        }

        public async Task<List<RentalsTienDt>> SearchAsync(string note, decimal securityDeposit, string statusName)
        {
            var items = await _context.RentalsTienDts
                .Include(c => c.RentalStatusTienDt)
                .Include(c => c.UserAccount)
                .Include(c => c.Vehicle)
                .Include(c => c.Station)
                .Where(c => c.IsActive == true &&
                    (string.IsNullOrEmpty(note) || c.Note.Contains(note))
                    && (securityDeposit == 0 || c.SecurityDeposit == securityDeposit)
                    && (string.IsNullOrEmpty(statusName) || c.RentalStatusTienDt.StatusName.Contains(statusName))
                )
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            return items ?? new List<RentalsTienDt>();
        }

        // Create with validation
        public new async Task<int> CreateAsync(RentalsTienDt entity)
        {
            try
            {
                entity.CreatedDate = DateTime.Now;
                entity.UpdatedDate = DateTime.Now;
                entity.IsActive = true;
                entity.IsCompleted = false;
                
                _context.Add(entity);
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                // Provide detailed database error information
                var innerMessage = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
                throw new Exception($"Database error while creating rental: {innerMessage}", dbEx);
            }
        }

        // Update with validation
        public new async Task<int> UpdateAsync(RentalsTienDt entity)
        {
            entity.UpdatedDate = DateTime.Now;
            
            _context.ChangeTracker.Clear();
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        // Soft delete
        public async Task<bool> SoftDeleteAsync(int id)
        {
            var rental = await _context.RentalsTienDts.FindAsync(id);
            if (rental != null)
            {
                rental.IsActive = false;
                rental.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Get rental by user account
        public async Task<List<RentalsTienDt>> GetRentalsByUserAsync(int userAccountId)
        {
            var items = await _context.RentalsTienDts
                .Include(c => c.RentalStatusTienDt)
                .Include(c => c.Vehicle)
                .Include(c => c.Station)
                .Where(c => c.UserAccountId == userAccountId && c.IsActive == true)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
            return items ?? new List<RentalsTienDt>();
        }

        // Get available vehicles for rental
        public async Task<List<Vehicle>> GetAvailableVehiclesAsync()
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.VehicleType)
                .Where(v => v.IsAvailable == true && v.IsActive == true)
                .ToListAsync();
            return vehicles ?? new List<Vehicle>();
        }

        // Get active stations
        public async Task<List<Station>> GetActiveStationsAsync()
        {
            var stations = await _context.Stations
                .Where(s => s.IsActive == true && s.IsOperational == true)
                .ToListAsync();
            return stations ?? new List<Station>();
        }

        // Get rental statuses
        public async Task<List<RentalStatusesTienDt>> GetRentalStatusesAsync()
        {
            var statuses = await _context.RentalStatusesTienDt
                .Where(s => s.IsActive == true)
                .ToListAsync();
            return statuses ?? new List<RentalStatusesTienDt>();
        }

        // Get user accounts
        public async Task<List<SystemUserAccount>> GetUserAccountsAsync()
        {
            var users = await _context.SystemUserAccounts
                .ToListAsync();
            return users ?? new List<SystemUserAccount>();
        }


        public async Task<PaginationResult<List<RentalsTienDt>>> SearchWithPagingAsync(RentalsTienDtSearchRequest searchRequest)
        {
            var items = await this.SearchAsync(
                searchRequest.note ?? string.Empty, 
                searchRequest.securityDeposit ?? 0, 
                searchRequest.statusName ?? string.Empty);

            var totalItems = items.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / (searchRequest.PageSize ?? 10));

            items = items.Skip(((searchRequest.CurrentPage ?? 1) - 1) * (searchRequest.PageSize ?? 10))
                        .Take(searchRequest.PageSize ?? 10)
                        .ToList();

            var result = new PaginationResult<List<RentalsTienDt>>
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = searchRequest.CurrentPage ?? 1,
                PageSize = searchRequest.PageSize ?? 10,
                Items = items
            };

            return result ?? new PaginationResult<List<RentalsTienDt>>();
        }

        //public async Task<Task<PaginationResult<List<RentalsTienDt>>>> SearchWithPagingAsync(RentalsTienDtSearchRequest searchRequest)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
