using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveCV.Models
{
    public class Project
    {

        public int ID { get; set; }
        public string Title { get; set; }
        public string imageUrl { get; set; }
        public string Description { get; set; }
        public bool isActive { get; set; }
        public string Color { get; set; }
        public string Status { get; set; }

        public ICollection<ProjectUpdate> updates { get; set; }
        public ICollection<ProjectGoal> goals { get; set; }
    }
}
