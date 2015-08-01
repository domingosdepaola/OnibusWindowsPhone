﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace OnibusWPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            myMap.Loaded += myMap_Loaded;
            PinMyLocation();
        }

        void myMap_Loaded(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string numeroLinha = txtNumero.Text;
            var task = RequestAPI.GetString("http://192.168.0.12/OnibusAPI/api/onibus/getOnibusOpen?numeroLinha=" + numeroLinha);
            var items = await task;
            List<Onibus> lstOnibus = (List<Onibus>)items;
            AddPins(lstOnibus);
        }
        private async void PinMyLocation() 
        {
            var geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 100;
            Geoposition position = await geolocator.GetGeopositionAsync();

            // reverse geocoding
            BasicGeoposition myLocation = new BasicGeoposition
            {
                Longitude = position.Coordinate.Longitude,
                Latitude = position.Coordinate.Latitude
            };

            MapIcon MapIcon1 = new MapIcon();
            MapIcon1.Location = new Geopoint(new BasicGeoposition()
            {
                Latitude = myLocation.Latitude,
                Longitude = myLocation.Longitude
            });
            MapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
            MapIcon1.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/voce.png"));
            MapIcon1.Title = "VOCE ESTA AQUI";

            myMap.MapElements.Add(MapIcon1);
            myMap.Center = new Geopoint(new BasicGeoposition()
            {
                Latitude = myLocation.Latitude,
                Longitude = myLocation.Longitude
            });
            myMap.ZoomLevel = 15;
            Geopoint pointToReverseGeocode = new Geopoint(myLocation);

            MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);

            // here also it should be checked if there result isn't null and what to do in such a case
            string country = result.Locations[0].Address.Country;
        }
        private async void AddPins(List<Onibus> lstOnibus)
        {
            foreach (Onibus item in lstOnibus)
            {
                MapIcon MapIcon1 = new MapIcon();
                BasicGeoposition position = new BasicGeoposition()
                {
                    Latitude = item.Latitude,
                    Longitude = item.Longitude
                };
                MapIcon1.Location = new Geopoint(position);
                MapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
                MapIcon1.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/busRed.png"));
                try
                {
                    Geopoint pointToReverseGeocode = new Geopoint(position);

                    MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);

                    MapIcon1.Title = "Onibus " + item.Linha + " Numero: " + item.Ordem + " Localizacao : " + result.Locations[0].Address.Street + " " + result.Locations[0].Address.StreetNumber + " " + result.Locations[0].Address.Neighborhood + " " + result.Locations[0].Address.Region;
                }
                catch { MapIcon1.Title = "Onibus " + item.Linha + " Numero: " + item.Ordem; }
                

                myMap.MapElements.Add(MapIcon1);
                myMap.Center = new Geopoint(new BasicGeoposition()
                {
                    Latitude = item.Latitude,
                    Longitude = item.Longitude
                });
            }
            myMap.ZoomLevel = 15;
        }
    }
}
