using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TILMultiApp.Droid
{
    [Activity(Label = "Did You Know?", Icon ="@drawable/icon_head",
              Theme = "@style/SplashTheme", 
              MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | 
              ConfigChanges.Orientation,
              ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.Window.RequestFeature(WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(bundle);

            if (!IsTaskRoot)
            {
                Finish();
                return;
            }

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

