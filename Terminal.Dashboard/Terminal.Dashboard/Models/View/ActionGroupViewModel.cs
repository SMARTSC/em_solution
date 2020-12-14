namespace EM.Terminal.Dashboard.Models.View
{
    public class ActionGroupViewModel
    {
        public string Key { get; private set; }

        public System.Collections.Generic.List<ActionViewModel> Actions { get; private set; }

        public ActionGroupViewModel(string key, System.Collections.Generic.List<ActionViewModel> items = null)
        {
            Key = key;
            Actions = items ?? new System.Collections.Generic.List<ActionViewModel>();
        }
    }
}
