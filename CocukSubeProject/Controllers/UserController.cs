using AutoMapper;
using CocukSubeProject.Entities;
using CocukSubeProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace CocukSubeProject.Controllers
{
    public class UserController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public UserController(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            List<UserModel> users = _databaseContext.Users.ToList()
                .Select(x => _mapper.Map<UserModel>(x)).ToList();

            return View(users);
        }

        public IActionResult Edit(Guid id)
        {
            User user = _databaseContext.Users.Find(id);
            EditUserModel model = _mapper.Map<EditUserModel>(user);

            return View(model);
        }
        [HttpPost]
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


    }
}
