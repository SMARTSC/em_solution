namespace EM.Cloud.Api.Common.Extensions
{
    public static class ContentExtensions
    {
        public static Enumerations.ContentKind ToContentKind(this Persistence.Entities.Content target)
        {
            if (target is Persistence.Entities.PictureContent)
            {
                return Enumerations.ContentKind.Picture;
            }

            if (target is Persistence.Entities.PhotoSessionContent)
            {
                return Enumerations.ContentKind.Session;
            }

            if (target is Persistence.Entities.AnimateContent)
            {
                return Enumerations.ContentKind.Animate;
            }

            if (target is Persistence.Entities.ClipContent)
            {
                return Enumerations.ContentKind.Clip;
            }

            throw new System.InvalidOperationException();
        }
    }
}
