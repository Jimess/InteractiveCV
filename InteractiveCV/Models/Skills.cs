using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveCV.Models
{
    public class Skills
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Experience { get; set; }
        public string RelatedExperience { get; set; }

        public List<SkillLink> Links { get; set; }
    }
}
