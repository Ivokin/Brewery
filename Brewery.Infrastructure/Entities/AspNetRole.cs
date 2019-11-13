namespace Brewery.Infrastructure.Entities
{
    public class AspNetRole
    {
        public AspNetRole()
        {
        }

        public string Id { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Name { get; set; }
    }
}
