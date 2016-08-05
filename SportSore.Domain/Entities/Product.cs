using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SportSore.Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        public int ProductID { get; set; }
        
        [Required(ErrorMessage= "Пожалуйста, введите наименование отовара")]
        [Display(Name="Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите описание")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        
        [Required(ErrorMessage = "Пожалуйста, укажите категорию")]
        [Display(Name = "Категория")]
        public string Category { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage="Пожалуйста, введите положительное значение для цены")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }
    }
}
