namespace CPE.Domain.Model
{
    public interface IPart : IEntity
    {
        string DrawingNumber { get; set; }

        string Name { get; set; }

        string ToolingLocation { get; set; }

        int CustomerId { get; set; }
    }
}