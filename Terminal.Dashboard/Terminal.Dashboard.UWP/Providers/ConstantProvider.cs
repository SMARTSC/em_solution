[assembly: Xamarin.Forms.Dependency(typeof(EM.Terminal.Dashboard.UWP.Providers.ConstantProvider))]

namespace EM.Terminal.Dashboard.UWP.Providers
{
    public sealed class ConstantProvider : Dashboard.Providers.Facade.IConstantProviderFacade
    {
        public int SmallTextFontSize => 10;

        public int NormalTextFontSize => 14;

        public int MiddleTextFontSize => 18;

        public int SmallTitleFontSize => 19;

        public int NormalTitleFontSize => 22;

        public int MiddleTitleFontSize => 25;
    }
}
