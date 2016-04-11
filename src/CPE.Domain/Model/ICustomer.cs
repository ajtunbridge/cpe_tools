namespace CPE.Domain.Model
{
    public interface ICustomer
    {
        string Name { get; set; }

        int? TricornReference { get; set; }

        byte[] LogoBlob { get; set; }
    }
}