using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace dxclusive
{
    public partial class ClassPage4 : ContentPage
    {
        private readonly FirestoreService _firestoreService;
        private readonly string _selectedTerm;
        private readonly string _selectedClass;

        public ClassPage4(string selectedTerm, string selectedClass)
        {
            InitializeComponent();
            _firestoreService = new FirestoreService();
            _selectedTerm = selectedTerm;
            _selectedClass = selectedClass;
            LoadClassDetailsAsync();
        }

        private async void LoadClassDetailsAsync()
        {
            try
            {
                // Fetch the class details using GetClassDetailsAsync
                var classDetails = await _firestoreService.GetClassDetailsAsync(_selectedTerm, _selectedClass);

                // If class details exist, display them
                if (classDetails.Count() > 0)
                {
                    // You can add UI components dynamically based on the data
                    foreach (var detail in classDetails)
                    {
                        // Example: Display each detail as a label
                        var label = new Label
                        {
                            Text = $"{detail.Key}: {detail.Value}",
                            FontSize = 20,
                            TextColor = Colors.Black,
                            Padding = new Thickness(5)
                        };
                        DetailsContainer.Children.Add(label); // Add to the container
                    }
                }
                else
                {
                    await DisplayAlert("No Data", "No details found for this class.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load class details: {ex.Message}", "OK");
            }
        }

        // Event handler for the back button click
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to the previous page (or modify as needed)
            await Navigation.PopAsync();
        }
    }
}
