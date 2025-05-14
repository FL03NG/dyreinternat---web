namespace dyreinternat___web.Models
{
    public class Booking
    {
        public int AnimalID { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string User {  get; set; }

        public Booking(int animalID, string name, DateTime startTime, DateTime endTime, string user)
        {
            AnimalID = animalID;
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            User = user;
        }
    }
}
