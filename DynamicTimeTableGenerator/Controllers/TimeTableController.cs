using DynamicTimeTableGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DynamicTimeTableGenerator.Controllers
{
    public class TimeTableController : Controller
    {
        // Step 1: Show the initial form
        public IActionResult Index()
        {
            return View();
        }

        // Step 2: Process the entered input
        [HttpPost]
        public IActionResult EnterSubjects(TimeTableModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model); // Return the form with validation errors

            // Distribute the subject hours automatically
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

            // Step 3: Send the model with subject hours to the next view
            return View("EnterSubjects", subjectHoursModel);
        }

        // Step 4: Generate the time-table grid based on the input
        [HttpPost]
        public IActionResult GenerateTimeTable(SubjectHoursModel model)
        {
            var timeTable = GenerateTimeTableGrid(model);
            return View("TimeTable", timeTable);
        }

        // Helper method to generate the time-table grid
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
