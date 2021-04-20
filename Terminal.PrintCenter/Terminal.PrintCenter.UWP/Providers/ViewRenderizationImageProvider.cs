using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;

[assembly: Xamarin.Forms.Dependency(typeof(EM.Terminal.PrintCenter.UWP.Providers.ViewRenderizationImageProvider))]

namespace EM.Terminal.PrintCenter.UWP.Providers
{
    public class ViewRenderizationImageProvider : PrintCenter.Providers.Facade.IRenderizationImageProviderFacade
    {
        public async System.Threading.Tasks.Task<byte[]> GetRenderizationImageDataAsync(Xamarin.Forms.VisualElement visualElement)
        {
            byte[] result = null;

            using (var viewRenderer = Xamarin.Forms.Platform.UWP.Platform.GetRenderer(visualElement) ?? Xamarin.Forms.Platform.UWP.Platform.CreateRenderer(visualElement))
            {
                var uIElement = viewRenderer.GetNativeElement() ?? viewRenderer.ContainerElement;

                var renderTargetBitmap = new Windows.UI.Xaml.Media.Imaging.RenderTargetBitmap();

                await renderTargetBitmap.RenderAsync(uIElement);

                var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();

                // Support custom DPI
                var displayInformation = Windows.Graphics.Display.DisplayInformation.GetForCurrentView();

                using (var stream = new Windows.Storage.Streams.InMemoryRandomAccessStream())
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);

                    encoder.SetPixelData(
                        BitmapPixelFormat.Bgra8, // RGB with alpha
                        BitmapAlphaMode.Ignore,
                        (uint)renderTargetBitmap.PixelWidth,
                        (uint)renderTargetBitmap.PixelHeight,
                        displayInformation.RawDpiX,
                        displayInformation.RawDpiY,
                        pixelBuffer.ToArray());

                    await encoder.FlushAsync(); // Write data to the stream

                    using (var reader = new Windows.Storage.Streams.DataReader(stream.GetInputStreamAt(0)))
                    {
                        result = new byte[stream.Size];

                        await reader.LoadAsync((uint)stream.Size);

                        reader.ReadBytes(result);
                    }
                }
            }

            return result;
        }
    }
}
