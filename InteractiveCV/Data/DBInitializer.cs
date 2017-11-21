using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InteractiveCV.Models;

namespace InteractiveCV.Data
{
    public class DBInitializer
    {
        public static void Initialize (ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Projects.Any())
            {
                return; // the database has already been seeded
            }

            var projects = new Project[]
            {
                new Project { Title = "Interractive CV", imageUrl = "~/images/dyi.png" },
                new Project {Title = "Project HCRoguelike", imageUrl = "~/images/dyi.png"}
            };

            foreach (Project prj in projects)
            {
                context.Projects.Add(prj);
            }
            context.SaveChanges();

            
            var goals = new ProjectGoal[]
            {
                new ProjectGoal {projectID = 1, goalText = "Do this on the first one"},
                new ProjectGoal {projectID = 1, goalText = "Do this 2 on the first one"},
                new ProjectGoal {projectID = 2, goalText = "do this on the second one"}
            };

            foreach (ProjectGoal goal in goals)
            {
                context.ProjectGoals.Add(goal);
            }
            context.SaveChanges();

            var updates = new ProjectUpdate[]
            {
                new ProjectUpdate {projectID = 1, updateText = "Done this in the first one", updateNumber = 1 },
                new ProjectUpdate {projectID = 1, updateText = "Done this second in the first one", updateNumber = 1 },
                new ProjectUpdate {projectID = 2, updateText = "Done this in the second one", updateNumber = 2 }
            };

            foreach (ProjectUpdate upd in updates)
            {
                context.ProjectUpdates.Add(upd);
            }
            context.SaveChanges();
        }
    }
}
