using Shosh.Service.Dto;

namespace Shosh.API.ViewModel
{
    public class TopicDto
    {
        public int Id { get; set; } // Başlık ID'si
        public string Title { get; set; } = string.Empty; // Başlık adı
        public string Description { get; set; } = string.Empty; // Başlık açıklaması (isteğe bağlı)
        public DateTime CreatedAt { get; set; } // Başlık oluşturulma tarihi
        public List<EntryDto> Entries { get; set; } = new(); // Başlık altındaki entry'ler
    }
}
