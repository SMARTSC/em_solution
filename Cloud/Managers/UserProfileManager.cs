namespace EM.Cloud.Managers
{
    public class UserProfileManager : SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Managers.Facade.IUserProfileManagerFacade
    {
        public SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Models.View.IUserProfileViewModel GetUserProfile(string username)
        {
            return new Models.View.UserProfileViewModel();
        }

        public void UpdateUserProfile(string username, SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Models.View.IUserProfileViewModel profileModel)
        {
        }
    }
}
