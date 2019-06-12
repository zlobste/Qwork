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
    public class ResumeController : Controller
    {
        
        ApplicationContext db = new ApplicationContext();
        
        
        
        
        //Отображение формы создания резюме
        [HttpGet]
        [Authorize]
        public ActionResult CreateResume(int? id)
        {
            User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);


            ViewBag.usage = GetAge(us.UserAge);
            ViewBag.usname = us.UserName;
            ViewBag.usemail = us.UserEmail;
            ViewBag.usmobile = us.UserMobileNumber;
            ViewBag.id = id;
            return View();
            
        }
        //Высчитывание возраста юзера по дате рождения
        public static int GetAge(DateTime birthDate)
        {
            var now = DateTime.Today;
            return now.Year - birthDate.Year - 1 + 
                   ((now.Month > birthDate.Month || now.Month == birthDate.Month && now.Day >= birthDate.Day) ? 1 : 0);
        }
        
        
        
        
        
        //Создание резюме
        [Authorize]
        [HttpPost]
        public ActionResult CreateResume(Resume resume, int id , HttpPostedFileBase PhotoImage)
        {
            if (ModelState.IsValid)
            {
                if (PhotoImage == null)
                {
                    return CreateResume(id);
                }


                byte[] imageData;
                using (var binaryReader = new BinaryReader(PhotoImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(PhotoImage.ContentLength);
                }

                resume.Photo = imageData;
                resume.Vision = 0;
                resume.UserId = id;
                db.resume.Add(resume);
                db.SaveChanges();
                // перенаправляем на главную страницу
                return RedirectToAction("MyResumes");
            }
            
           
            return CreateResume(id);
        }
        
        
        
        //Вывод резюме пользователя
        [Authorize]
        public ActionResult MyResumes()
        {
            db.Database.EnsureCreated();
            User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);
            string f = User.Identity.Name;
            ViewBag.el = f;
            ViewBag.id = us.UserId;
            List<Resume> ff= new List<Resume>(db.resume.Where(f2 => f2.UserId == us.UserId));
            ViewBag.res = ff;
            return View(ff);
        }
        
        
        
        
        //Удаление резюме пользователя
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
        
        
        
        
        
        //Отображение формы изменения резюме
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
        
        
        
        
        //Изменение резюме
        [Authorize]
        [HttpPost]
        public ActionResult EditResume(Resume rso, HttpPostedFileBase PhotoImage)
        {
            if (ModelState.IsValid)
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

            return View(rso);
        }
        
        
        
        //Добавление резюме на вакансию
        [Authorize]
        [HttpPost]
        public ActionResult AddResume(int? idres, int? idvac)
            
        {

            if (idres == null)
            {
                return RedirectToAction("MyResumes");
            }
            else
            {
                Vacancy vac = db.vacancy.Find(idvac);
                if (vac.Resume == null)
                {
                    vac.Resume = "" + idres;

                }
                else if (vac.Resume == "")
                {
                    vac.Resume += idres;
                }
                else {
                    vac.Resume += (" " + idres);
                }
                
                
                db.Entry(vac).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index" , "Home");
            }
        }
        
        
        
        
        
        //Поиск по резюме
        public ActionResult SearchByResume(string searchString, string searchCity)
        {
            if (User.Identity.IsAuthenticated)
            {
                User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);
                ViewBag.m = us.ResumeMarks;
            }

            if (searchString == null)
            {
                searchString = "";
            }
            if (searchCity == null)
            {
                searchCity = "";
            }
           
            ViewBag.s = searchString;
            ViewBag.c = searchCity;
            var resumes = db.resume;
            return View(resumes.ToList());
        }
        
        
        
        
        
        
        //Изменения видимости резюме
        [Authorize]
        public ActionResult EditVision(int? id)
        {
            Resume rso = db.resume.Find(id);

            if (rso.Vision == 1)
            {
                rso.Vision = 0;
            }
            else if(rso.Vision == 0)
            {
                rso.Vision = 1;
            }

            db.Entry(rso).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("MyResumes");
    
        }
        
        
        
        
        
        //Добавление резюме в закладки
        [Authorize]
        public ActionResult AddResumeMark(int? id, string searchString, string searchCity)

        {

            if (id == null)
            {
                return RedirectToAction("SearchByResume");
            }
            else
            {
                User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);
                
                if (us.ResumeMarks == null)
                {
                    us.ResumeMarks = "" + id;

                }
                else if (us.ResumeMarks == "")
                {
                    us.ResumeMarks += id;
                }
                else
                {
                    us.ResumeMarks += (" " + id);
                }


                db.Entry(us).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SearchByResume",new {  searchString = searchString, searchCity=searchCity});
             

            }
        }

        
        
        
        
        //Вывод закладок резюме
        [Authorize]
        public ActionResult ResumeMarks()
        {
            User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);
               
                
            ViewBag.m = us.ResumeMarks; 
                

            List<Resume> res = new List<Resume>();
            var resumes = db.resume.ToList();
                

                
            foreach (var t in us.ResumeMarks.Split(' '))
            {
                foreach (var m in resumes)
                {
                    if (m.ResumeId == Convert.ToInt32(t))
                    {
                        res.Add(m);
                        break;

                    }

                }

            }

            return View(res);
     
        }

        
        
        
        
        
        //Удаление резюме из зщакладок
        [Authorize]
        public ActionResult DeleteResumeMark(int? id , int st , string searchString, string searchCity)
        {
            User us = db.user.FirstOrDefault(topic => topic.UserEmail == User.Identity.Name);

            string[] s = us.ResumeMarks.Split(' ');
            delByIndex(ref s, s.IndexOf(Convert.ToString(id)));        
            us.ResumeMarks= string.Join(" ", s);
            db.SaveChanges();
            if (us.ResumeMarks == "" || us.ResumeMarks == " " || us.ResumeMarks == null)
            {
                return RedirectToAction("Cabinet" , "Home");
            }

           
            if (st == 1)
            {
                return RedirectToAction("SearchByResume",
                    new {searchString = searchString, searchCity = searchCity});
            }

            return RedirectToAction("ResumeMarks");
        }
        
        
        
        
        
        //Удаление по индексу из массива
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
        
        
        
        
        
        //Удаление резюме из вакансии
        [Authorize]
        [HttpPost]
        public ActionResult DeleteThisResume(int? id , int? idvac)
        {
            Vacancy vacancy = db.vacancy.Find(idvac);

            string[] s = vacancy.Resume.Split(' ');
            delByIndex(ref s, s.IndexOf(Convert.ToString(id)));        
            vacancy.Resume = string.Join(" ", s);
            db.SaveChanges();
            
            return RedirectToAction("MyCompanies", "Company");
        }
    }
}