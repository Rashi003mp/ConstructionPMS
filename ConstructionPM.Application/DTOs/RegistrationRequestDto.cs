using ConstructionPM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionPM.Application.DTOs
{
    public class RegistrationRequestDto
    {
        // Common
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;
        public RegistrationRole RoleName { get; set; }
        // PM
        public int? ExperienceYears { get; set; }

        // Engineer
        public string? Skills { get; set; }

        // Client
        public string? ProjectName { get; set; }
    }




}


