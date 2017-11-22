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

        [BindRequired]
        [Required(ErrorMessage = "Number is not given")]
        [Range(typeof(int), "1", "100", ErrorMessage = "err")]
        public int Number { get; set; }
    
        [BindNever]
        public decimal DecimalValue { get; set; }

        [Display(Name = "First")]
        public Coordinate CoordinateFirst { get; set; }

        [Display(Name = "Second")]
        public Coordinate CoordinateSecond { get; set; }
    }
}
