using Garage3.Models.ViewModels;
using Garage3.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Validations
{
    public class NameCheckAttribute : ValidationAttribute, IClientModelValidator
    {
        public NameCheckAttribute()
        {
            ErrorMessage = "First and last name can't be the same.";
            ErrorMessageResourceName = "Name";
            ErrorMessageResourceType = typeof(ValidationMessages);
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var model = (RegisterMemberViewModel)validationContext.ObjectInstance;
            if (model.FirstName == model.LastName)
                return new ValidationResult(ErrorMessage);
            else
                return ValidationResult.Success;
        }
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-namecheck", ErrorMessage);
        }
    }
}
