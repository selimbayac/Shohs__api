using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Shosh.API.ViewModel;
using Shosh.Service.IService;
using Shosh.Service.Service;
using System.Security.Claims;

namespace Shosh.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService; // Interface ile Bağlamalıyız!
        private readonly ILikeService _likeService;
        private readonly IConfiguration _configuration;
        private readonly IBanService _banService;

        public UserController(IUserService userService, IConfiguration configuration , IBanService banService, ILikeService likeService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
           
            _configuration = configuration;
            _banService = banService;
            _likeService = likeService;
        }

        // 📌 **1️⃣ Kullanıcı Kaydı**
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var result = await _userService.RegisterAsync(model.Email, model.Password, model.Nickname);

            if (result != null)  // 📌 HATA MESAJI VARSA BAD REQUEST
                return BadRequest(result);

            return Ok(new { Message = "Kayıt başarılı!" });
        }

        //📌 **2️⃣ Kullanıcı Girişi ve JWT Token Alma**
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userService.LoginUserAsync(loginDto.Email, loginDto.Password);

            if (user == null)
                return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı." });

            // 📌 Eğer kullanıcı banlıysa, token yerine ban mesajı dön
            if (user.IsBanned)
            {
                return StatusCode(403, new
                {
                    message = "Banlı olduğunuz için giriş yapabilirsiniz, ancak içerik ekleyemezsiniz.",
                    reason = user.BanReason,
                    expiresAt = user.BanExpirationDate?.ToString("dd.MM.yyyy HH:mm") ?? "Süresiz"
                });
            }

            // 📌 Kullanıcıya JWT Token oluştur
            var token = _userService.GenerateJwtToken(user);

            return Ok(new
            {
                token,
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    nickname = user.Nickname,
                    role = user.Role,
                    isBanned = user.IsBanned
                }
            });
        }

        // 📌 **3️⃣ Kullanıcı Profili Getirme**
      
        [HttpGet("{userId}/profile")]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            var userProfile = await _userService.GetUserProfileAsync(userId);
            if (userProfile == null)
                return NotFound();
            return Ok(userProfile);
        }

        // 📌 **4️⃣ Kullanıcı Güncelleme (E-posta, Nickname, Bio Güncelleme)**
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            int userId = int.Parse(userIdClaim);
            bool success = await _userService.UpdateUserAsync(userId, request.Email, request.Nickname, request.Bio);

            if (!success) return BadRequest("Güncelleme başarısız.");
            return Ok("Profil başarıyla güncellendi.");
        }

        // 📌 **5️⃣ Şifre Değiştirme**
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            int userId = int.Parse(userIdClaim);
            bool success = await _userService.ChangePasswordAsync(userId, request.OldPassword, request.NewPassword);

            if (!success) return BadRequest("Eski şifre yanlış veya güncelleme başarısız.");
            return Ok("Şifre başarıyla değiştirildi.");
        }

        // 📌 **6️⃣ E-posta Doğrulama Kodu Gönderme**
        [HttpPost("send-verification")]
        public async Task<IActionResult> SendVerificationEmail([FromBody] EmailDto request)
        {
            bool success = await _userService.SendEmailVerificationAsync(request.Email);
            if (!success) return NotFound("Kullanıcı bulunamadı.");

            return Ok("Doğrulama kodu e-posta adresine gönderildi.");
        }

        // 📌 **7️⃣ E-posta Doğrulama**
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto request)
        {
            bool success = await _userService.VerifyEmailAsync(request.Email, request.Code);
            if (!success) return BadRequest("Kod hatalı veya süresi dolmuş.");

            return Ok("E-posta doğrulandı!");
        }

        // 📌 **8️⃣ Şifre Sıfırlama Kodu Gönderme**
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] EmailDto request)
        {
            bool success = await _userService.SendPasswordResetCodeAsync(request.Email);
            if (!success) return NotFound("Kullanıcı bulunamadı!");

            return Ok("Şifre sıfırlama kodu e-posta adresine gönderildi.");
        }

        // 📌 **9️⃣ Şifre Sıfırlama**
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            bool success = await _userService.ResetPasswordAsync(request.Email, request.VerificationCode, request.NewPassword);
            if (!success) return BadRequest("Kod hatalı veya süresi dolmuş!");

            return Ok("Şifre başarıyla değiştirildi.");
        }
    }
}