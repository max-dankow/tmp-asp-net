using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Razor.Parser;

namespace DemoWebApplication.ViewModels
{
    public struct Coordinate
    {
        public decimal X;
        public decimal Y;

        public override string ToString()
        {
            return $"{X}, {Y}";
        }
    }

    public class DemoModel
    {
        [BindNever]
        public CultureInfo Lang { get; set; }

        [BindNever]
        public string Message { get; set; }

        [Display(Name = "Number")]
        [Required(ErrorMessage = "Number is not given")]
        [Range(typeof(int), "0", "100", ErrorMessage = "err")]
        public int Number { get; set; }
    
        [BindNever]
        public decimal DecimalValue { get; set; }

        [Display(Name = "First")]
        [Required(ErrorMessage = "CoordRequired")]
        [FromBody]
        [FromQuery] // For example: /Home/Coords?lang=ru&First=10,%2010&Second=0,%200
        public Coordinate First { get; set; }

        [Display(Name = "Second")]
        [Required(ErrorMessage = "CoordRequired")]
        [FromBody]
        [FromQuery]
        public Coordinate Second { get; set; }

        public double Length()
        {
            var deltaX = (double) (First.X - Second.X);
            var deltaY = (double) (First.Y - Second.Y);
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
    }
}
