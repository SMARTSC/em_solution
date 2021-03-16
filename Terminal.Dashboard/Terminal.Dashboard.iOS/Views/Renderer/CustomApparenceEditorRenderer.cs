[assembly: Xamarin.Forms.ExportRenderer(typeof(Xamarin.Forms.Editor), typeof(EM.Terminal.Dashboard.iOS.Views.Renderer.CustomApparenceEditorRenderer))]

namespace EM.Terminal.Dashboard.iOS.Views.Renderer
{
    public class CustomApparenceEditorRenderer : Xamarin.Forms.Platform.iOS.EditorRenderer
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<Xamarin.Forms.Editor> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.Layer.CornerRadius = 5;
                Control.Layer.BorderColor = UIKit.UIColor.FromWhiteAlpha(0, 0.45f).CGColor;
                Control.Layer.BorderWidth = 0.5f;

                Control.TextContainerInset = new UIKit.UIEdgeInsets(4, 2, 4, 2);
            }
        }
    }
}
