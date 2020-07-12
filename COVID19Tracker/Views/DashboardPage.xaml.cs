using System;
using System.Collections.Generic;
using COVID19Tracker.ViewModels;
using Xamarin.Forms;

namespace COVID19Tracker.Views
{

    public partial class DashboardPage : ContentPage
    {
        DashboardPageViewModel viewModel;
        bool isAlreadyLoaded = false;

        public DashboardPage()
        {
            InitializeComponent();

            viewModel = new DashboardPageViewModel(Navigation);                       

            this.BindingContext = viewModel;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!isAlreadyLoaded)
            {
                await viewModel.LoadCOVIDData();

                isAlreadyLoaded = true;
            }
        }
    }
}
