namespace EventTicketing.DTOs.Events
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartsOn { get; set; }
        public DateTime EndsOn { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
    }
}
