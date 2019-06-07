using Everyday.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Everyday.Controllers
{
    public class AccountController : Controller
    {
        
        //---------------------------------------------------------------------------------
        //Вывод страницы входа
        public ActionResult Login()
        {
            return View();
        }

        //...................................................................................
        //Пост запрос аунтификации
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {

            ApplicationContext db = new ApplicationContext();
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                User user = null;
                user = db.user.FirstOrDefault(u => u.UserEmail == model.Email && u.Password == model.Password);

                
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, true);
                    return RedirectToAction("Cabinet", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
                ViewBag.userid = user.UserId;
            }

            return View(model);
        }

        //---------------------------------------------------------------------------------
        //Отображение страницы регистрации
        public ActionResult Register()
        {
            return View();
        }
        //...................................................................................
        //Пост запрос регистрации
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (ApplicationContext db = new ApplicationContext())
                {
                    user = db.user.FirstOrDefault(u => u.UserEmail == model.UserEmail);
                }
                if (user == null)
                {
                    // создаем нового пользователя
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        db.user.Add(new User { UserEmail = model.UserEmail, Password = model.Password, UserAge = model.UserAge, UserName = model.UserName, UserMobileNumber = model.UserMobileNumber });
                        db.SaveChanges();

                        user = db.user.Where(u => u.UserEmail == model.UserEmail && u.Password == model.Password).FirstOrDefault();
                    }
                    // если пользователь удачно добавлен в бд
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.UserEmail, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }

            return View(model);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }





}