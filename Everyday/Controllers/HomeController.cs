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

namespace Everyday.Controllers
{
    public class HomeController : Controller
    {

        ApplicationContext db = new ApplicationContext();



        //---------------------------------------------------------------------------------
        //Список(вывод) вакансий
        public ActionResult MyVacancies()
        {
            db.Database.EnsureCreated();
            var vacancies = db.vacancy.Include(v => v.Company);
            return View(vacancies.ToList());
        }
        //---------------------------------------------------------------------------------
        //Cоздание вакансии
        //Get метод
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
        //..................................................................................
        //Post метод
        [Authorize]
        [HttpPost]
        public ActionResult Create(Vacancy vacancy)
        {
            //Добавляем вакансию в таблицу
            db.vacancy.Add(vacancy);
            db.SaveChanges();
            // перенаправляем на главную страницу
            return RedirectToAction("MyCompanies");
        }
        //---------------------------------------------------------------------------------
        //Изменение вакансии
        //Get метод
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
                SelectList companies = new SelectList(db.company, "CompanyId", "CompanyName", vacancy.CompanyId);
                ViewBag.company = companies;
                return View(vacancy);
            }
            return RedirectToAction("MyCompanies");
        }
        //..................................................................................
        //Post метод
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Vacancy vacancy)
        {
            db.Entry(vacancy).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("MyCompanies");
        }
        //---------------------------------------------------------------------------------
        //Cоздание компании
        //Get метод
        [Authorize]
        [HttpGet]
        public ActionResult CreateCompany(int? id)
        {
            ViewBag.id = id;

            return View();
        }
        //..................................................................................
        //Post метод
        [Authorize]
        [HttpPost]
        public ActionResult CreateCompany(Company company, int id)
        {
            //Добавляем компани в таблицу
            company.UserId = id;
            db.company.Add(company);
            db.SaveChanges();
            // перенаправляем на главную страницу
            return RedirectToAction("MyCompanies");
        }
        //---------------------------------------------------------------------------------
        //Изменение компании
        //Get метод
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
        //..................................................................................
        //Post метод
        [Authorize]
        [HttpPost]
        public ActionResult EditCompany(Company companyo)
        {
            db.Entry(companyo).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("MyCompanies");
        }
        //---------------------------------------------------------------------------------
        //Список(вывод) вакансий
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
        //---------------------------------------------------------------------------------
        //Удаление компании
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
        //---------------------------------------------------------------------------------
        //Показать вакансии конкретной компании
        [Authorize]
        public ActionResult Details(int? id)
        {
            ViewBag.searchId = id;
            db.Database.EnsureCreated();
            var vacancies = db.vacancy.Include(v => v.Company);
            return View(vacancies.ToList());
        }
        //---------------------------------------------------------------------------------
        //Удаление vacancy
        [Authorize]
        public ActionResult DeleteVacancy(int? id)
        {
            Vacancy vac = db.vacancy.Find(id);
            if (vac != null)
            {
                db.vacancy.Remove(vac);
                db.SaveChanges();
            }
            return RedirectToAction("MyCompanies");
        }
        //---------------------------------------------------------------------------------


        public ActionResult Index(string searchString, string searchCity, string salary)
        {

            User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);

           
            SelectList rezimes = new SelectList(db.resume.Where(f => f.UserId == us.UserId), "ResumeId", "UserEmail");
            ViewBag.rezimes = rezimes;


            ViewBag.sel = db.resume.Where(f => f.UserId == us.UserId);

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



        [HttpPost]
        public string Index(FormCollection fc, string searchString)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }


        [Authorize]
        public ActionResult Cabinet()
        {

            return View();
        }





        [HttpGet]
        [Authorize]
        public ActionResult CreateResume(int? id)
        {


            ViewBag.id = id;

            return View();
            
        }
        //..................................................................................
        //Post метод
        [Authorize]
        [HttpPost]
        public ActionResult CreateResume(Resume resume, int id , HttpPostedFileBase PhotoImage)
        {
            if(PhotoImage== null) { return CreateResume(id); }

            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(PhotoImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(PhotoImage.ContentLength);
                }
                resume.Photo = imageData;

            }

            //Добавляем вакансию в таблицу
            resume.UserId = id;
            db.resume.Add(resume);
            db.SaveChanges();
            // перенаправляем на главную страницу
            return RedirectToAction("MyResumes");
        }


       



        [Authorize]
        public ActionResult MyResumes()
        {
            db.Database.EnsureCreated();



            User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);

            string f = User.Identity.Name;
            ViewBag.el = f;
            ViewBag.id = us.UserId;


            db.Database.EnsureCreated();
            var resumes = db.resume.Include(v => v.User);
            return View(resumes.ToList());
        }


        [Authorize]
        public ActionResult DeleteResume(int? id)
        {
            Resume vac = db.resume.Find(id);
            if (vac != null)
            {
                db.resume.Remove(vac);
                db.SaveChanges();
            }
            return RedirectToAction("MyResumes");
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditResume(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            // Находим в бд 
            Resume reso = db.resume.Find(id);
            if (reso != null)
            {
               
                return View(reso);
            }
            return RedirectToAction("MyResumes");
        }
        //..................................................................................
        //Post метод
        [Authorize]
        [HttpPost]
        public ActionResult EditResume(Resume rso, HttpPostedFileBase PhotoImage)
        {




            if (PhotoImage != null)
            {
                if (ModelState.IsValid)
                {
                    using (var binaryReader = new BinaryReader(PhotoImage.InputStream))
                    {
                        byte[] imageData = null;
                        imageData = binaryReader.ReadBytes(PhotoImage.ContentLength);
                        rso.Photo = imageData;
                    }
                }
            } 
            

          

            db.Entry(rso).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("MyResumes");


            
        }



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

                    return RedirectToAction("MyCompanies");
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
            return RedirectToAction("MyCompanies");
        }





       
        //..................................................................................
        //Post метод
        [Authorize]
        [HttpPost]
        public ActionResult AddResume(int? idres, int? idvac)
            
        {

            if (idres == null)
            {
                return RedirectToAction("MyCompanies");
            }
            else
            {
                Vacancy vac = db.vacancy.Find(idvac);
                if (vac.Resume == null)
                {
                    vac.Resume = "" + idres;

                }
                else {
                    vac.Resume += (" " + idres);
                }
                
                
                db.Entry(vac).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult DeleteThisResume(int? id , int? idvac)
        {


            Vacancy vacancy = db.vacancy.Find(idvac);

            string[] s = vacancy.Resume.Split(' ');
            delByIndex(ref s, s.IndexOf(Convert.ToString(id)));        
            vacancy.Resume = string.Join(" ", s);
            db.SaveChanges();
            
            return RedirectToAction("MyCompanies");
        }




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