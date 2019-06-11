using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Everyday.Models
{
    public class User
    {

        [Column("UserId")]
        [Key]
        public int UserId { get; set; }

        
        [Display(Name = "ФИО")]
        [Column("UserName")]
        public string UserName { get; set; }


       
        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        [Column("UserAge")]
        public DateTime UserAge { get; set; }


        [RegularExpression (@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [Display(Name = "Email")]
        [Column("UserEmail")]

        public string UserEmail { get; set; }


        [Display(Name = "Мобильный номер")]
        [Column("UserMobileNumber")]
        public string UserMobileNumber { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [Column("Password")]
        public string Password { get; set; }

        
        
        
        public string VacancyMarks{ get; set; }
        
        
        

        public List<Resume> Resumes { get; set; }

        public List<Company> Companies { get; set; }
    }
}