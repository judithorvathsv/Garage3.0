using Garage3.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Validations
{
    public class NameCheckAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            const string errorMessage = "First and last name can't be the same.";

            var model = (RegisterMemberViewModel)validationContext.ObjectInstance;
            if (model.FirstName == model.LastName)
                return new ValidationResult(errorMessage);
            else
                return ValidationResult.Success;
        }
    }
}
