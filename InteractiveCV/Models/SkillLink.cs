using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveCV.Models
{
    public class SkillLink
    {
        public int SkillLinkID { get; set; }
        public int skillsID { get; set; } //foreign key
        public string Text { get; set; }
        public int? projectNum { get; set; }
        public bool isExternal { get; set; } // if isExternal link will be outside the controller action
        public string externalLink { get; set; }
    }
}
