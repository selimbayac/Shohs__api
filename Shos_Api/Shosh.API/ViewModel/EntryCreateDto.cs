namespace Shosh.API.ViewModel
{
    public class EntryCreateDto
    {
        public string Content { get; set; }
        public int TopicId { get; set; }  // 🔥 TopicId opsiyonel olmalı
    }
}
