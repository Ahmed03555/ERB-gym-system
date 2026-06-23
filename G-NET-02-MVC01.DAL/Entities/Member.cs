namespace G_NET_02_MVC01.DAL.Entities
{
    public class Member : GymUser
    {
        public string? Photos { get; set; }= null!;
        public HealthRecord HealthRecord { get; set; } = null!;
        public ICollection<Membership> Memberships { get; set; } = new HashSet<Membership>();
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}