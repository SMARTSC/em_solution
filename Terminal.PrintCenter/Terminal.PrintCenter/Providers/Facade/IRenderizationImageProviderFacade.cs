namespace EM.Terminal.PrintCenter.Providers.Facade
{
    public interface IRenderizationImageProviderFacade
    {
        System.Threading.Tasks.Task<byte[]> GetRenderizationImageDataAsync(Xamarin.Forms.VisualElement visualElement);
    }
}
