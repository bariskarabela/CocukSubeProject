using AutoMapper;
using CocukSubeProject.Entities;
using CocukSubeProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            List<SuspectModel> suspects = _databaseContext.Suspects.ToList()
               .Select(x => _mapper.Map<SuspectModel>(x)).ToList();

            return PartialView("_SuspectListPartial", suspects);
        }
        public IActionResult Chart()
        {
            return View();
        }

        public IActionResult AddNewSuspectPartial()
        {


            return PartialView("_AddNewSuspectPartial", new SuspectModel());
        }
        [HttpPost]
        public IActionResult AddNewSuspect(SuspectModel model)
        {
            if (ModelState.IsValid)
            {
                Suspect suspect = _mapper.Map<Suspect>(model);

                _databaseContext.Suspects.Add(suspect);
                _databaseContext.SaveChanges();

                return PartialView("_AddNewSuspectPartial", new SuspectModel { Done = "Olay eklendi." });
            }

            return PartialView("_AddNewSuspectPartial", model);
        }


        public IActionResult EditSuspectPartial(int id)
        {
            Suspect suspect = _databaseContext.Suspects.Find(id);
            SuspectModel model = _mapper.Map<SuspectModel>(suspect);


            return PartialView("_EditSuspectPartial", model);
        }
        [HttpPost]
        public IActionResult EditSuspect(int id, SuspectModel model)
        {
            if (ModelState.IsValid)
            {
                Suspect suspect = _databaseContext.Suspects.Find(id);

                _mapper.Map(model, suspect);
                _databaseContext.SaveChanges();

                return PartialView("_EditSuspectPartial", new SuspectModel { Done = "Olay düzenlendi." });
            }



            return PartialView("_EditSuspectPartial", model);
        }

        public IActionResult DeleteSuspect(int id)
        {
            Suspect suspect = _databaseContext.Suspects.Find(id);
            if (suspect != null)
            {
                _databaseContext.Suspects.Remove(suspect);
                _databaseContext.SaveChanges();
            }

            return SuspectListPartial();
        }


        public IActionResult Filter(string name = null, string lastName = null, string nationality = null, string catchAdress = null, string district = null, bool? isUnder18 = false, string gender = null)
        {
            IQueryable<Suspect> suspects = _databaseContext.Suspects;

            if (!string.IsNullOrEmpty(name))
            {
                suspects = suspects.Where(s => s.Name == name);
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                suspects = suspects.Where(s => s.LastName == lastName);
            }
            if (!string.IsNullOrEmpty(nationality))
            {
                suspects = suspects.Where(s => s.Nationality == nationality);
            }
            if (!string.IsNullOrEmpty(catchAdress))
            {
                suspects = suspects.Where(s => s.CatchAdress == catchAdress);
            }
            if (!string.IsNullOrEmpty(district))
            {
                suspects = suspects.Where(s => s.District == district);
            }
            if (!string.IsNullOrEmpty(gender))
            {
                suspects = suspects.Where(s => s.Gender == gender);
            }

            if (!isUnder18.Value)
            {
                DateTime maxBirthDate = DateTime.Now.AddYears(-18);
                suspects = suspects.Where(s => s.DateOfBirth > maxBirthDate);
            }
            else
            {
                DateTime minBirthDate = DateTime.Now.AddYears(-18);
                suspects = suspects.Where(s => s.DateOfBirth <= minBirthDate);
            }

            // Sonuçları ViewModel ile görüntüleme.
            var viewModel = new SuspectViewModel
            {
                Suspects = suspects.ToList(),
                SuspectCount = suspects.Count()
            };

            return View(viewModel);
        }
    }
}
