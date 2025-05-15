using Shosh.Core.Entities;

using Shosh.Data.IRepository;
using Shosh.Data.Repository;
using Shosh.Service.Dto;
using Shosh.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Shosh.Service.Service
{
    public class ComplaintService : IComplaintService
    {
        private readonly IComplaintRepository _complaintRepository;
        private readonly IEntryRepository _entryRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IBlogRepository _blogRepository;

        public ComplaintService(IComplaintRepository complaintRepository,
            IEntryRepository entryRepository,
            ICommentRepository commentRepository,
            IBlogRepository blogRepository)
        {
            _complaintRepository = complaintRepository;
            _entryRepository = entryRepository;
            _commentRepository = commentRepository;
            _blogRepository = blogRepository;
        }

        public async Task<bool> AddComplaintAsync(int userId, int targetUserId, string content, string reason = "Sebep belirtilmedi")
        {
            var complaint = new Complaint
            {
                UserId = userId,
                TargetUserId = targetUserId,
                Content = content,
                Reason = reason,
                CreatedAt = DateTime.UtcNow
            };

            await _complaintRepository.AddComplaintAsync(complaint);
            return true;
        }

        public async Task<List<ComplaintDto>> GetAllComplaintsAsync()
        {
            var complaints = await _complaintRepository.GetAllComplaintsAsync();

            return complaints.Select(c => new ComplaintDto
            {
                Id = c.Id,
                UserId = c.UserId,
                TargetUserId = c.TargetUserId,
                Complainant = c.User?.Nickname ?? "Bilinmiyor",
                TargetUser = c.TargetUser?.Nickname ?? "Bilinmiyor",
                Reason = c.Reason,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                EntryContent = c.Entry?.Content,
                CommentContent = c.Comment?.Content,
                BlogTitle = c.Blog?.Title
            }).ToList();
        }

        // 📌 **Şikayeti Çöz & İçeriği Kaldır**
        public async Task<bool> ResolveComplaintAsync(int complaintId)
        {
            var complaint = await _complaintRepository.GetComplaintByIdAsync(complaintId);
            if (complaint == null)
            {
                Console.WriteLine($"❌ HATA: Şikayet bulunamadı. (Complaint ID: {complaintId})");
                return false;
            }

            bool isDeleted = false;

            try
            {
                // 🔍 **Entry varsa kaldır**
                if (complaint.EntryId.HasValue)
                {
                    var entry = await _entryRepository.GetEntryByIdAsync(complaint.EntryId.Value);
                    if (entry != null)
                    {
                        await _entryRepository.DeleteEntryAsync(complaint.EntryId.Value);
                        isDeleted = true;
                        Console.WriteLine($"✅ Entry silindi (ID: {complaint.EntryId.Value})");
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ Entry bulunamadı (ID: {complaint.EntryId.Value}).");
                    }
                }

                // 🔍 **Yorum varsa kaldır**
                if (complaint.CommentId.HasValue)
                {
                    var comment = await _commentRepository.GetCommentByIdAsync(complaint.CommentId.Value);
                    if (comment != null)
                    {
                        await _commentRepository.DeleteCommentAsync(complaint.CommentId.Value);
                        isDeleted = true;
                        Console.WriteLine($"✅ Yorum silindi (ID: {complaint.CommentId.Value})");
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ Yorum bulunamadı (ID: {complaint.CommentId.Value}).");
                    }
                }

                // 🔍 **Blog varsa kaldır**
                if (complaint.BlogId.HasValue)
                {
                    var blog = await _blogRepository.GetBlogByIdAsync(complaint.BlogId.Value);
                    if (blog != null)
                    {
                        await _blogRepository.DeleteBlogAsync(complaint.BlogId.Value);
                        isDeleted = true;
                        Console.WriteLine($"✅ Blog silindi (ID: {complaint.BlogId.Value})");
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ Blog bulunamadı (ID: {complaint.BlogId.Value}).");
                    }
                }

                // **Şikayet edilen içerik bulunamadıysa, şikayeti kaldırma**
                if (!isDeleted)
                {
                    Console.WriteLine("⚠️ Uyarı: Şikayet edilen içerik bulunamadığı için işlem yapılmadı.");
                    return false;
                }

                // 📌 **Şikayeti kaldır**
                await _complaintRepository.RemoveComplaintAsync(complaintId);
                Console.WriteLine($"✅ Şikayet kaldırıldı (ID: {complaintId})");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 HATA: Şikayet çözülürken bir hata oluştu. Hata: {ex.Message}");
                return false;
            }
        }
        }
  }
