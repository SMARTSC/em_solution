[assembly: Xamarin.Forms.Dependency(typeof(EM.Terminal.Dashboard.iOS.Providers.ConstantProvider))]

namespace EM.Terminal.Dashboard.iOS.Providers
{
    public sealed class ConstantProvider : Dashboard.Providers.Facade.IConstantProviderFacade
    {
        public int SmallTextFontSize => 12;

        public int NormalTextFontSize => 16;

        public int MiddleTextFontSize => 20;

        public int SmallTitleFontSize => 21;

        public int NormalTitleFontSize => 24;

        public int MiddleTitleFontSize => 27;
    }
}
