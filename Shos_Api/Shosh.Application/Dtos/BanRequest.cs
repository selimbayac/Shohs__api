namespace Shosh.Application.Dtos
{
    public class BanRequest
    {
        public string Reason { get; set; }
        public int DurationInDays { get; set; }
    }
}
