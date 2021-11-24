namespace EM.Print.Persistence.Entities
{
    public class PrintOrder : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Entities.IHabilitableEntity<long>
    {
        public PrintOrder()
        {
            Created = System.DateTime.Now;
            Created = System.DateTime.MinValue;
        }

        [System.ComponentModel.DataAnnotations.Key]
        public long Id { get; set; }

        public bool Enabled { get; set; }

        public System.DateTime Created { get; set; }

        public System.DateTime Attended { get; set; }

        public int Count { get; set; }

        public System.Guid TargetContentEventLogoImageId { get; set; }

        public System.Guid TargetContentAlbumId { get; set; }

        public System.Guid TargetContentId { get; set; }

        // Null => PictureContent, !Null => PhotoSessionContent
        public string PermutationSerialied { get; set; }
    }
}
