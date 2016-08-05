using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportSore.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage="Введите имя")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Введите первую строку адреса")]
        [Display(Name="Строка 1")]
        public string Line1 { get; set; }

        [Display(Name = "Строка 2")]
        public string Line2 { get; set; }

        [Display(Name = "Строка 3")]
        public string Line3 { get; set; }


        [Required(ErrorMessage = "Введите название города")]
        [Display(Name = "Город")]
        public string City { get; set; }

        [Required(ErrorMessage = "Введите название области")]
        [Display(Name = "Область / Штат")]
        public string State { get; set; }

        [Display(Name = "Почтовый индекс / Zip-код")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "Введите название страны")]
        [Display(Name = "Страна")]
        public string Country { get; set; }

        //Подарочная упаковка
        public bool GiftWrap { get; set; }

    }
}
