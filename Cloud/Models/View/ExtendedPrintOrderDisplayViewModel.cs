namespace EM.Cloud.Models.View
{
    public class ExtendedPrintOrderDisplayViewModel : PrintOrderDisplayViewModel
    {
        public string Base64ImageData { get; set; }

        public override void Import(Persistence.Entities.PrintOrder entity)
        {
            base.Import(entity);

            Base64ImageData = System.Convert.ToBase64String(entity.Data);
        }
    }
}
