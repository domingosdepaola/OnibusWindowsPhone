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
        
        private void AddPins(List<Onibus> lstOnibus)
        {
            foreach (Onibus item in lstOnibus)
            {
                MapIcon MapIcon1 = new MapIcon();
                MapIcon1.Location = new Geopoint(new BasicGeoposition()
                {
                    Latitude = item.Latitude,
                    Longitude = item.Longitude
                });
                MapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
                MapIcon1.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/busRed.png"));
                MapIcon1.Title = "Onibus " + item.Linha + " Numero: " + item.Ordem;

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
