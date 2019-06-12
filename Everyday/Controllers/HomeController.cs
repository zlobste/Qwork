using Everyday.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;

namespace Everyday.Controllers
{
    public class HomeController : Controller
    {

        ApplicationContext db = new ApplicationContext();

       
        //Главная страница с поиском по вакансиям
        public ActionResult Index(string searchString, string searchCity, string salary)
        {
            if (User.Identity.IsAuthenticated)
            {

                User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);

                SelectList rezimes = new SelectList(db.resume.Where(f => f.UserId == us.UserId), "ResumeId", "UserEmail");
                ViewBag.rezimes = rezimes;
                ViewBag.sel = db.resume.Where(f => f.UserId == us.UserId);
                string mark = us.VacancyMarks;
                if (mark == null) { mark = ""; }
                ViewBag.m = mark;

            }

            
            if (searchString == null)
            {
                searchString = "";
            }
            if (searchCity == null)
            {
                searchCity = "";
            }
           
            ViewBag.salary = salary;
            ViewBag.s = searchString;
            ViewBag.c = searchCity;
            db.Database.EnsureCreated();
            var vacancies = db.vacancy.Include(v => v.Company);
            return View(vacancies.ToList());
        }
        
        
        //---------------------------------------------------------------------------------
        //Кабинет пользователя с навигацией по приложению
        [Authorize]
        public ActionResult Cabinet()
        {
            User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);
            ViewBag.usn = us.UserName;
            ViewBag.us = us.VacancyMarks;
            ViewBag.rs = us.ResumeMarks;
            return View();
        }
   
        
    


        
       
        
    
        
        
        
        
        
        
        

        

        
    

        
        
        
        
        
        
        

    }
}