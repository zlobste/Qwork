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
using Microsoft.EntityFrameworkCore.Internal;

namespace Everyday.Controllers
{
    public class VacancyController : Controller
    {
        ApplicationContext db = new ApplicationContext();
        
        
        
        //Отображение создания вакансии
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);

            // Формируем список компаний для передачи в представление
            SelectList companies = new SelectList(db.company.Where(f => f.UserId == us.UserId), "CompanyId", "CompanyName");
            ViewBag.company = companies;
            return View();
        }
        
        
        
        //Создание вакансии
        [Authorize]
        [HttpPost]
        public ActionResult Create(Vacancy vacancy)
        {
            if (ModelState.IsValid)
            {
                //Добавляем вакансию в таблицу
                db.vacancy.Add(vacancy);
                db.SaveChanges();
                // перенаправляем на главную страницу
                return RedirectToAction("MyCompanies" , "Company");
            }
            User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);
            SelectList companies = new SelectList(db.company.Where(f => f.UserId == us.UserId), "CompanyId", "CompanyName", vacancy.CompanyId);
           
            ViewBag.company = companies;
            return View(vacancy);

        }
        
        
        //Отображение формы изменения вакансии
        [Authorize]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            // Находим в бд вакансию
            Vacancy vacancy = db.vacancy.Find(id);
            if (vacancy != null)
            {
                // Создаем список компаний для передачи в представление
                User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);
                SelectList companies = new SelectList(db.company.Where(f => f.UserId == us.UserId), "CompanyId", "CompanyName", vacancy.CompanyId);

                ViewBag.company = companies;
                return View(vacancy);
            }
            return RedirectToAction("MyCompanies" , "Company");
        }
        
        
        //Изменение вакансии
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Vacancy vacancy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vacancy).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("MyCompanies", "Company");
            }
            User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);
            SelectList companies = new SelectList(db.company.Where(f => f.UserId == us.UserId), "CompanyId", "CompanyName", vacancy.CompanyId);
           
            ViewBag.company = companies;
            return View(vacancy);
        }
        
        
        
        
        //Вывод вакансий определённой компании
        [Authorize]
        public ActionResult Details(int? id)
        {
            ViewBag.searchId = id;
            db.Database.EnsureCreated();
            var vacancies = db.vacancy.Include(v => v.Company);
            return View(vacancies.ToList());
        }
        
        
        
        //Удаление вакансии
        [Authorize]
        public ActionResult DeleteVacancy(int? id)
        {
            Vacancy vac = db.vacancy.Find(id);
            if (vac != null)
            {
                db.vacancy.Remove(vac);
                db.SaveChanges();
            }
            return RedirectToAction("MyCompanies", "Company");
        }
        
        
        
        //Просмотр резюме отосланых на эту вакансию
        [Authorize]
        public ActionResult VacancyDetails(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
           
            // Находим в бд вакансию
            Vacancy vacancy = db.vacancy.Find(id);
            if (vacancy != null)
            {

                if (vacancy.Resume==null|| vacancy.Resume.Equals("")|| vacancy.Resume.Equals(" "))
                {

                    return RedirectToAction("MyCompanies", "Company");
                }

                Resume r;
                int k;
                List<Resume> res = new List<Resume>();
                foreach (var t in vacancy.Resume.Split(' '))
                {
                    k = Convert.ToInt32(t);
                    
                    r = db.resume.FirstOrDefault(topic => topic.ResumeId == k);
                    res.Add(r);
                   
                }

                ViewBag.r = res;
                ViewBag.v = vacancy;
                ViewBag.er = id;
                return View();
               
            }
            return RedirectToAction("MyCompanies", "Company");
        }
        
        
        
        //Добавление вакансии в  закладки
        
        [Authorize]
        public ActionResult AddVacancyMark(int? id, string searchString, string searchCity, string salary)

        {

            if (id == null)
            {
                return RedirectToAction("Index" , "Home");
            }
            else
            {
                User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);
                if (us.VacancyMarks == null)
                {
                    us.VacancyMarks = "" + id;

                }
                else if (us.VacancyMarks == "")
                {
                    us.VacancyMarks += id;
                }
                else
                {
                    us.VacancyMarks += (" " + id);
                }


                db.Entry(us).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home",new {  searchString = searchString, searchCity=searchCity, salary=salary });
             

            }
        }
        
        
        
        //Просмотр закадок вакансий
        [Authorize]
        public ActionResult VacancyMarks()
        {
            User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);
            SelectList rezimes =new SelectList(db.resume.Where(f => f.UserId == us.UserId), "ResumeId", "UserEmail");
            ViewBag.rezimes = rezimes;
            ViewBag.sel = db.resume.Where(f => f.UserId == us.UserId);

            List<Vacancy> vac = new List<Vacancy>();
            var vacancies = db.vacancy.Include(v => v.Company);
            vacancies.ToList();

                
            foreach (var t in us.VacancyMarks.Split(' '))
            {
                foreach (var m in vacancies)
                {
                    if (m.VacancyId == Convert.ToInt32(t))
                    {
                        vac.Add(m);
                        break;

                    }

                }

            }

            return View(vac);
        }
        
        
        
        
        
        //Удаление вакансии из закадок
        [Authorize]
        public ActionResult DeleteVacancyMark(int? id , int st , string searchString, string searchCity, string salary)
        {
            User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);

            string[] s = us.VacancyMarks.Split(' ');
            delByIndex(ref s, s.IndexOf(Convert.ToString(id)));        
            us.VacancyMarks= string.Join(" ", s);
            db.SaveChanges();
            if (us.VacancyMarks == "" || us.VacancyMarks == " " || us.VacancyMarks == null)
            {
                return RedirectToAction("Cabinet" , "Home");
            }

           
            if (st == 1)
            {
                return RedirectToAction("Index", "Home" ,new {searchString = searchString, searchCity = searchCity, salary = salary});
            }

            return RedirectToAction("VacancyMarks");
        }


         //Удаение по игдексу из массива
        public static void delByIndex(ref string[] data, int delIndex)
        {
            string[] newData = new string[data.Length - 1];
            for (int i = 0; i < delIndex; i++)
            {
                newData[i] = data[i];
            }
            for (int i = delIndex; i < newData.Length; i++)
            {
                newData[i] = data[i + 1];
            }
            data = newData;
        }
        
       
    }
}