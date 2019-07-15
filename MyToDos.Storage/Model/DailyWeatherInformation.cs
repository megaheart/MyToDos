using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    class DailyWeatherInformation
    {

        /// <summary>
        /// Celsius Degree
        /// </summary>
        public float MaxTemperature { get; protected set; }
        /// <summary>
        /// Celsius Degree
        /// </summary>
        public float MinTemperature { get; protected set; }
        public float Rain { get; protected set; }
        /// <summary>
        /// Persent
        /// </summary>
        public int Clouds { get; protected set; }
        /// <summary>
        /// Name of weather
        /// </summary>
        public string Type { get; protected set; }
        public string Description { get; protected set; }
        public string Icon { get; protected set; }
        /// <summary>
        /// time when the weather be like
        /// </summary>
        public DateTime Time { get; protected set; }
    }
}
