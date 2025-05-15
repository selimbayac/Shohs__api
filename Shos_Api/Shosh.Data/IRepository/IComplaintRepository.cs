using Shosh.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Data.IRepository
{
    public interface IComplaintRepository
    {
        Task AddComplaintAsync(Complaint complaint);// dto kullancaz
        Task<Complaint?> GetComplaintByIdAsync(int complaintId);
        Task<List<Complaint>> GetAllComplaintsAsync();
        Task<bool> RemoveComplaintAsync(int complaintId);
    }
}
