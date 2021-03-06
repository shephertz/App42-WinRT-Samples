﻿using com.shephertz.app42.paas.sdk.windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App42_WNS_Sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        AppHelper helper = null;

        public MainPage()
        {
            this.InitializeComponent();
            DISPATCHER.Initialize();
            helper = new AppHelper();
            ShowLoadingBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CreateOrUpdateChannel();
        }

        public void HideLoadingBar()
        {
            LoadingBar.Visibility = Visibility.Collapsed;
        }

        public void ShowLoadingBar()
        {
            LoadingBar.Visibility = Visibility.Visible;
        }


        private void CreateOrUpdateChannel()
        {
            helper.CreateOrUpdateChannelUri();
        }

        private void SendToast_Click(object sender, RoutedEventArgs e)
        {
            ShowLoadingBar();
            helper.SendSimpleToast();
        }

        private void SendToastWithParams_Click(object sender, RoutedEventArgs e)
        {
            ShowLoadingBar();
            helper.SendToastWithParams();
        }

        private void SendTile_Click(object sender, RoutedEventArgs e)
        {
            ShowLoadingBar();
            helper.SendSimpleTile();
        }

        private void SendTileWithImage_Click(object sender, RoutedEventArgs e)
        {
            ShowLoadingBar();
            helper.SendTileWithImage();
        }

        /// <summary>
        /// Used to display messages to the user
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="type"></param>
        public void NotifyUser(string strMessage, NotifyType type)
        {
            switch (type)
            {
                case NotifyType.StatusMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                    break;
                case NotifyType.ErrorMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                    break;
            }
            StatusBlock.Text = strMessage;

            // Collapse the StatusBlock if it has no text to conserve real estate.
            if (StatusBlock.Text != String.Empty)
            {
                StatusBorder.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                StatusBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

    }
}
