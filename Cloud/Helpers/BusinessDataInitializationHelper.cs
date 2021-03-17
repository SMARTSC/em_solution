using EM.Cloud.Extensions;
using EM.Cloud.Security.Authorization;
using Microsoft.EntityFrameworkCore;
using SmartSolucionesCuba.SAPRESSC.Core.Security.Persistence.Entities;
using SmartSolucionesCuba.SAPRESSC.Core.Web.SecurityLocal.Extensions;
using System.Linq;

namespace EM.Cloud.Helpers
{
    public sealed class BusinessDataInitializationHelper : SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Helpers.Facade.IDataInitializationHelper
    {
        public void InitialiseData(DbContext dbContext)
        {
            var roleDbSet = dbContext.Set<Role>();
            var groupDbSet = dbContext.Set<Group>();

            var anyGroup = groupDbSet.Any();

            #region Client Roles definition!!
            if (!roleDbSet.Any(role => role.Code.Equals(Roles.ClientViewer)))
            {
                roleDbSet.Add(Roles.ClientViewer.ToRoleEntity());
            }

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.ClientCreator)))
            {
                roleDbSet.Add(Roles.ClientCreator.ToRoleEntity());
            }

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.ClientEditor)))
            {
                roleDbSet.Add(Roles.ClientEditor.ToRoleEntity());
            }

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.ClientRemover)))
            {
                roleDbSet.Add(Roles.ClientRemover.ToRoleEntity());
            }
            #endregion

            #region Event Kind Roles definition!!
            if (!roleDbSet.Any(role => role.Code.Equals(Roles.EventKindViewer)))
            {
                roleDbSet.Add(Roles.EventViewer.ToRoleEntity());
            }

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.EventKindCreator)))
            {
                roleDbSet.Add(Roles.EventKindCreator.ToRoleEntity());
            }

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.EventKindEditor)))
            {
                roleDbSet.Add(Roles.EventKindEditor.ToRoleEntity());
            }

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.EventKindRemover)))
            {
                roleDbSet.Add(Roles.EventKindRemover.ToRoleEntity());
            }
            #endregion

            #region Event Roles definition!!
            var eventViewerRole = Roles.EventViewer.ToRoleEntity();

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.EventViewer)))
            {
                roleDbSet.Add(eventViewerRole);
            }
            else if (!anyGroup)
            {
                eventViewerRole = roleDbSet.First(role => role.Code.Equals(Roles.EventViewer));
            }

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.EventCreator)))
            {
                roleDbSet.Add(Roles.EventCreator.ToRoleEntity());
            }

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.EventEditor)))
            {
                roleDbSet.Add(Roles.EventEditor.ToRoleEntity());
            }

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.EventRemover)))
            {
                roleDbSet.Add(Roles.EventRemover.ToRoleEntity());
            }
            #endregion

            #region Album Roles definition!!
            if (!roleDbSet.Any(role => role.Code.Equals(Roles.AlbumViewer)))
            {
                roleDbSet.Add(Roles.AlbumViewer.ToRoleEntity());
            }

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.AlbumCreator)))
            {
                roleDbSet.Add(Roles.AlbumCreator.ToRoleEntity());
            }

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.AlbumEditor)))
            {
                roleDbSet.Add(Roles.AlbumEditor.ToRoleEntity());
            }

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.AlbumRemover)))
            {
                roleDbSet.Add(Roles.AlbumRemover.ToRoleEntity());
            }
            #endregion

            #region Content Roles definition!!
            var contentViewerRole = Roles.ContentViewer.ToRoleEntity();

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.ContentViewer)))
            {
                roleDbSet.Add(contentViewerRole);
            }
            else if (!anyGroup)
            {
                contentViewerRole = roleDbSet.First(role => role.Code.Equals(Roles.ContentViewer));
            }

            var contentCreatorRole = Roles.ContentCreator.ToRoleEntity();

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.ContentCreator)))
            {
                roleDbSet.Add(contentCreatorRole);
            }
            else if (!anyGroup)
            {
                contentCreatorRole = roleDbSet.First(role => role.Code.Equals(Roles.ContentCreator));
            }

            var contentEditorRole = Roles.ContentEditor.ToRoleEntity();

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.ContentEditor)))
            {
                roleDbSet.Add(contentEditorRole);
            }
            else if (!anyGroup)
            {
                contentEditorRole = roleDbSet.First(role => role.Code.Equals(Roles.ContentEditor));
            }

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.ContentRemover)))
            {
                roleDbSet.Add(Roles.ContentRemover.ToRoleEntity());
            }
            #endregion

            #region Terminal Roles definition!!
            if (!roleDbSet.Any(role => role.Code.Equals(Roles.TerminalViewer)))
            {
                roleDbSet.Add(Roles.TerminalViewer.ToRoleEntity());
            }
            if (!roleDbSet.Any(role => role.Code.Equals(Roles.TerminalEditor)))
            {
                roleDbSet.Add(Roles.TerminalEditor.ToRoleEntity());
            }
            #endregion

            #region Invokation Roles definition!!
            if (!roleDbSet.Any(role => role.Code.Equals(Roles.InvocationViewer)))
            {
                roleDbSet.Add(Roles.InvocationViewer.ToRoleEntity());
            }
            #endregion

            #region CollageLayout Roles definition!!
            if (!roleDbSet.Any(role => role.Code.Equals(Roles.CollageLayoutViewer)))
            {
                roleDbSet.Add(Roles.CollageLayoutViewer.ToRoleEntity());
            }
            #endregion

            #region PrintOrder Roles definition!!
            var printOrderViewerRole = Roles.PrintOrderViewer.ToRoleEntity();

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.PrintOrderViewer)))
            {
                roleDbSet.Add(printOrderViewerRole);
            }
            else if (!anyGroup)
            {
                printOrderViewerRole = roleDbSet.First(role => role.Code.Equals(Roles.PrintOrderViewer));
            }

            var printOrderCreator = Roles.PrintOrderCreator.ToRoleEntity();

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.PrintOrderCreator)))
            {
                roleDbSet.Add(printOrderCreator);
            }
            else if (!anyGroup)
            {
                printOrderCreator = roleDbSet.First(role => role.Code.Equals(Roles.PrintOrderCreator));
            }

            var printOrderEditorRole = Roles.PrintOrderEditor.ToRoleEntity();

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.PrintOrderEditor)))
            {
                roleDbSet.Add(printOrderEditorRole);
            }
            else if (!anyGroup)
            {
                printOrderEditorRole = roleDbSet.First(role => role.Code.Equals(Roles.PrintOrderEditor));
            }

            if (!roleDbSet.Any(role => role.Code.Equals(Roles.PrintOrderRemover)))
            {
                roleDbSet.Add(Roles.PrintOrderRemover.ToRoleEntity());
            }
            #endregion

            #region Print Order Stack Roles definition!!
            if (!roleDbSet.Any(role => role.Code.Equals(Roles.PrintOrderStackManager)))
            {
                roleDbSet.Add(Roles.PrintOrderStackManager.ToRoleEntity());
            }
            #endregion

            #region Trace Roles definition!!
            if (!roleDbSet.Any(role => role.Code.Equals(Roles.TraceViewer)))
            {
                roleDbSet.Add(Roles.TraceViewer.ToRoleEntity());
            }
            #endregion

            #region Collage Layouts definitios
            var collageLayoutDbSet = dbContext.Set<Persistence.Entities.CollageLayout>();

            var descriptors = Extend.Layout.Provision.Providers.CollageLayoutDescriptorProvider.Instance.GetCollageLayoutDescriptors();

            foreach (var descriptor in descriptors)
            {
                if (!collageLayoutDbSet.Any(entity => entity.Id.Equals(descriptor.Id)))
                {
                    collageLayoutDbSet.Add(descriptor.ToEntity());
                }
            }
            #endregion

            if (dbContext is SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Context.ITraceableDbContext)
            {
                (dbContext as SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Context.ITraceableDbContext).SaveChanges("system");
            }
            else
            {
                dbContext.SaveChanges();
            }

            #region Groups definitions

            if (!anyGroup)
            {
                var photoMirrorGroup = new Group
                {
                    Id = System.Guid.NewGuid(),
                    Name = "PhotoMirror Users"
                };

                photoMirrorGroup.GroupRoleBridges.Add(new GroupRoleBridge
                {
                    GroupId = photoMirrorGroup.Id,
                    RoleId = eventViewerRole.Id
                });
                photoMirrorGroup.GroupRoleBridges.Add(new GroupRoleBridge
                {
                    GroupId = photoMirrorGroup.Id,
                    RoleId = contentCreatorRole.Id
                });
                photoMirrorGroup.GroupRoleBridges.Add(new GroupRoleBridge
                {
                    GroupId = photoMirrorGroup.Id,
                    RoleId = contentEditorRole.Id
                });

                groupDbSet.Add(photoMirrorGroup);

                var printCenterGroup = new Group
                {
                    Id = System.Guid.NewGuid(),
                    Name = "PrintCenter Users"
                };

                printCenterGroup.GroupRoleBridges.Add(new GroupRoleBridge
                {
                    GroupId = printCenterGroup.Id,
                    RoleId = eventViewerRole.Id
                });
                printCenterGroup.GroupRoleBridges.Add(new GroupRoleBridge
                {
                    GroupId = printCenterGroup.Id,
                    RoleId = contentViewerRole.Id
                });
                printCenterGroup.GroupRoleBridges.Add(new GroupRoleBridge
                {
                    GroupId = printCenterGroup.Id,
                    RoleId = printOrderCreator.Id
                });

                groupDbSet.Add(printCenterGroup);

                var printServiceGroup = new Group
                {
                    Id = System.Guid.NewGuid(),
                    Name = "PrintService Users"
                };

                printServiceGroup.GroupRoleBridges.Add(new GroupRoleBridge
                {
                    GroupId = printServiceGroup.Id,
                    RoleId = printOrderViewerRole.Id
                });
                printServiceGroup.GroupRoleBridges.Add(new GroupRoleBridge
                {
                    GroupId = printServiceGroup.Id,
                    RoleId = printOrderEditorRole.Id
                });

                groupDbSet.Add(printServiceGroup);
            }

            #endregion

            if (dbContext is SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Context.ITraceableDbContext)
            {
                (dbContext as SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Context.ITraceableDbContext).SaveChanges("system");
            }
            else
            {
                dbContext.SaveChanges();
            }
        }
    }
}
