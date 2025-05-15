namespace Shosh.API.ViewModel
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string VerificationCode { get; set; }
        public string NewPassword { get; set; }
    }

}
