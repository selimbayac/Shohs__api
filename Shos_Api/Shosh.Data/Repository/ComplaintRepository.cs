using Microsoft.EntityFrameworkCore;
using Shosh.Core.Entities;

using Shosh.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Data.Repository
{
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly ApplicationDbContext _context;

        public ComplaintRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddComplaintAsync(Complaint complaint)
        {
            try
            {
                await _context.Complaints.AddAsync(complaint);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Şikayet eklenirken hata oluştu: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Complaint>> GetAllComplaintsAsync()
        {
            return await _context.Complaints
                .Include(c => c.User)
                .Include(c => c.TargetUser)
                .Include(c => c.Entry)
                .Include(c => c.Comment)
                .Include(c => c.Blog)
                .ToListAsync();
        }

        public async Task<Complaint?> GetComplaintByIdAsync(int complaintId)
        {
            return await _context.Complaints
          .Include(c => c.User) // Şikayet eden kişi
          .Include(c => c.TargetUser) // Şikayet edilen kişi
          .Include(c => c.Entry) // Şikayet edilen entry
          .Include(c => c.Comment) // Şikayet edilen yorum
          .Include(c => c.Blog) // Şikayet edilen blog
          .FirstOrDefaultAsync(c => c.Id == complaintId);
        }
        public async Task RemoveComplaint(int complaintId)
        {
            var complaint = await _context.Complaints.FindAsync(complaintId);
            if (complaint != null)
            {
                _context.Complaints.Remove(complaint);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> ResolveComplaintAsync(int complaintId)
        {
            try
            {
                var complaint = await _context.Complaints.FindAsync(complaintId);
                if (complaint == null) return false;

                _context.Complaints.Remove(complaint);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Şikayet silinirken hata oluştu: {ex.Message}");
                return false;
            }
        }
        // ✅ Şikayet Silme Metodu
        public async Task<bool> RemoveComplaintAsync(int complaintId) // 📌 ✅ Yeni metod eklendi
        {
            var complaint = await _context.Complaints.FindAsync(complaintId);
            if (complaint == null) return false;

            _context.Complaints.Remove(complaint);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
