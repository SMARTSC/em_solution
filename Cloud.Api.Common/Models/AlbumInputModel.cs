namespace EM.Cloud.Api.Common.Models
{
    public class AlbumInputModel : SmartSolucionesCuba.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.Album, System.Guid>
    {
        public System.Guid Id { get; set; }

        public Persistence.Entities.Album Export()
        {
            throw new System.NotImplementedException();
        }

        public void Import(Persistence.Entities.Album entity)
        {
            throw new System.NotImplementedException();
        }

        public void Mergue(Persistence.Entities.Album entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
