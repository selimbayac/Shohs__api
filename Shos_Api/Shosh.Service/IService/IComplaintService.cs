using Shosh.Core.Entities;
using Shosh.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.IService
{
    public interface IComplaintService
    {
        Task<bool> AddComplaintAsync(int userId, int targetUserId, string content, string reason = "Sebep belirtilmedi");
        Task<List<ComplaintDto>> GetAllComplaintsAsync();
        Task<bool> ResolveComplaintAsync(int complaintId);
    }
}
