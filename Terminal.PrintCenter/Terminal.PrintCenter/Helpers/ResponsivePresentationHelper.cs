namespace EM.Terminal.PrintCenter.Helpers
{
    public static class ResponsivePresentationHelper
    {
        private static double NTSC_ASPECT_RATIO = 960d / 720d;
        private static double WIDESCREEN_ASPECT_RATIO_16X9 = 1280d / 720d;

        public static int ContentOverviewCollectionViewSnap
        {
            get
            {
                var aspectaRatio = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width / Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Height;

                if (aspectaRatio == 1) return 2;

                if (aspectaRatio == NTSC_ASPECT_RATIO) return 3;

                if (aspectaRatio == WIDESCREEN_ASPECT_RATIO_16X9) return 4;

                return  4;
            }
        }

        //public static double ContentOverviewLayoutRequestWidth
        //{
        //    get
        //    {
        //        var aspectaRatio = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width / Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Height;

        //        if (aspectaRatio == CUADRATE_ASPECT_RATIO) return Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width / 2;

        //        if (aspectaRatio == NTSC_ASPECT_RATIO) return Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width / 3;

        //        if (aspectaRatio == WIDESCREEN_ASPECT_RATIO_16X9) return Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width / 4;

        //        return Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width / 4;
        //    }
        //}
    }
}
