using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    public class WeatherInformation
    {
        public WeatherInformation()
        {
        }
        /// <summary>
        /// Celsius Degree
        /// </summary>
        public float Temperature { get; protected set; }
        /// <summary>
        /// Celsius Degree
        /// </summary>
        public float MaxTemperature { get; protected set; }
        /// <summary>
        /// Celsius Degree
        /// </summary>
        public float MinTemperature { get; protected set; }
        /// <summary>
        /// mm/h
        /// </summary>
        public float Rain { get; protected set; }
        /// <summary>
        /// Persent
        /// </summary>
        public int Clouds { get; protected set; }
        public string Type { get; protected set; }
        public string Description { get; protected set; }
        public string Icon { get; protected set; }
    }
}
