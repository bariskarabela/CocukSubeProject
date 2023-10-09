using AutoMapper;
using CocukSubeProject.Entities;
using CocukSubeProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using X.PagedList;
using X.PagedList;
using X.PagedList.Mvc;
using X.PagedList.Mvc.Core;



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

        public IActionResult SuspectListPartial(int page=1)
        {
            var suspects = _databaseContext.Suspects
               .Select(x => _mapper.Map<SuspectModel>(x)).ToPagedList(page,10);

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


        public IActionResult Filter(string name = null, string lastName = null, string nationality = null, string catchAdress = null, string district = null, string ages = null,/* int? minAge = null, int? maxAge = null,*/ string gender = null,string tc = null)
        {
            IQueryable<Suspect> suspects = _databaseContext.Suspects;

            if (!string.IsNullOrEmpty(name))
            {
                suspects = suspects.Where(s => s.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(tc))
            {
                suspects = suspects.Where(s => s.Tc.Contains(tc));
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                suspects = suspects.Where(s => s.LastName.Contains(lastName));
            }
            if (!string.IsNullOrEmpty(nationality))
            {
                suspects = suspects.Where(s => s.Nationality.Contains(nationality));
            }
            if (!string.IsNullOrEmpty(catchAdress))
            {
                suspects = suspects.Where(s => s.CatchAdress.Contains(catchAdress));
            }
            if (!string.IsNullOrEmpty(district))
            {
                suspects = suspects.Where(s => s.District.Contains(district));
            }

            if (!string.IsNullOrEmpty(ages))
            {

                var range = ages.Split("-", StringSplitOptions.None);
                var start = int.Parse(range[0].Trim());
                var end = int.Parse(range[1].Trim());
                DateTime minBirthDate = DateTime.Now.AddYears(-start);
                DateTime maxBirthDate = DateTime.Now.AddYears(-end);
                suspects = suspects.Where(s => s.DateOfBirth <= minBirthDate && s.DateOfBirth > maxBirthDate);

            }
            if (!string.IsNullOrEmpty(gender))
            {
                suspects = suspects.Where(s => s.Gender.Contains(gender));
            }

            //if (minAge.HasValue)
            //{
            //    DateTime minBirthDate = DateTime.Now.AddYears(-minAge.Value);
            //    suspects = suspects.Where(s => s.DateOfBirth <= minBirthDate);
            //}

            //if (maxAge.HasValue)
            //{
            //    DateTime maxBirthDate = DateTime.Now.AddYears(-maxAge.Value);
            //    suspects = suspects.Where(s => s.DateOfBirth > maxBirthDate);
            //}

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
