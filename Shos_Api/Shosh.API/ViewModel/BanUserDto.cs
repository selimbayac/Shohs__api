namespace Shosh.API.ViewModel
{
    public class BanUserDto
    {
        public int? Days { get; set; } // Ban süresi (gün olarak), null ise süresiz
        public string Reason { get; set; } // Ban sebebi
    }
}
