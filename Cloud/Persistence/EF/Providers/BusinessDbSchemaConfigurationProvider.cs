using System.Collections.Generic;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Configurators;

namespace EM.Cloud.Persistence.EF.Providers
{
    public sealed class BusinessDbSchemaConfigurationProvider : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Providers.Facade.IDbSchemaConfigurationProviderFacade
    {
        public string DefaultPersistenceSchema => null;

        public ICollection<IEntityTypeConfigurator> EntityTypeConfigurators => new List<IEntityTypeConfigurator>
        {
            new Configurators.ContentEntityTypeConfigurator(),
            new Configurators.PictureContentEntityTypeConfigurator(),
            new Configurators.ClipContentEntityTypeConfigurator(),
            new Configurators.PhotoSessionContentEntityTypeConfigurator(),
            new Configurators.PhotoSessionContentImageBridgeEntityTypeConfigurator(),
            new Configurators.AnimateContentEntityTypeConfigurator(),
            new Configurators.AnimateContentImageBridgeEntityTypeConfigurator(),
            new Configurators.AlbumEntityTypeConfigurator(),
            new Configurators.CollageLayoutEntityTypeConfigurator(),
            new Configurators.AlbumLockBridgeEntityTypeConfigurator(),
            new Configurators.ClientEntityTypeConfigurator(),
            new Configurators.PersonClientEntityTypeConfigurator(),
            new Configurators.OrganizationClientEntityTypeConfigurator(),
            new Configurators.EventEntityTypeConfigurator(),
            new Configurators.ContractingEventClientBridgeEntityTypeConfigurator(),
            new Configurators.PlacingEventClientBridgeEntityTypeConfigurator(),
            new Configurators.SponsoringEventClientBridgeEntityTypeConfigurator(),
            new Configurators.AlbumCollageLayoutBridgeEntityTypeConfigurator(),
            new Configurators.TerminalAlbumBridgeEntityTypeConfigurator()
        };
    }
}
