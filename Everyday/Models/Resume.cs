using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Everyday.Models
{
    public class Resume
    {

       
            [Column("ResumeId")]
            [Key]
            public int ResumeId { get; set; }


            [Column("UserId")] 
            public int UserId { get; set; }

            
            [Required(ErrorMessage ="ФИО не указано")]
            [StringLength(100, MinimumLength = 10, ErrorMessage = "Недопустимое ФИО")]
            [Display(Name = "ФИО")]
            [Column("UserName")]
            public string UserName { get; set; }
            
            [Range(16, 100, ErrorMessage = "Недопустимый возраст")]
            [Display(Name = "возраст")]
            [Required(ErrorMessage = "возраст не указан")]
            [Column("UserAge")]
            public int UserAge { get; set; }


            [RegularExpression (@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
            [StringLength(100, MinimumLength = 5, ErrorMessage = "Некорректный адрес")]
            [Required(ErrorMessage = "Почта не указана")]
            [Display(Name = "Email")]
            [Column("UserEmail")]
            public string UserEmail { get; set; }

           [Required(ErrorMessage ="мобильный не указан")]
           [Display(Name = "мобильный")]
           [Column("MobileNumber")]
            public string MobileNumber { get; set; }

            
            [StringLength(10000, MinimumLength = 50, ErrorMessage = "Длина строки должна быть от 50 до 10000 символов")]
            [Required(ErrorMessage = "Вы обязательно должнв рассказать что-то  о себе")]
            [Display(Name = "О себе")]
            [Column("About")]
            public string About { get; set; }

            
            
            
            [Display(Name = "Фото")]
            //[Required(ErrorMessage = "Фотография обязательна в резюме")]
            [Column("Photo")]
            public byte[] Photo { get; set; }
            
            
            
            
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
            [Display(Name = "Название резюме")]
            [Column("ResumeName")]
            [Required(ErrorMessage = "название не указано")]
            public string ResumeName { get; set; }
            

            [StringLength(100, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 100 символов")]
            [Required(ErrorMessage = "город не указан")]
            [Display(Name = "Город")]
            [Column("City")]
            public string City { get; set; }
            
            [Column("Vision")]
            public int? Vision{ get; set; }

             //public User User { get; set; }//навигационным свойством - при получении данных об игроке оно будет автоматически получать данные из БД.
     
    }
}