using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Storage.Model
{
    //public enum WeatherInformationType
    //{
    //    Current,
    //    DailyForecast,
    //    HourlyForecast
    //}
    /// <summary>
    /// weather information at a specific time
    /// </summary>
    public class WeatherInformation
    {
        public WeatherInformation(){}
        /// <summary>
        /// Celsius Degree
        /// </summary>
        public float Temperature { get; internal set; }
        /// <summary>
        /// mm/h
        /// </summary>
        public bool IsRainy { get; internal set; }
        /// <summary>
        /// Persent
        /// </summary>
        public int CloudCover { get; internal set; }
        /// <summary>
        /// Name of weather
        /// </summary>
        public string Title { get; internal set; }
        //public string Description { get; internal set; }
        public string Icon { get; internal set; }
        /// <summary>
        /// time when the weather be like
        /// </summary>
        public DateTime Time { get; internal set; }
    }
}
