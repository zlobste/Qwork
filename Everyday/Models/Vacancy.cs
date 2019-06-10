
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Everyday.Models
{

    [Table("vacancy")]
    public class Vacancy
    {
        [Column("VacancyId")]
        [Key]
        public int VacancyId { get; set; }
        
        [Required(ErrorMessage = "Вы не указали компанию")]
        [Column("CompanyId")]
        public int CompanyId { get; set; }

        
        

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        [Required(ErrorMessage = "Вы не указали  низвани е вакансии")]
        [Display(Name = "Название вакансии")]
        [Column("VacancyName")]
        public string VacancyName { get; set; }
        
        
        
        

        [Column("Salary")]
        [Display(Name = "Зарплата")]
        public double? Salary { get; set; }
        
        
        
        
        
        

        
        

        [StringLength(10000, MinimumLength = 50, ErrorMessage = "Длина строки должна быть от 50 до 10000 символов")]
        [Required(ErrorMessage = "Вы обязательно должны рассказать что-то  о  вакансии")]
        [Display(Name = "О вакансии")]
        [Column("About")]
        public string About { get; set; }

       
       

      
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 100 символов")]
        [Required(ErrorMessage = "город не указан")]
        [Display(Name = "Город")]
        [Column("City")]
        public string City { get; set; }
        
        
        [Column("Resume")]
        public string Resume { get; set; }

        public  Company Company { get; set; } //навигационным свойством - при получении данных об игроке оно будет автоматически получать данные из БД.

    }
}