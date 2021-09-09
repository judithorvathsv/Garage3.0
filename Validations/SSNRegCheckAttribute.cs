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
        public SSNRegCheckAttribute() : base("[0-9]{6}-[0-9]{4}")
        {
            ErrorMessageResourceType = typeof(ValidationMessages);
            ErrorMessageResourceName = "Invalid";
        }
    }
}
