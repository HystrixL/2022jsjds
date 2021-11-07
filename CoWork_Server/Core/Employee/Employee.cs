using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Co_Work.Core.Project;

namespace Co_Work.Core.Employee
{
    public enum Level
    {
        Visitor,
        Staff,
        Admin,
        Boss,
    }

    [Table(nameof(Employee))]
    public class Employee
    {
        [Key]public string GUID { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 员工ID
        /// </summary>
        [Required]public string ID { get; set; }
        
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]public string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [Required]public int Age { get; set; }

        /// <summary>
        /// 员工等级
        /// </summary>
        [Required]public Level Level { get; set; } = Level.Visitor;

        /// <summary>
        /// 入职时间
        /// </summary>
        [Required]public DateTime EntryTime { get; set; }

        /// <summary>
        /// 工龄
        /// </summary>
        [Required]public int ServiceLength
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
        [Required]public int TotalTasks { get; set; } = 0;

        /// <summary>
        /// 已完成任务数
        /// </summary>
        [Required]public int FinishedTasks { get; set; } = 0;

        /// <summary>
        /// 完成项目列表
        /// </summary>
        public List<PastProject> Projects { get; set; } = new List<PastProject>();

        /// <summary>
        /// 工龄指标
        /// </summary>
        [Required]public int ServiceLengthRate
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
        [Required]public double UnitTasksRate
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
        [Required]public double TasksCompletionRate
        {
            get
            {
                if (TotalTasks==0)
                {
                    return 0;
                }
                return FinishedTasks / (double)TotalTasks * 100;
            }
        }

        /// <summary>
        /// 任务精度
        /// </summary>
        /// <returns></returns>
        [Required]public double ProjectsAccuracy
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
        [Required]public string Rate
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
        
        /*public Employee(string name,string id, int age, string entryTime)
        {
            Name = name;
            ID = id;
            Age = age;
            EntryTime = DateTime.ParseExact(entryTime, "yyyy-MM-dd", CultureInfo.CurrentCulture);
        }*/

        /// <summary>
        /// 任务完成耗时预测
        /// </summary>
        /// <param name="estimatedTime"></param>
        /// <returns></returns>
        public double TimeConsumingPredict(double estimatedTime)
        {
            double rateScore = Rate[0] switch
            {
                'S' => new Random().NextDouble(0.85,0.90),
                'A' => new Random().NextDouble(0.90,0.95),
                'B' => new Random().NextDouble(0.95,1.00),
                'C' => 1,
                _ => 1
            };

            return estimatedTime * rateScore;
        }
    }
}