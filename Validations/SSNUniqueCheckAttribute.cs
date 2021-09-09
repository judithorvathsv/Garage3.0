using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Validations
{
    public class SSNUniqueCheckAttribute : ValidationAttribute, IClientModelValidator
    {
        public SSNUniqueCheckAttribute()
        {
            ErrorMessage = "A member with this social security number already exists!";
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            throw new NotImplementedException();
        }

        public override bool IsValid(object value)
        {
            return base.IsValid(value);
        }
    }
}
