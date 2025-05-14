namespace dyreinternat___web.Models
{
    public class Event
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Event(string name, string description, DateTime startTime, DateTime endTime)
        {
            Name = name;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
