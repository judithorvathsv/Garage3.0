using Garage3.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Validations
{
    public class SSNRegCheckAttribute : RegularExpressionAttribute
    {
        public SSNRegCheckAttribute() : base("[0-9]{2}(?:0[1-9]|1[012])(?:0[1-9]|[12][0-9]|3[01])-[0-9]{4}")
        {
            ErrorMessageResourceType = typeof(ValidationMessages);
            ErrorMessageResourceName = "Invalid";
        }
    }
}