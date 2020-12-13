namespace EM.Terminal.Dashboard.Models.View
{
    public class ActionViewModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.BaseBindableModel
    {
        public string IconGlyph { get; set; }

        public string Caption { get; set; }

        public Enumerations.ActionKind Kind { get; set; }

        public bool IsEnabled { get; set; }
    }
}