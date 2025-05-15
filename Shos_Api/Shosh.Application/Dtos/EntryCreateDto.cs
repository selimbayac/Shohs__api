namespace Shosh.Application.Dtos
{
    public class EntryCreateDto
    {
        public string Content { get; set; }
        public int? TopicId { get; set; }  // 🔥 TopicId opsiyonel olmalı
    }
}
