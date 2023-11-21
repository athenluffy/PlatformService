namespace PlatformService.Models
{
    public class Command
    {
        public int Id { get; set; }
        public int HowTo { get; set; }
        public int Commandline { get; set; }
        public int PlatformId { get; set; }

        public Platform? Platform  {get; set;}
    }
}