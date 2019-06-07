using Everyday.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Everyday.Models
{
    public class Company
    {


        [Column("CompanyId")]
        [Key]
        public int CompanyId { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }
        
        
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        [Required(ErrorMessage = "Вы не ввели название компании")]
        [Column("CompanyName")]
        public string CompanyName { get; set; }


        [RegularExpression (@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [Required(ErrorMessage = "Вы не ввели почту")]
        [Column("CompanyEmail")]
        public string CompanyEmail { get; set; }

        
        [StringLength(10000, MinimumLength = 50, ErrorMessage = "Длина строки должна быть от 50 до 10000 символов")]
        [Required(ErrorMessage = "Вы обязательно должны рассказать что-то  о компании")]
        [Column("About")]
        public string About { get; set; }


        public  User User { get; set; }//навигационным свойством - при получении данных об игроке оно будет автоматически получать данные из БД.

        public  List<Vacancy> Vacancies { get; set; }

    }
}