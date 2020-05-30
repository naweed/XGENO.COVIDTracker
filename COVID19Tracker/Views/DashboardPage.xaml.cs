using System;
using System.Collections.Generic;
using COVID19Tracker.ViewModels;
using Xamarin.Forms;

namespace COVID19Tracker.Views
{   

    public partial class DashboardPage : ContentPage
    {
        DashboardPageViewModel viewModel;

        public DashboardPage()
        {
            InitializeComponent();

            viewModel = new DashboardPageViewModel();                       

            this.BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await viewModel.LoadCOVIDData();
        }
    }
}
