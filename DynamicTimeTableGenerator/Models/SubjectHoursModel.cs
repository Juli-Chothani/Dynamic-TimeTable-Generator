namespace DynamicTimeTableGenerator.Models
{
    public class SubjectHoursModel
    {
        public int TotalHoursForWeek { get; set; }
        public List<SubjectHours> Subjects { get; set; }
    }

    public class SubjectHours
    {
        public string Name { get; set; }
        public int Hours { get; set; }
    }

}
