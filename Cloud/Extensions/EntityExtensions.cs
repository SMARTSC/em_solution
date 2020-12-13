using EM.Cloud.Api.Common.Models;
using EM.Cloud.Persistence.Entities;

namespace EM.Cloud.Extensions
{
    public static class EntityExtensions
    {
        public static ContentDisplayModel ToDisplayModel(Content entity)
        {
            if (entity is PictureContent)
            {
                var model = new PictureContentDisplayModel();

                model.Import(entity as PictureContent);

                return model;
            }
            else if (entity is PhotoSessionContent)
            {
                var model = new PhotoSessionContentDisplayModel();

                model.Import(entity as PhotoSessionContent);

                return model;
            }

            throw new System.InvalidOperationException();
        }
    }
}