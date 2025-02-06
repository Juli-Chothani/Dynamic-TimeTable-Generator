using System.ComponentModel.DataAnnotations;

namespace DynamicTimeTableGenerator.Models
{
    public class TimeTableModel
    {
        [Required(ErrorMessage = "Number of working days is required.")]
        [Range(1, 7, ErrorMessage = "Number of working days must be between 1 and 7.")]
        public int WorkingDays { get; set; }

        [Required(ErrorMessage = "Subjects per day is required.")]
        [Range(1, 8, ErrorMessage = "Subjects per day must be less than 9.")]
        public int SubjectsPerDay { get; set; }

        [Required(ErrorMessage = "Total subjects are required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Total subjects must be a positive number.")]
        public int TotalSubjects { get; set; }

        public int TotalHoursForWeek => WorkingDays * SubjectsPerDay;
    }
}
