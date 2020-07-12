using COVID19Tracker.Exceptions;
using COVID19Tracker.Helpers;
using COVID19Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace COVID19Tracker.ViewModels
{
    public class AllCountriesPageViewModel : ViewModelBase
    {
        private List<CountryStats> _allCountryStats;

        private List<CountryStats> _countryStats;
        public List<CountryStats> CountryStats
        {
            get => _countryStats;
            set => SetProperty(ref _countryStats, value);
        }

        private CountryStats _selectedCountry;
        public CountryStats SelectedCountry
        {
            get => _selectedCountry;
            set => SetProperty(ref _selectedCountry, value);
        }

        public Command GoBackCommand { get; set; }
        public Command<string> SearchCountryCommand { get; set; }

        private INavigation _navigation;

        public AllCountriesPageViewModel(INavigation navigation) : base()
        {
            _navigation = navigation;

            Title = "Countries Report";

            GoBackCommand = new Command(GoBack);
            SearchCountryCommand = new Command<string>(SearchCountry);
        }

        private async void GoBack()
        {
            await _navigation.PopAsync(true);
        }

        private async void SearchCountry(string query)
        {
            query = query?.Trim();

            if (!string.IsNullOrEmpty(query))
                CountryStats = _allCountryStats.Where(_c => _c.country.ToLower().Contains(query.ToLower())).ToList();
            else
                CountryStats = _allCountryStats;
        }

        public async Task LoadCountries()
        {
            this.LoadingText = "Loading Countries Data";

            this.IsBusy = true;
            this.DataLoaded = false;
            this.IsErrorState = false;
            this.ErrorMessage = "";
            ErrorImage = "";

            try
            {
                //Get Country Stats
                _allCountryStats = (await RestHelper.GetAsync<List<CountryStats>>(AppSession.CountryStatsURL)).OrderByDescending(_c => _c.cases).ToList();

                CountryStats = _allCountryStats;


                //Dat Load Completed
                this.DataLoaded = true;
            }
            catch (InternetConnectionException iex)
            {
                this.IsErrorState = true;
                this.ErrorMessage = "Slow or no internet connection." + Environment.NewLine + "Please check you internet connection and try again.";
                ErrorImage = "nointernet.png";
            }
            catch (Exception ex)
            {
                this.IsErrorState = true;
                this.ErrorMessage = "Something went wrong. Please try again later. If the problem persists, contact support at Apps@xgeno.com with the error message:" + Environment.NewLine + Environment.NewLine + ex.Message;
                ErrorImage = "error.png";
            }
            finally
            {
                this.LoadingText = "";
                this.IsBusy = false;
            }
        }
    }
}
