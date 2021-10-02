using System;
using System.Collections.Generic;
using System.Globalization;

namespace EmployeesHierarchicalQuantitative
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee employee = new Employee("Huang Enhao", 31, "2015-03-27");
            employee.TotalTasks = 10;
            employee.FinishedTasks = 8;
            Project project1 = new Project(5.0, 3, 15, 10);
            Project project2 = new Project(8.0, 2, 23, 15);
            employee.Projects = new List<Project>() { project1, project2 };

            Console.WriteLine(employee.ServiceLengthRate);
            Console.WriteLine(employee.UnitTasksRate);
            Console.WriteLine(employee.TasksCompletionRate);
            Console.WriteLine(employee.ProjectsAccuracy);

            Console.WriteLine(employee.Rate);

            double predictTime = employee.TimeConsumingPredict(5, 1);
            Console.WriteLine(predictTime);
        }
    }

    class Employee
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime EntryTime { get; }

        /// <summary>
        /// 工龄
        /// </summary>
        public int ServiceLength
        {
            get
            {
                var nowDate = DateTime.Now;
                var timeSpan = nowDate - EntryTime;
                return timeSpan.Days / 365;
            }
        }

        /// <summary>
        /// 总任务数
        /// </summary>
        public int TotalTasks { get; set; } = 0;

        /// <summary>
        /// 已完成任务数
        /// </summary>
        public int FinishedTasks { get; set; } = 0;

        /// <summary>
        /// 完成项目列表
        /// </summary>
        public List<Project> Projects { get; set; } = new List<Project>();

        /// <summary>
        /// 工龄指标
        /// </summary>
        public int ServiceLengthRate
        {
            get
            {
                double oriServiceLengthScore = Math.Log(ServiceLength, Math.E / 2.0) - ServiceLength / Math.E;

                int serviceLengthScore = oriServiceLengthScore switch
                {
                    < 1 => 60,
                    >= 1 and < 2 => 70,
                    >= 2 and < Math.PI => 80,
                    >= Math.PI and < Math.PI + 0.5 => 90,
                    >= Math.PI + 0.5 and < 3.9 => 100,
                    _ => 0
                };

                return serviceLengthScore;
            }
        }

        /// <summary>
        /// 单位任务数
        /// </summary>
        public double UnitTasksRate
        {
            get
            {
                double unitTasks = TotalTasks / (double)ServiceLength;
                int unitTasksScore = unitTasks switch
                {
                    >=3 => 100,
                    >1 and <3 => 75,
                    >0 and <=1 => 50,
                    0 => 0,
                    _ => 0
                };
                return unitTasksScore;
            }
        }

        /// <summary>
        /// 任务完成率
        /// </summary>
        public double TasksCompletionRate
        {
            get { return FinishedTasks / (double)TotalTasks * 100; }
        }

        /// <summary>
        /// 任务精度
        /// </summary>
        /// <returns></returns>
        public double ProjectsAccuracy
        {
            get
            {
                if (Projects.Count == 0) return 0;

                double earningsRate = 0;
                foreach (var project in Projects)
                {
                    double sumEarnings = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        double earningsRatio = Math.Log(project.Cost + project.Time + project.PeopleNumber,
                                                   Math.Pow(Math.E, project.Time)) *
                                               (0.9 + (new Random().NextDouble()) * 0.15);
                        double earnings = earningsRatio * project.Cost;
                        sumEarnings += earnings;
                    }

                    sumEarnings -= project.Cost;
                    earningsRate += sumEarnings / project.ProspectiveEarnings;
                }

                earningsRate /= Projects.Count;

                earningsRate = earningsRate switch
                {
                    > 1 => 1,
                    < 0 => 0,
                    _ => earningsRate
                };

                return earningsRate * 100;
            }
        }

        /// <summary>
        /// 总分评级
        /// </summary>
        public string Rate
        {
            get
            {
                double[] weights = { 0.2, 0.2, 0.3, 0.3 };
                double rateScore = ServiceLengthRate * weights[0] + UnitTasksRate * weights[1] +
                                   TasksCompletionRate * weights[2] + ProjectsAccuracy * weights[3];
                string rate = rateScore switch
                {
                    >90 => "S",
                    >85 and <=90 => "A+",
                    >75 and <=85 => "A",
                    >70 and <=75 => "A-",
                    >65 and <=70 => "B+",
                    >55 and <=65 => "B",
                    >50 and <=55 => "B-",
                    <=50 => "C",
                    _ => ""
                };

                return rate;
            }
        }

        public Employee(string name, int age, string entryTime)
        {
            Name = name;
            Age = age;
            EntryTime = DateTime.ParseExact(entryTime, "yyyy-MM-dd", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// 任务完成耗时预测
        /// </summary>
        /// <param name="totalTime">总限时</param>
        /// <param name="lastTime">上次耗时与总限时之比</param>
        /// <returns></returns>
        public double TimeConsumingPredict(double totalTime, double lastTime)
        {
            double rateScore = Rate[0] switch
            {
                'S' => 0.85 + (new Random().NextDouble()) * 0.05,
                'A' => 0.9 + (new Random().NextDouble()) * 0.05,
                'B' => 0.95 + (new Random().NextDouble()) * 0.05,
                'C' => 1,
                _ => 1
            };

            return totalTime * rateScore * lastTime;
        }
    }

    class Project
    {
        /// <summary>
        /// 项目成本(百万)
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        /// 项目用时(年)
        /// </summary>
        public double Time { get; set; }

        /// <summary>
        /// 项目人数
        /// </summary>
        public int PeopleNumber { get; set; }

        /// <summary>
        /// 预期收益(百万)
        /// </summary>
        public double ProspectiveEarnings { get; set; }

        public Project(double cost, double time, int peopleNumber, double prospectiveEarnings)
        {
            Cost = cost;
            Time = time;
            PeopleNumber = peopleNumber;
            ProspectiveEarnings = prospectiveEarnings;
        }
    }
}