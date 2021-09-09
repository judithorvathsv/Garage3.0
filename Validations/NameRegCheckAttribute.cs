using Garage3.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Validations
{
    public class NameRegCheckAttribute : RegularExpressionAttribute
    {
        public NameRegCheckAttribute() : base("[-a-zA-Z]+")
        {
            ErrorMessageResourceType = typeof(ValidationMessages);
            ErrorMessageResourceName = "Invalid";
        }
    }
}
