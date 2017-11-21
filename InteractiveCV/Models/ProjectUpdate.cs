using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveCV.Models
{
    public class ProjectUpdate
    {
        public int projectUpdateID { get; set; }
        public int projectID { get; set; }
        public int updateNumber { get; set; }
        public string updateText { get; set; }

    }
}
