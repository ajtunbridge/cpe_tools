namespace CPE.Domain.Model
{
    public interface ICustomer : IEntity
    {
        string Name { get; set; }

        int? TricornReference { get; set; }

        byte[] LogoBlob { get; set; }

        bool HasSalesOrderParserSettings { get; }

        void SetSalesOrderParserSettings(SalesOrderParserSettingsBlob settings);

        SalesOrderParserSettingsBlob GetSalesOrderParserSettings();
    }
}