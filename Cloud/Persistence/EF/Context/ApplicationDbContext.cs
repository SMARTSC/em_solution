using System.Collections.Generic;
using EM.Cloud.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Providers.Facade;
using SmartSolucionesCuba.SAPRESSC.Core.Security.Persistence.Entities;
using SmartSolucionesCuba.SAPRESSC.Middleware.Identity.Persistence.Entities;
using SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities;
using SmartSolucionesCuba.SAPRESSC.Middleware.Region.Persistence.Entities;

namespace EM.Cloud.Persistence.EF.Context
{
    public sealed class ApplicationDbContext :
        SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Context.AbstractTraceableDbContext,
        SmartSolucionesCuba.SAPRESSC.Middleware.Identity.Persistence.EF.Context.IIdentityMiddlewarePersistenceDbContext,
        SmartSolucionesCuba.SAPRESSC.Core.Security.Persistence.EF.Context.ISecurityPersistenceModuleDbContext
    {
        public ApplicationDbContext(IEnumerable<IDbSchemaConfigurationProviderFacade> dbSchemaConfigurationProviders, DbContextOptions options) : base(dbSchemaConfigurationProviders, options)
        {
        }

        public DbSet<Content> Contents { get; set; }
        public DbSet<PictureContent> PictureContents { get; set; }
        public DbSet<PhotoSessionContent> PhotoSessionContents { get; set; }
        public DbSet<AnimateContent> AnimateContents { get; set; }
        public DbSet<ClipContent> ClipContents { get; set; }
        public DbSet<AnimateContentImageBridge> AnimateContentImageBridges { get; set; }
        public DbSet<PhotoSessionContentImageBridge> PhotoSessionContentImageBridges { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<CollageLayout> CollageLayouts { get; set; }
        public DbSet<AlbumLockBridge> AlbumLockBridges { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventKind> EventKinds { get; set; }
        public DbSet<AlbumCollageLayoutBridge> AlbumCollageLayoutBridges { get; set; }
        public DbSet<PrintOrder> PrintOrders { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<PersonClient> PersonClients { get; set; }
        public DbSet<OrganizationClient> OrganizationClients { get; set; }
        public DbSet<ContractingEventClientBridge> ContractingEventClientBridges { get; set; }
        public DbSet<PlacingEventClientBridge> PlacingEventClientBridges { get; set; }
        public DbSet<SponsoringEventClientBridge> SponsoringEventClientBridges { get; set; }
        public DbSet<Terminal> Terminals { get; set; }
        public DbSet<TerminalAlbumBridge> TerminalAlbumBridges { get; set; }
        public DbSet<Invokation> Invokations { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserGroupBridge> UserGroupBridges { get; set; }
        public DbSet<GroupRoleBridge> GroupRoleBridges { get; set; }

        public DbSet<PersonIdentity> PersonIdentities { get; set; }
        public DbSet<OrganizationIdentity> OrganizationIdentities { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<EmailContactItem> EmailContactItems { get; set; }
        public DbSet<PhoneContactItem> PhoneContactItems { get; set; }
        public DbSet<SocialContactItem> SocialContactItems { get; set; }
        public DbSet<WebSiteContactItem> WebSiteContactItems { get; set; }
        public DbSet<PostalContactItem> PostalContactItems { get; set; }
        public DbSet<PersonEmailContactItemBridge> PersonEmailContactItemBridges { get; set; }
        public DbSet<PersonPhoneContactItemBridge> PersonPhoneContactItemBridges { get; set; }
        public DbSet<PersonPostalContactItemBridge> PersonPostalContactItemBridges { get; set; }
        public DbSet<PersonSocialContactItemBridge> PersonSocialContactItemBridges { get; set; }
        public DbSet<PersonWebSiteContactItemBridge> PersonWebSiteContactItemBridges { get; set; }
        public DbSet<OrganizationEmailContactItemBridge> OrganizationEmailContactItemBridges { get; set; }
        public DbSet<OrganizationPhoneContactItemBridge> OrganizationPhoneContactItemBridges { get; set; }
        public DbSet<OrganizationPostalContactItemBridge> OrganizationPostalContactItemBridges { get; set; }
        public DbSet<OrganizationSocialContactItemBridge> OrganizationSocialContactItemBridges { get; set; }
        public DbSet<OrganizationWebSiteContactItemBridge> OrganizationWebSiteContactItemBridges { get; set; }

        public DbSet<Thumbnail> Thumbnails { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Video> Videos { get; set; }

        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<Locality> Localities { get; set; }
        public DbSet<CountryRegion> CountryRegions { get; set; }
        public DbSet<StateRegion> StateRegions { get; set; }
        public DbSet<MunicipalityRegion> MunicipalityRegions { get; set; }
        public DbSet<LocalityRegion> LocalityRegions { get; set; }
        public DbSet<Region> Regions { get; set; }
    }
}
