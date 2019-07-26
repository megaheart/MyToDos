using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storage.Model;
using Newtonsoft.Json.Linq;

namespace Storage
{
    internal static class WeatherInfoConverter
    {
        public static DailyWeatherInformation GetDailyWeatherInformation(JToken jObject)
        {
            DailyWeatherInformation daily = new DailyWeatherInformation();
            daily.Time = jObject.Value<DateTime>("Date");
            JToken temperature = jObject.Value<JToken>("Temperature");
            daily.MaxTemperature = temperature.Value<JToken>("Minimum").Value<float>("Value");
            daily.MaxTemperature = temperature.Value<JToken>("Maximum").Value<float>("Value");
            JToken day = jObject.Value<JToken>("Day");
            daily.Day.Icon = day.Value<string>("Icon");
            daily.Day.Description = day.Value<string>("LongPhrase");
            daily.Day.Title = day.Value<string>("IconPhrase");
            daily.Day.Rain = day.Value<JToken>("IconPhrase").Value<float>("Value");
            daily.Day.CloudCover = day.Value<int>("CloudCover");
            JToken night = jObject.Value<JToken>("Night");
            daily.Night.Icon = night.Value<string>("Icon");
            daily.Night.Description = night.Value<string>("LongPhrase");
            daily.Night.Title = night.Value<string>("IconPhrase");
            daily.Night.Rain = night.Value<JToken>("IconPhrase").Value<float>("Value");
            daily.Night.CloudCover = night.Value<int>("CloudCover");
            JToken sun = jObject.Value<JToken>("Sun");
            daily.Sun.Rise = sun.Value<DateTime>("Rise").TimeOfDay;
            daily.Sun.Set = sun.Value<DateTime>("Set").TimeOfDay;
            JToken moon = jObject.Value<JToken>("Moon");
            daily.Moon.Rise = moon.Value<DateTime>("Rise").TimeOfDay;
            daily.Moon.Set = moon.Value<DateTime>("Set").TimeOfDay;
            return daily;
        }
        public static WeatherInformation GetCurrentWeatherInformation(JToken jObject)
        {
            WeatherInformation weather = new WeatherInformation();
            weather.Time = jObject.Value<DateTime>("LocalObservationDateTime");
            weather.Icon = jObject.Value<string>("WeatherIcon");
            weather.Title = jObject.Value<string>("WeatherText");
            weather.Temperature = jObject.Value<JToken>("Temperature").Value<JToken>("Metric").Value<float>("Value");
            weather.IsRainy = jObject.Value<bool>("HasPrecipitation");
            weather.CloudCover = jObject.Value<int>("CloudCover");
            return weather;
        }
    }
}
