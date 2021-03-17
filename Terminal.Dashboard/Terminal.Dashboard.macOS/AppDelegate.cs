using AppKit;
using Foundation;

namespace EM.Terminal.Dashboard.macOS
{
    [Register("AppDelegate")]
    public class AppDelegate : Xamarin.Forms.Platform.MacOS.FormsApplicationDelegate
    {
        private readonly NSWindow window;

        public AppDelegate()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

            var rect = new CoreGraphics.CGRect(200, 1000, 1024, 600);

            window = new NSWindow(rect, style, NSBackingStore.Buffered, false)
            {
                Title = "Espejo Molón Cloud Dashboard", // choose your own Title here
                TitleVisibility = NSWindowTitleVisibility.Hidden,
                MinSize = new CoreGraphics.CGSize(1024, 600)
            };
        }

        public override NSWindow MainWindow
        {
            get { return window; }
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            Helpers.ToastNotificationHelper.Init();

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            base.DidFinishLaunching(notification);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }

        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
        {
            return true;
        }
    }
}
