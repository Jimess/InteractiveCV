using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveCV.Models
{
    public class SkillLink
    {
        public int SkillLinkID { get; set; }
        public int skillsID { get; set; }
        public string Text { get; set; }
        public int projectNum { get; set; }
    }
}
