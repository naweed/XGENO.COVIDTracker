using COVID19Tracker.Models;
using COVID19Tracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COVID19Tracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllCountriesPage : ContentPage
    {
        AllCountriesPageViewModel viewModel;


        public AllCountriesPage()
        {
            InitializeComponent();

            viewModel = new AllCountriesPageViewModel(Navigation);
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await viewModel.LoadCountries();
        }

        void searchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            viewModel.SearchCountryCommand.Execute(e.NewTextValue);
        }

        async void CollectionView_SelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
            var collectionView = sender as CollectionView;
            viewModel.SelectedCountry = collectionView.SelectedItem as CountryStats;

            //Show the Panel with more details
            OpenPopupPane(CountryDetailsGrid);

            await Task.CompletedTask;

        }

        private void OpenPopupPane(View theView)
        {
            PageFader.IsVisible = true;
            PageFader.FadeTo(1, 300, Easing.SinInOut);
            theView.TranslateTo(0, 200, 300, Easing.SinInOut);

            AnimateClosePopupButton(ClosePopupPaneButton, entering: true);
        }

        private void AnimateClosePopupButton(VisualElement elementToTransform, bool entering)
        {
            var startingTranslation = entering ? 30 : 140;
            var endingTranslation = entering ? 140 : 30;
            var translationEasing = entering ? Easing.SpringOut : Easing.SinIn;

            var startingOpacity = entering ? 0 : 1;
            var endingOpacity = entering ? 1 : 0;

            var startingRotation = entering ? -90 : 0;
            var endingRotation = entering ? 0 : 180;

            elementToTransform.TranslationY = startingTranslation;
            elementToTransform.Opacity = startingOpacity;
            elementToTransform.Rotation = startingRotation;

            elementToTransform.FadeTo(endingOpacity, 500);
            elementToTransform.RotateTo(endingRotation, 700, Easing.SinInOut);
            elementToTransform.TranslateTo(0, endingTranslation, 600, translationEasing);
        }

        private async void ClosePopupPaneButton_Tapped(object sender, EventArgs e)
        {
            await ClosePopupPane();
        }


        private async Task ClosePopupPane()
        {
            AnimateClosePopupButton(ClosePopupPaneButton, entering: false);

            _ = CountryDetailsGrid.TranslateTo(0, Height, 300, Easing.SinInOut);


            await PageFader.FadeTo(0, 300, Easing.SinInOut);
            PageFader.IsVisible = false;
        }
    }


}