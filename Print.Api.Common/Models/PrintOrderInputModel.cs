namespace EM.Print.Api.Common.Models
{
    public class PrintOrderInputModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.PrintOrder, long>
    {
        public long Id { get; set; }

        public bool Enabled { get; set; }

        public System.DateTime Created { get; set; }

        public System.DateTime Attended { get; set; }

        public int Count { get; set; }

        public System.Guid TargetContentEventLogoImageId { get; set; }

        public System.Guid TargetContentAlbumId { get; set; }

        public System.Guid TargetContentId { get; set; }

        public string PermutationSerialied { get; set; }

        public Persistence.Entities.PrintOrder Export()
        {
            var entity = new Persistence.Entities.PrintOrder
            {
                Created = Created,
                Count = Count,
                TargetContentEventLogoImageId = TargetContentEventLogoImageId,
                TargetContentAlbumId = TargetContentAlbumId,
                TargetContentId = TargetContentId,
                PermutationSerialied = PermutationSerialied
            };

            Mergue(entity);

            return entity;
        }

        public void Import(Persistence.Entities.PrintOrder entity)
        {
            throw new System.InvalidOperationException();
        }

        public void Mergue(Persistence.Entities.PrintOrder entity)
        {
            entity.Enabled = Enabled;
            entity.Attended = Attended;
        }
    }
}
