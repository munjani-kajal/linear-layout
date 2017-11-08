using System;
using Android.App;
using Android.OS;
using Android.Gms.Location;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Util;
using Android.Widget;
using Android.Locations;

namespace App5

{
    [Activity(Label = "App5", MainLauncher = true)]
    public class MainActivity : Activity, GoogleApiClient.IConnectionCallbacks,
        GoogleApiClient.IOnConnectionFailedListener, Android.Gms.Location.ILocationListener
    {
        GoogleApiClient apiClient;
        LocationRequest locRequest;
        Button button2;
        TextView latitude2;
        TextView longitude2;
        TextView provider2;

        ////Lifecycle methods

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // UI to print location updates
            button2 = FindViewById<Button>(Resource.Id.myButton2);
            latitude2 = FindViewById<TextView>(Resource.Id.latitude2);
            longitude2 = FindViewById<TextView>(Resource.Id.longitude2);
            provider2 = FindViewById<TextView>(Resource.Id.provider2);

            // pass in the Context, ConnectionListener and ConnectionFailedListener
            apiClient = new GoogleApiClient.Builder(this, this, this).AddApi(LocationServices.API).Build();

            // generate a location request that we will pass into a call for location updates
            locRequest = new LocationRequest();

        }

        protected override void OnResume()
        {
            base.OnResume();
            Log.Debug("OnResume", "OnResume called, connecting to client...");

            apiClient.Connect();

            // Clicking the second button will send a request for continuous updates
            button2.Click += async delegate {
                if (apiClient.IsConnected)
                {
                    button2.Text = "Requesting Location Updates";

                    // Setting location priority to PRIORITY_HIGH_ACCURACY (100)
                    locRequest.SetPriority(100);

                    // Setting interval between updates, in milliseconds
                    // NOTE: the default FastestInterval is 1 minute. If you want to receive location updates more than 
                    // once a minute, you _must_ also change the FastestInterval to be less than or equal to your Interval
                    locRequest.SetFastestInterval(500);
                    locRequest.SetInterval(1000);

                    // pass in a location request and LocationListener
                    await LocationServices.FusedLocationApi.RequestLocationUpdates(apiClient, locRequest, this);
                    // In OnLocationChanged (below), we will make calls to update the UI
                    // with the new location data
                }
                else
                {
                    Log.Info("LocationClient", "Please wait for Client to connect");
                }
            };
        }

        protected override async void OnPause()
        {
            base.OnPause();

            if (apiClient.IsConnected)
            {
                // stop location updates, passing in the LocationListener
                await LocationServices.FusedLocationApi.RemoveLocationUpdates(apiClient, this);

                apiClient.Disconnect();
            }
        }


        ////Interface methods

        public void OnConnected(Bundle bundle)
        {
            // This method is called when we connect to the LocationClient. We can start location updated directly form
            // here if desired, or we can do it in a lifecycle method, as shown above 

            // You must implement this to implement the IGooglePlayServicesClientConnectionCallbacks Interface

        }

        public void OnDisconnected()
        {
            // This method is called when we disconnect from the LocationClient.

            // You must implement this to implement the IGooglePlayServicesClientConnectionCallbacks Interface

        }

        public void OnConnectionFailed(ConnectionResult bundle)
        {
            // This method is used to handle connection issues with the Google Play Services Client (LocationClient). 
            // You can check if the connection has a resolution (bundle.HasResolution) and attempt to resolve it

            // You must implement this to implement the IGooglePlayServicesClientOnConnectionFailedListener Interface

        }

        public void OnLocationChanged(Location location)
        {
            // This method returns changes in the user's location if they've been requested

            // You must implement this to implement the Android.Gms.Locations.ILocationListener Interface
            Log.Debug("LocationClient", "Location updated");

            latitude2.Text = "Latitude: " + location.Latitude.ToString();
            longitude2.Text = "Longitude: " + location.Longitude.ToString();
            provider2.Text = "Provider: " + location.Provider.ToString();
        }

        public void OnConnectionSuspended(int i)
        {

        }
    }
}
