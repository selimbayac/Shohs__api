using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shosh.Service.Dto
{
    public class TopicDto
    {
        public int Id { get; set; }  // Başlık ID'si
        public string Title { get; set; } = string.Empty;  // Başlık adı
        public DateTime CreatedAt { get; set; }  // Başlık oluşturulma tarihi
        public List<EntryDto> Entries { get; set; } = new();  // Başlık altındaki entry'ler
    }
}
