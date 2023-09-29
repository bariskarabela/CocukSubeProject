using AutoMapper;
using CocukSubeProject.Entities;
using CocukSubeProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace CocukSubeProject.Controllers
{
    public class SuspectController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public SuspectController(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SuspectListPartial()
        {
            List<UserModel> users = _databaseContext.Users.ToList()
               .Select(x => _mapper.Map<UserModel>(x)).ToList();

            return PartialView("_SuspectListPartial",users);
        }
    }
}
