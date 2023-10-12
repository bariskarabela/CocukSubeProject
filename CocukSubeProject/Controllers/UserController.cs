using AutoMapper;
using CocukSubeProject.Entities;
using CocukSubeProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;
using System.Data;

namespace CocukSubeProject.Controllers
{
    public class UserController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserController(DatabaseContext databaseContext, IMapper mapper, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
            _configuration = configuration;

        }
        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            List<UserModel> users = _databaseContext.Users.ToList()
                .Select(x => _mapper.Map<UserModel>(x)).ToList();

            return View(users);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Edit(Guid id)
        {
            User user = _databaseContext.Users.Find(id);
            EditUserModel model = _mapper.Map<EditUserModel>(user);

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(Guid id, EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (_databaseContext.Users.Any(x => x.Sicil == model.Sicil && x.Id != id))
                {
                    ModelState.AddModelError(nameof(model.Sicil), "Bu sicilde kullanıcı zaten var.");
                    return View(model);
                }

                User user = _databaseContext.Users.Find(id);

                _mapper.Map(model, user);
                _databaseContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View();
        }
        [Authorize(Roles = "admin")]
        public IActionResult Delete(Guid id, EditUserModel model)
        {

            User user = _databaseContext.Users.Find(id);

            if (user!=null)
            {
                _databaseContext.Users.Remove(user);
                _databaseContext.SaveChanges();

            }

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "admin")]
        public IActionResult ResetPassword(Guid id)
        {
            User user = _databaseContext.Users.Find(id);

            if (user != null)
            {

                string password = "Pl123456++";

                string hashedPassword = DoMD5HashedString(password);

                user.Password = hashedPassword;
                _databaseContext.SaveChanges();
                ViewData["result"] = "PasswordReseted";

            }
      
            return RedirectToAction("Index","Home");
        }
        private string DoMD5HashedString(string s)
        {
            string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
            string salted = s + md5Salt;
            string hashed = salted.MD5();
            return hashed;
        }

    }
}
