﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
    public class RegisterVehicleViewModel
    {
        [Required]
        public string SocialSecurityNumber {  get; set; }
        public string Name {  get; set; }

        [Required]
        public string RegistrationNumber {  get; set; }
        public string Brand {  get; set; }
        public string VehicleModel {  get; set; }
        public VehicleType VehicleType {  get; set; }
        public IEnumerable<SelectListItem> VehicleTypes { get; set; }


    }
}