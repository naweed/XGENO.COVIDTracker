using System;
namespace COVID19Tracker.Models
{
    public class WorldStats
    {
        public int cases { get; set; }
        public int todayCases { get; set; }
        public int deaths { get; set; }
        public int todayDeaths { get; set; }
        public int recovered { get; set; }
        public int active { get; set; }
        public int critical { get; set; }

        public float Active_Percentage
        {
            get
            {
                return Convert.ToSingle(active) / Convert.ToSingle(cases);
            }
        }

        public float Recovered_Percentage
        {
            get
            {
                return Convert.ToSingle(recovered) / Convert.ToSingle(cases);
            }
        }

        public float Deaths_Percentage
        {
            get
            {
                return Convert.ToSingle(deaths) / Convert.ToSingle(cases);
            }
        }
    }
}
