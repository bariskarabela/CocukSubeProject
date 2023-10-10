using CocukSubeProject.Entities;
using CocukSubeProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Claims;

namespace CocukSubeProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;

        public AccountController(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
        }

        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = DoMD5HashedString(model.Password);

                User user = _databaseContext.Users.SingleOrDefault(x => x.Sicil == model.Sicil && x.Password == hashedPassword);

                if (user != null)
                {
                    if (user.Locked)
                    {
                        ModelState.AddModelError(nameof(model.Sicil), "Yetkiniz yok sistem yöneticinizle görüşün.");
                    }

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.FullName));
                    claims.Add(new Claim(ClaimTypes.Role, user.Role));
                    claims.Add(new Claim("Sicil", user.Sicil));
                    claims.Add(new Claim(ClaimTypes.Locality, user.District));


                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Şifre veya kullanıcı adı hatalı.");
                }

            }
            return View(model);
        }

        private string DoMD5HashedString(string s)
        {
            string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
            string salted = s + md5Salt;
            string hashed = salted.MD5();
            return hashed;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_databaseContext.Users.Any(x => x.Sicil == model.Sicil))
                {
                    ModelState.AddModelError(nameof(model.Sicil), "Bu kullanıcı zaten mevcut.");
                    return View(model);
                }

                string hashedPassword = DoMD5HashedString(model.Password);

                User user = new()
                {
                    FullName = model.FullName,

                    Password = hashedPassword,
                    Sicil = model.Sicil,
                    District = model.District,
                };
                _databaseContext.Users.Add(user);
                int affectedRowCount = _databaseContext.SaveChanges();
                if (affectedRowCount == 0)
                {
                    ModelState.AddModelError("", "Kullanıcı eklenemedi.");
                }
                else
                {

                    ViewData["result"] = "UserAddSuccess"; 
                    return View();
                }
            }
            return View(model);
        }
        [Authorize]
        public IActionResult Profile()
        {
            ProfileInfoLoader();

            return View();
        }

        private void ProfileInfoLoader()
        {
            Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userId);

            ViewData["fullName"] = user.FullName;
            ViewData["ProfileImage"] = user.ProfileImageFileName;
        }

        [HttpPost]
        public IActionResult ProfileChangeName([Required(ErrorMessage = "İsim boş olamaz.")][StringLength(50, ErrorMessage = "50 Karakterden büyük olamaz.")] string? name)
        {
            if (ModelState.IsValid)
            {
                Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userId);

                user.FullName = name;
                _databaseContext.SaveChanges();

                ViewData["result"] = "FullNameChanged";
                ProfileInfoLoader();
                return View(nameof(Profile));

                // return RedirectToAction(nameof(Profile));
            }
            ProfileInfoLoader();
            return View(nameof(Profile));
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        public IActionResult ProfileChangePassword([Required(ErrorMessage = "Şifre boş olamaz.")][StringLength(50, ErrorMessage = "50 Karakterden büyük olamaz.")][MinLength(6,ErrorMessage ="Minimum 6 hane.")] string? password)
        {
            if (ModelState.IsValid)
            {
                Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userId);

                string hashedPassword = DoMD5HashedString(password);

                user.Password = hashedPassword;
                _databaseContext.SaveChanges();

                ViewData["result"] = "PasswordChanged";
            }
            ProfileInfoLoader();
            return View(nameof(Profile));
        }
        [HttpPost]
        public IActionResult ProfileChangeImage([Required(ErrorMessage = "Fotoğraf boş olamaz.")] IFormFile file)
        {
            if (ModelState.IsValid)
            {
                Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userId);

                string fileName = $"p_{userId}.jpg";

                Stream stream = new FileStream($"wwwroot/uploads/{fileName}",FileMode.OpenOrCreate);

                file.CopyTo(stream);

                stream.Close();
                stream.Dispose();

                user.ProfileImageFileName = fileName;
                _databaseContext.SaveChanges();
            }
            ProfileInfoLoader();
            return View(nameof(Profile));
        }

 

    }
}
