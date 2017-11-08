
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Locations;
using Android.Runtime;
using System;
using Android.Util;
using Android;
using Android.Content;

namespace App2
{
    [Activity(Label = "App2", MainLauncher = true, Icon = "@drawable/icon")]

    //Implement IlocationaListener interface to get location updates
    public class MainActivity : Activity, ILocationListener
    {
        LocationManager locMgr;
        string tag = "Mainactivity";
        Button button;
        TextView latitude;
        TextView longitude;
        TextView provider;


        public void OnLocationChanged(Location location)
        {
            latitude.Text = "Latitude: " + location.Latitude.ToString();
            longitude.Text = "Longitude" + location.Longitude.ToString();
            provider.Text = "Provider" + location.Provider.ToString();
        }

        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            button = FindViewById<Button>(Resource.Id.button1);
            latitude = FindViewById<TextView>(Resource.Id.latitude);
            longitude = FindViewById<TextView>(Resource.Id.longitude);
            provider = FindViewById<TextView>(Resource.Id.provider);


        }
        protected override void OnResume()
        {
            base.OnResume();

            //initialize location manager
            locMgr = GetSystemService(Context.LocationService) as LocationManager;

            button.Click += delegate
            {
                button.Text = "Location Service Running";

                //GPS provider
                string Provider = LocationManager.GpsProvider;
                if (locMgr.IsProviderEnabled(Provider))
                {
                    locMgr.RequestLocationUpdates(Provider, 2000, 1, this);
                }
                else
                {
                    Log.Info(tag, Provider + " is not available.");
                }
            };
        }
        protected override void OnPause()
        {
            base.OnPause();

            locMgr.RemoveUpdates(this);
        }
        protected override void OnStart()
        {
            base.OnStart();
        }
        protected override void OnStop()
        {
            base.OnStop();
        }
    }
}
