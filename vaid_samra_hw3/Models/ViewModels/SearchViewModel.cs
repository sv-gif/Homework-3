using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace vaid_samra_hw3.Models
{
    public enum PriceType { GreaterThan, LessThan}
    public class SearchViewModel
    {
        [Key]

        [Display(Name = "Search by Title: ")]
        public String SearchName { get; set; }

        [Display(Name = "Search by Description")]
        public String SearchDescription { get; set; }

        [Display(Name = "Search by Format:")]
        public Format? SearchFormat { get; set; }

        [Display(Name = "Search by Genre:")]
        public Int32 SearchGenreID { get; set; }


        [Display(Name = "Search by Price: ")]
        public Decimal? SearchPrice { get; set; }

        [Display(Name = "Type of Search: ")]
        public /*PriceType*/ PriceType? SearchPriceType { get; set; } 

        [Display(Name = "Search by Published Date: ")]
        [DataType(DataType.Date)]
        public DateTime? SearchPublished { get; set; }
    }
}
