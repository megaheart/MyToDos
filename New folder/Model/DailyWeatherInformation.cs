using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    public class DailyWeatherInformation
    {
        public DailyWeatherInformation()
        {
            Day = new PartOfDayWeatherInformation();
            Night = new PartOfDayWeatherInformation();
            Sun = new SpaceObjectActivity();
            Moon = new SpaceObjectActivity();
        }
        public int MinimumPrecipitation => 16; // (mm/24h)
        /// <summary>
        /// Celsius Degree
        /// </summary>
        public float MaxTemperature { get; internal set; }
        /// <summary>
        /// Celsius Degree
        /// </summary>
        public float MinTemperature { get; internal set; }
        /// <summary>
        /// time when the weather be like
        /// </summary>
        public DateTime Time { get; internal set; }
        public PartOfDayWeatherInformation Day { get; private set; }
        public PartOfDayWeatherInformation Night { get; private set; }
        public class PartOfDayWeatherInformation
        {
            /// <summary>
            /// mm
            /// </summary>
            public float Rain { get; internal set; }
            /// <summary>
            /// Percent
            /// </summary>
            public int CloudCover { get; internal set; }
            /// <summary>
            /// Name of weather
            /// </summary>
            public string Title { get; internal set; }
            public string Description { get; internal set; }
            public string Icon { get; internal set; }
        }
        public SpaceObjectActivity Sun { get; private set; }
        public SpaceObjectActivity Moon { get; private set; }
        public class SpaceObjectActivity
        {
            public TimeSpan Rise { get; internal set; }
            public TimeSpan Set { get; internal set; }
        }
    }
}
