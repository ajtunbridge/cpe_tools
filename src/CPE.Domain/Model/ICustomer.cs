namespace CPE.Domain.Model
{
    public interface ICustomer : IEntity
    {
        string Name { get; set; }

        int? TricornReference { get; set; }

        byte[] LogoBlob { get; set; }
    }
}