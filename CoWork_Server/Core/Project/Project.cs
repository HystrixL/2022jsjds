using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace Co_Work.Core.Project
{
    [Table(nameof(Project))]
    public class Project
    {
        [Key]public string GUID { get; set; } = Guid.NewGuid().ToString();
        
        [Required]public string Name { get; set; }
        [Required]public DateTime StartDate { get; set; }
        [Required]public Employee.Employee Creator { get; set; }
        [Required]public List<Employee.Employee> Members { get; set; } = new List<Employee.Employee>();
        [Required]public string Note { get; set; }
        
        /*public int ProgressRate {
            get
            {
                int finishCount = 0;
                foreach (var subProject in SubProject)
                {
                    if (subProject.IsFinish)
                    {
                        finishCount++;
                    }
                }

                return (int)(finishCount / (double)SubProject.Count * 100);
            }
            set{}
        }*/
        
        [Required]public double ProgressRate { get; set; }
        
        //public List<Project> SubProject { get; set; } = new List<Project>();
        //public bool IsSubProject { get; set; }
        
        [Required]public bool IsFinish { get; set; }
        [Required] public DateTime EndDate { get; set; } = DateTime.MinValue;

        //[Required] public string ProjectFilesPath { get; set; } = Path.Combine(Environment.CurrentDirectory, "CoWorkData","CoWorkData",GUID);

        // public Project(string name,DateTime startTime,Employee creator,List<Employee> employees)
        // {
        //     Name = name;
        //     StartTime = startTime;
        //     Creator = creator;
        //     Members = employees;
        // }

        /*public bool AddSubProject(Project subProject)
        {
            if (subProject.GUID == this.GUID||this.SubProject.Any(p=>p.GUID==subProject.GUID)) return false;
            subProject.IsSubProject = true;
            SubProject.Add(subProject);
            return true;
        }*/
        
        /*public bool AddSubProject(Project[] subProjects)
        {
            if (subProjects.Any(p1 => p1.GUID==this.GUID||this.SubProject.Any(p2=>p1.GUID==p2.GUID)))
            {
                return false;
            }

            foreach (var subProject in subProjects)
            {
                SubProject.Add(subProject);
            }
            return true;
        }*/
        
        public bool AddMember(Employee.Employee member)
        {
            if (this.Members.Any(m=>m.GUID==member.GUID)) return false;
            Members.Add(member);
            return true;
        }
        
        public bool AddMember(Employee.Employee[] members)
        {
            if (members.Any(m1 => this.Members.Any(m2=>m1.GUID==m2.GUID)))
            {
                return false;
            }

            foreach (var member in members)
            {
                Members.Add(member);
            }
            return true;
        }

        /*public double TimeConsumingPredict()
        {
            double predictTime = 0;
            foreach (var member in Members)
            {
                predictTime += member.TimeConsumingPredict((EndTime-StartTime).TotalDays);
            }

            return predictTime / Members.Count;
        }*/
    }

    [Table(nameof(PastProject))]
    public class PastProject
    {
        [Key] public string GUID { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// 项目成本(百万)
        /// </summary>
        [Required]public double Cost { get; set; }

        /// <summary>
        /// 项目用时(年)
        /// </summary>
        [Required]public double Time { get; set; }

        /// <summary>
        /// 项目人数
        /// </summary>
        [Required]public int PeopleNumber { get; set; }

        /// <summary>
        /// 预期收益(百万)
        /// </summary>
        [Required]public double ProspectiveEarnings { get; set; }

        /*public PastProject(double cost, double time, int peopleNumber, double prospectiveEarnings)
        {
            Cost = cost;
            Time = time;
            PeopleNumber = peopleNumber;
            ProspectiveEarnings = prospectiveEarnings;
        }*/
    }
}