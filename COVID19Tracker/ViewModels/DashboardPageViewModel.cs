using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COVID19Tracker.Exceptions;
using COVID19Tracker.Helpers;
using COVID19Tracker.Models;

namespace COVID19Tracker.ViewModels
{
    public class DashboardPageViewModel : ViewModelBase
    {
        public List<News> _covidNews;
        public List<News> COVIDNews
        {
            get => _covidNews;
            set => SetProperty(ref _covidNews, value);
        }

        public DashboardPageViewModel()
        {
            Title = "COVID-19 Report";
        }

        public async Task LoadCOVIDData()
        {
            IsBusy = true;
            LoadingText = "Preparing Data";
            DataLoaded = false;
            IsErrorState = false;

            try
            {
                //Get News Information
                var newsWrapper = await RestHelper.GetAsync<NewsWrapper>(AppSession.NewsAPIURL);

                COVIDNews = newsWrapper.articles.Where(_n => !string.IsNullOrEmpty(_n.urlToImage)).ToList();

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
