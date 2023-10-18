using AutoMapper;
using CocukSubeProject.Entities;
using CocukSubeProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Claims;
using System.Xml.Linq;
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
        //[Authorize]
        //public IActionResult Index()
        //{
        //    string district = User.FindFirstValue(ClaimTypes.Locality);

        //    if (User.IsInRole("admin"))
        //    {
        //        var suspects = _databaseContext.Suspects
        //        .Select(x => _mapper.Map<SuspectModel>(x)).ToList();

        //        return View(suspects);

        //    }
        //    else
        //    {
        //        var suspects = _databaseContext.Suspects.Where(i => i.District == district)
        //    .Select(x => _mapper.Map<SuspectModel>(x)).ToList();

        //        return View(suspects);
        //    }
        //}
        [Authorize(Roles = "admin")]
        public IActionResult Mukerrer()
        {

            var suspects = _databaseContext.Suspects.GroupBy(p => new { p.Tc, p.DateOfBirth/*p.Name, p.LastName, p.Nationality, *//*, */ },
                p => p, (key, g) => new MukerrerModel
                {
                    Tc = key.Tc,
                    Ages = AgeStatus(key.DateOfBirth), /*,Name = key.Name, LastName = key.LastName,*/
                    //* Nationality = key.Nationality, DateOfBirth = key.DateOfBirth,
                    Counting = g.Count()
                }).Where(x => x.Counting > 1).ToList();


            return View(suspects);

        }

        //public IActionResult SuspectListPartial(int page=1)
        //{
        //    var suspects = _databaseContext.Suspects
        //       .Select(x => _mapper.Map<SuspectModel>(x)).ToPagedList(page,10);

        //    return PartialView("_SuspectListPartial", suspects);
        //}
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult SuspectListPartial(int page = 1)
        {
            string district = User.FindFirstValue(ClaimTypes.Locality);

            if (User.IsInRole("admin"))
            {
                var suspects = _databaseContext.Suspects
                .Select(x => _mapper.Map<SuspectModel>(x)).ToPagedList(page, 10);

                return PartialView("_SuspectListPartial", suspects);

            }
            else
            {
                var suspects = _databaseContext.Suspects.Where(i => i.District == district)
            .Select(x => _mapper.Map<SuspectModel>(x)).ToPagedList(page, 10);

                return PartialView("_SuspectListPartial", suspects);
            }

        }
   
        [Authorize]
        public IActionResult AddNewSuspectPartial()
        {


            return PartialView("_AddNewSuspectPartial", new SuspectModel());
        }

        [HttpPost]
        [Authorize]
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

        [Authorize]
        public IActionResult EditSuspectPartial(int id)
        {
            Suspect suspect = _databaseContext.Suspects.Find(id);
            SuspectModel model = _mapper.Map<SuspectModel>(suspect);


            return PartialView("_EditSuspectPartial", model);
        }
        [HttpPost]
        [Authorize]
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
        [Authorize]
        public IActionResult DeleteSuspect(int id)
        {
            Suspect suspect = _databaseContext.Suspects.Find(id);
            if (suspect != null)
            {
                _databaseContext.Suspects.Remove(suspect);
                _databaseContext.SaveChanges();
            }

            return RedirectToAction("Index", "Suspect");
        }

        [Authorize(Roles = "admin")]
        public IActionResult Filter(string catchDateStart = null, string catchDateEnd = null, string tc = null, string name = null, string lastName = null, string nationality = null, string catchAdress = null, string district = null, string ages = null, string gender = null,string allSuspects = null)
        {
            IQueryable<Suspect> suspects = _databaseContext.Suspects;

            if (!string.IsNullOrEmpty(allSuspects))
            {
               suspects = _databaseContext.Suspects;
            }

            if (!string.IsNullOrEmpty(catchDateStart))
            {
                DateTime start = DateTime.Parse(catchDateStart);
                suspects = suspects.Where(s => s.CatchDate >= start);
            }

            if (!string.IsNullOrEmpty(catchDateEnd))
            {
                DateTime end = DateTime.Parse(catchDateEnd);
                suspects = suspects.Where(s => s.CatchDate <= end);
            }


            if (!string.IsNullOrEmpty(name))
            {
                suspects = suspects.Where(s => s.Name.ToLower().Trim().Contains(name.ToLower().Trim()));
            }

            if (!string.IsNullOrEmpty(tc))
            {
                suspects = suspects.Where(s => s.Tc.ToLower().Trim().Contains(tc.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                suspects = suspects.Where(s => s.LastName.ToLower().Trim().Contains(lastName.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(nationality))
            {
                suspects = suspects.Where(s => s.Nationality.ToLower().Trim().Contains(nationality.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(catchAdress))
            {
                suspects = suspects.Where(s => s.CatchAdress.ToLower().Trim().Contains(catchAdress.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(district))
            {
                suspects = suspects.Where(s => s.District.ToLower().Trim().Contains(district.ToLower().Trim()));
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
                suspects = suspects.Where(s => s.Gender.ToLower().Trim().Contains(gender.ToLower().Trim()));
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


            if (!string.IsNullOrEmpty(catchDateStart) || !string.IsNullOrEmpty(catchDateEnd) || !string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(tc) || !string.IsNullOrEmpty(lastName) || !string.IsNullOrEmpty(nationality) || !string.IsNullOrEmpty(catchAdress) || !string.IsNullOrEmpty(district) || !string.IsNullOrEmpty(ages) || !string.IsNullOrEmpty(gender) || !string.IsNullOrEmpty(allSuspects))
            {
                // Eğer herhangi bir filtre parametresi doluysa veritabanından verileri getir
                var viewModel = new SuspectViewModel
                {
                    Suspects = suspects.ToList(),
                    SuspectCount = suspects.Count()
                };

                return View(viewModel);
            }
            else
            {
                // Eğer hiçbir filtre parametresi girilmediyse, boş bir sonuç göster
                return View(new SuspectViewModel { SuspectCount =  suspects.Count(), Suspects = new List<Suspect> {  } });
            }
        }

        public static string AgeStatus(DateTime dob)
        {
            var today = DateTime.Today;
            var age = (today - dob).TotalDays / 365;
            return age >= 18 ? "18 Yaşından büyük" : "18 Yaşından küçük";
        }


        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            // if we have an empty search then just order the results by Id ascending
            var orderCriteria = "Id";
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }

            var result = _databaseContext.Suspects.AsQueryable();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper()));
            }

            result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = await result.CountAsync();
            var totalResultsCount = await _databaseContext.Suspects.CountAsync();



            var Data = await result
                     .Skip(dtParameters.Start)
                     .Take(dtParameters.Length)
                     .ToListAsync();
            var ddd = _mapper.Map<List<SuspectModel>>(Data);
            return Json(new DtResult<SuspectModel>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = ddd
            });
        }
    }
}
