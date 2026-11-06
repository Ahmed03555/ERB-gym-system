namespace G_NET_02_MVC01.DAL.Entities
{
    public class Category :BaseEntity
    {
        public string categoryName { get; set; } = null!;
       
        public ICollection<Session> Plans { get; set; }
           = new HashSet<Session>();
    }
}