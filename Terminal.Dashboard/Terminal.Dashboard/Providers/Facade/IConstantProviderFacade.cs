namespace EM.Terminal.Dashboard.Providers.Facade
{
    public interface IConstantProviderFacade
    {
        int SmallTextFontSize { get; }
        int NormalTextFontSize { get; }
        int MiddleTextFontSize { get; }

        int SmallTitleFontSize { get; }
        int NormalTitleFontSize { get; }
        int MiddleTitleFontSize { get; }
    }
}
