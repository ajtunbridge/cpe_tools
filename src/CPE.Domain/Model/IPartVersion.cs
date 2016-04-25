namespace CPE.Domain.Model
{
    public interface IPartVersion : IEntity
    {
        string VersionNumber { get; set; }

        int PartId { get; set; }

        int? DrawingDocumentId { get; set; }
    }
}