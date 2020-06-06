using System;
namespace COVID19Tracker.Models
{
    public static class AppSession
    {
        public static string NewsAPIURL = "https://newsapi.org/v2/top-headlines?q=COVID&sortBy=publishedAt&apiKey=bd2e651da4cf40ecae5df0c03666498a&language=en&pageSize=20";
        public static string WorldStatsURL = "https://corona.lmao.ninja/v2/all";
        public static string CountryStatsURL = "https://corona.lmao.ninja/v2/countries?sort=cases";
    }
}
