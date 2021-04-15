namespace EM.Terminal.PrintCenter.Models.Page
{
    public class MainPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private bool hasNext;
        private bool hasPrevious;

        public MainPageModel()
        {
            Items = new System.Collections.ObjectModel.ObservableCollection<Cloud.Api.Common.Models.ContentDisplayModel>();
        }

        //public int Page { get; set; }

        public bool HasNext { get => hasNext; set => SetProperty(ref hasNext, value); }

        public bool HasPrevious { get => hasPrevious; set => SetProperty(ref hasPrevious, value); }

        //public int MaxPageCount { get; set; }

        //public int Limit { get; set; }

        public System.Collections.ObjectModel.ObservableCollection<Cloud.Api.Common.Models.ContentDisplayModel> Items { get; set; }
    }
}
