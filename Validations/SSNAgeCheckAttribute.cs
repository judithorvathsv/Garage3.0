using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Validations
{
    public class SSNAgeCheckAttribute : ValidationAttribute, IClientModelValidator
    {
        public SSNAgeCheckAttribute()
        {

        }
        public void AddValidation(ClientModelValidationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
