namespace EM.Print.Api.Common.Models
{
    public class PrintOrderDisplayModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.PrintOrder, long>
    {
        public long Id { get; set; }

        public bool Enabled { get; set; }

        public System.DateTime Created { get; set; }

        public System.DateTime Attended { get; set; }

        public int Count { get; set; }

        public System.Guid TargetContentEventLogoImageId { get; set; }

        public System.Guid TargetContentAlbumId { get; set; }

        public System.Guid TargetContentId { get; set; }

        public string PermutationSerialized { get; set; }

        public void Import(Persistence.Entities.PrintOrder entity)
        {
            Id = entity.Id;
            Enabled = entity.Enabled;
            Created = entity.Created;
            Attended = entity.Attended;
            Count = entity.Count;
            TargetContentEventLogoImageId = entity.TargetContentEventLogoImageId;
            TargetContentAlbumId = entity.TargetContentAlbumId;
            TargetContentId = entity.TargetContentId;
            PermutationSerialized = entity.PermutationSerialied;
        }
    }
}
