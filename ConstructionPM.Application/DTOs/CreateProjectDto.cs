using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionPM.Domain.Enums;

namespace ConstructionPM.Application.DTOs
{
    public class CreateProjectDto

    {
        public string ProjectName { get; set; } = null!;

        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ProjectStatus Status { get; set; }
    }
}
