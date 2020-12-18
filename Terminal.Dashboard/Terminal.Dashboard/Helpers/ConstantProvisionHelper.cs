namespace EM.Terminal.Dashboard.Helpers
{
    public static class ConstantProvisionHelper
    {
        public static int SmallTextFontSize => Xamarin.Forms.DependencyService.Get<Providers.Facade.IConstantProviderFacade>().SmallTextFontSize;

        public static int NormalTextFontSize => Xamarin.Forms.DependencyService.Get<Providers.Facade.IConstantProviderFacade>().NormalTextFontSize;

        public static int MiddleTextFontSize => Xamarin.Forms.DependencyService.Get<Providers.Facade.IConstantProviderFacade>().MiddleTextFontSize;

        public static int SmallTitleFontSize => Xamarin.Forms.DependencyService.Get<Providers.Facade.IConstantProviderFacade>().SmallTitleFontSize;

        public static int NormalTitleFontSize => Xamarin.Forms.DependencyService.Get<Providers.Facade.IConstantProviderFacade>().NormalTitleFontSize;

        public static int MiddleTitleFontSize => Xamarin.Forms.DependencyService.Get<Providers.Facade.IConstantProviderFacade>().MiddleTitleFontSize;
    }
}
