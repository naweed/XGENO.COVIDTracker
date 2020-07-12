using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using COVID19Tracker.Exceptions;
using COVID19Tracker.Helpers;
using COVID19Tracker.Models;
using COVID19Tracker.Views;
using Microcharts;
using SkiaSharp;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace COVID19Tracker.ViewModels
{
    public class DashboardPageViewModel : ViewModelBase
    {
        private List<News> _covidNews;
        public List<News> CovidNews
        {
            get => _covidNews;
            set => SetProperty(ref _covidNews, value);
        }

        private WorldStats _worldStats;
        public WorldStats WorldStats
        {
            get => _worldStats;
            set => SetProperty(ref _worldStats, value);
        }

        private List<CountryStats> _countryStats;
        public List<CountryStats> CountryStats
        {
            get => _countryStats;
            set => SetProperty(ref _countryStats, value);
        }

        private Chart _caseStatsChart;
        public Chart CaseStatsChart
        {
            get => _caseStatsChart;
            set => SetProperty(ref _caseStatsChart, value);
        }

        public Command<News> NavigateToNewsDetailsCommand { get; set; }
        public Command AllCountriesPageCommand { get; set; }

        private INavigation _navigation;

        public DashboardPageViewModel(INavigation navigation)
        {
            Title = "COVID-19 Report";

            _navigation = navigation;

            NavigateToNewsDetailsCommand = new Command<News>(NavigateToNewsPage);
            AllCountriesPageCommand = new Command(NavigateToAllCountriesPage);
        }

        private async void NavigateToAllCountriesPage()
        {
            await _navigation.PushAsync(new AllCountriesPage());
        }

        private async void NavigateToNewsPage(News newsItem)
        {
            await Browser.OpenAsync(newsItem.url, new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show
            });
        }

        public async Task LoadCOVIDData()
        {
            IsBusy = true;            
            DataLoaded = false;
            IsErrorState = false;

            try
            {
                //Get News Information
                LoadingText = "Fetching lastest world news";
                var newsWrapper = await RestHelper.GetAsync<NewsWrapper>(AppSession.NewsAPIURL);

                CovidNews = newsWrapper.articles.Where(_n => !string.IsNullOrEmpty(_n.urlToImage)).ToList();

                //Get the World Stats Information
                LoadingText = "Getting total number of COVID cases";
                WorldStats = await RestHelper.GetAsync<WorldStats>(AppSession.WorldStatsURL);

                CaseStatsChart = new DonutChart()
                {
                    Entries = new[]
                        {
                            new ChartEntry(WorldStats.Active_Percentage)
                            {
                                Color = SKColor.Parse("#003CBF"),
                                TextColor = new SKColor(0,0,0,0),
                                Label = "Active",
                                ValueLabel = WorldStats.Active_Percentage.ToString("0.0%")
                            },
                            new ChartEntry(WorldStats.Recovered_Percentage)
                            {
                                Color = SKColor.Parse("#EBB335"),
                                TextColor = new SKColor(0,0,0,0),
                                Label = "Recovered",
                                ValueLabel = WorldStats.Recovered_Percentage.ToString("0.0%")
                            },
                            new ChartEntry(WorldStats.Deaths_Percentage)
                            {
                                Color = SKColor.Parse("#FF5C4D"),
                                TextColor = new SKColor(0,0,0,0),
                                Label = "Deaths",
                                ValueLabel = WorldStats.Deaths_Percentage.ToString("0.0%")
                            }
                        },
                    LabelTextSize = 0,
                    HoleRadius = 0.65f
                };

                //Get Country Stats
                LoadingText = "Getting cases for top 5 countries";
                var countryStats = (await RestHelper.GetAsync<List<CountryStats>>(AppSession.CountryStatsURL)).
                                            OrderByDescending(_c => _c.cases).
                                            Take(5).
                                            ToList();

                countryStats.ForEach(_c =>
                    _c.Percentage_of_Global = Convert.ToSingle(_c.cases) / Convert.ToSingle(WorldStats.cases));

                CountryStats = countryStats;

                DataLoaded = true;
            }
            catch(InternetConnectionException iex)
            {
                IsErrorState = true;
                ErrorMessage = "Slow or no internet connection." + Environment.NewLine + "Please check you connection and try again later.";
                ErrorImage = "nointernet.png";
            }
            catch (Exception ex)
            {
                IsErrorState = true;
                ErrorMessage = "Something went wrong. Please try again later. If the error persists, contact support team at xyz@test.com with error message: " + ex.Message;
                ErrorImage = "error.png";
            }
            finally
            {
                IsBusy = false;
                LoadingText = "";
            }

            
        }
    }
}
