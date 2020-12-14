using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EM.Terminal.Dashboard.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageEventAlbumPhotoSessionContentsPage : ContentPage
    {
        public ManageEventAlbumPhotoSessionContentsPage(Cloud.Backend.Api.Common.Models.AlbumDisplayModel album)
        {
            InitializeComponent();
        }
    }
}