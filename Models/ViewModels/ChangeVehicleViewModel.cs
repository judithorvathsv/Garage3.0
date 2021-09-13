using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Garage3.Models.ViewModels
{
    public class ChangeVehicleViewModel
    {
            [Required]
            public int Id { get; set; }
            [Required]
            public string RegistrationNumber { get; set; }
            public int OwnerId { get; set; }
            public string Brand { get; set; }
            public string VehicleModel { get; set; }

            public int VehicleTypeId { get; set; }
            public IEnumerable<SelectListItem> VehicleTypes { get; set; }
        
    }
}
