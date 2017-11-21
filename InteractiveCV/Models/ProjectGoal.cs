using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveCV.Models
{
    public class ProjectGoal
    {
        public int projectGoalID { get; set; }
        public int projectID { get; set; }

        public string goalText { get; set; }
    }
}
