﻿using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace dxclusive
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Set the starting page with navigation
            MainPage = new NavigationPage(new MainPage());
        }
    }
}