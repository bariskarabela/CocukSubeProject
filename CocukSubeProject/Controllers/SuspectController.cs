﻿using AutoMapper;
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
        [Authorize]
        public IActionResult Index()
        {
            string district = User.FindFirstValue(ClaimTypes.Locality);

            if (User.IsInRole("admin"))
            {
                var suspects = _databaseContext.Suspects
                .Select(x => _mapper.Map<SuspectModel>(x)).ToList();

                return View(suspects);

            }
            else
            {
                var suspects = _databaseContext.Suspects.Where(i => i.District == district)
            .Select(x => _mapper.Map<SuspectModel>(x)).ToList();

                return View(suspects);
            }
        }
        [Authorize(Roles ="admin")]
        public IActionResult Mukerrer()
        {

            var suspects = _databaseContext.Suspects.GroupBy(p => new { p.Tc/*, p.Name, p.LastName, p.Nationality, p.DateOfBirth*/ },
                p => p, (key, g) => new MukerrerModel
                { Tc = key.Tc, /*Name = key.Name, LastName = key.LastName, Nationality = key.Nationality, DateOfBirth = key.DateOfBirth,*/ Counting = g.Count() }).ToList();
          

            return View(suspects);


        }

        //public IActionResult SuspectListPartial(int page=1)
        //{
        //    var suspects = _databaseContext.Suspects
        //       .Select(x => _mapper.Map<SuspectModel>(x)).ToPagedList(page,10);

        //    return PartialView("_SuspectListPartial", suspects);
        //}
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
        [Authorize(Roles = "admin")]
        public IActionResult Chart()
        {
            return View();
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

            return RedirectToAction("Index","Suspect");
        }

        [Authorize(Roles = "admin")]
        public IActionResult Filter(string catchDateStart=null, string catchDateEnd = null, string tc = null, string name = null, string lastName = null, string nationality = null, string catchAdress = null, string district = null, string ages = null, string gender = null)
        {
            IQueryable<Suspect> suspects = _databaseContext.Suspects;


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
