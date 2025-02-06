using DynamicTimeTableGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DynamicTimeTableGenerator.Controllers
{
    public class TimeTableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EnterSubjects(TimeTableModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model); 

            var subjects = new List<SubjectHours>();
            int baseHours = model.TotalHoursForWeek / model.TotalSubjects;
            int remainingHours = model.TotalHoursForWeek % model.TotalSubjects;

            for (int i = 0; i < model.TotalSubjects; i++)
            {
                subjects.Add(new SubjectHours
                {
                    Name = $"Subject {i + 1}",
                    Hours = baseHours + (remainingHours-- > 0 ? 1 : 0)
                });
            }

            var subjectHoursModel = new SubjectHoursModel
            {
                TotalHoursForWeek = model.TotalHoursForWeek,
                Subjects = subjects
            };

            return View("EnterSubjects", subjectHoursModel);
        }

        [HttpPost]
        public IActionResult GenerateTimeTable(SubjectHoursModel model)
        {
            var timeTable = GenerateTimeTableGrid(model);
            return View("TimeTable", timeTable);
        }

        private List<List<string>> GenerateTimeTableGrid(SubjectHoursModel model)
        {
            var subjectPool = new List<string>();
            foreach (var subject in model.Subjects)
                subjectPool.AddRange(Enumerable.Repeat(subject.Name, subject.Hours));

            var timeTable = new List<List<string>>();
            int rows = model.Subjects.Count;
            int cols = model.TotalHoursForWeek / rows;

            for (int i = 0; i < rows; i++)
            {
                var row = new List<string>();
                for (int j = 0; j < cols; j++)
                {
                    if (subjectPool.Any())
                    {
                        row.Add(subjectPool.First());
                        subjectPool.RemoveAt(0);
                    }
                    else
                    {
                        row.Add("");
                    }
                }
                timeTable.Add(row);
            }
            return timeTable;
        }
    }
}
