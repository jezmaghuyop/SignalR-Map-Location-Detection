namespace SignalR.MapMarker.Models
{
    public class UserInfo
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }

        public Coordinate Location { get; set; }
    }
}
