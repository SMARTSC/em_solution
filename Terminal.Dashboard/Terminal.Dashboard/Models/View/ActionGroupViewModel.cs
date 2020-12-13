namespace EM.Terminal.Dashboard.Models.View
{
    public class ActionGroupViewModel : System.Collections.Generic.List<ActionViewModel>
    {
        public string Key { get; private set; }

        public ActionGroupViewModel(string key)
        {
            Key = key;
        }

        public ActionGroupViewModel(string key, System.Collections.Generic.IEnumerable<ActionViewModel> items)
            : this(key)
        {
            AddRange(items);
        }
    }
}
