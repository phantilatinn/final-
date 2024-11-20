﻿﻿using Microsoft.Maui.Controls;

namespace dxclusive
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }


        private async void OnStartButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the next page
            await Navigation.PushAsync(new ClassPage());
        }
    }
}
