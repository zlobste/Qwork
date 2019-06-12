using System.Web.Mvc;
using System.Web.Mvc;
using Everyday.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Everyday.Models;
using Microsoft.EntityFrameworkCore;

namespace Everyday.Controllers
{
    public class CompanyController : Controller
    {
        ApplicationContext db = new ApplicationContext();
        
        
        
        //Щтображение формы создания компании
        [Authorize]
        [HttpGet]
        public ActionResult CreateCompany(int? id)
        {
            ViewBag.id = id;

            return View();
        }
        
        
        //Создание компании
        [Authorize]
        [HttpPost]
        public ActionResult CreateCompany(Company company, int id)
        {
            if (ModelState.IsValid)
            {
                //Добавляем компани в таблицу
                company.UserId = id;
                db.company.Add(company);
                db.SaveChanges();
                // перенаправляем на главную страницу


                return RedirectToAction("MyCompanies");
            }

            return CreateCompany(id);

        }
        
        
        
        
        
        //Отображение страницы изменения компании
        [Authorize]
        [HttpGet]
        public ActionResult EditCompany(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            // Находим в бд 
            Company companyo = db.company.Find(id);
            if (companyo != null)
            {
                // Создаем список компаний для передачи в представление
                //SelectList companies = new SelectList(db.company, "CompanyId", "CompanyName", vacancy.CompanyId);
                // ViewBag.company = companies;
                return View(companyo);
            }
            return RedirectToAction("MyCompanies");
        }
        
        
        
        
        //Изменения в компании
        [Authorize]
        [HttpPost]
        public ActionResult EditCompany(Company companyo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companyo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyCompanies");
            }

            return View(companyo);
        }
        
        
        
        //Вывод  всех компаний юзера
        [Authorize]
        public ActionResult MyCompanies()
        {
            db.Database.EnsureCreated();



            User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);

            string f = User.Identity.Name;
            ViewBag.el = f;
            ViewBag.id = us.UserId;

            var companies = db.company.Include(v => v.User);

            return View(companies);
        }
        
        
        
        //удаление компании
        [Authorize]
        public ActionResult DeleteCompany(int? id)
        {
            Company companyo = db.company.Find(id);
            if (companyo != null)
            {
                db.company.Remove(companyo);
                db.SaveChanges();
            }

            Vacancy vac = db.vacancy.FirstOrDefault(topic => topic.CompanyId == id);
            if (vac != null)
            {
                db.vacancy.Remove(vac);
                db.SaveChanges();
            }
            return RedirectToAction("MyCompanies");
        }
        
        
       // Просмотр детальной информации о компании
        public ActionResult SeeCompany(int id)
        {
            Company companyo = db.company.Find(id);
            ViewBag.co1 = companyo.CompanyName;
            ViewBag.co2 = companyo.CompanyEmail;
            ViewBag.co3 = companyo.About;
            
            return View();
        }
        
        
        
        
    }
}