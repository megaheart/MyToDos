using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyToDos.Model
{
    public class WeatherInformation
    {
        /// <summary>
        /// Celsius Degree
        /// </summary>
        [JsonProperty()]
        public float Temperature { get; protected set; }
        /// <summary>
        /// Celsius Degree
        /// </summary>
        [JsonProperty()]
        public float MaxTemperature { get; protected set; }
        /// <summary>
        /// Celsius Degree
        /// </summary>
        [JsonProperty()]
        public float MinTemperature { get; protected set; }
        /// <summary>
        /// mm/h
        /// </summary>
        [JsonProperty()]
        public float Rain { get; protected set; }
        /// <summary>
        /// Persent
        /// </summary>
        [JsonProperty()]
        public int Clouds { get; protected set; }
        [JsonProperty()]
        public string Type { get; protected set; }
        [JsonProperty()]
        public string Description { get; protected set; }
        [JsonProperty()]
        public string Icon { get; protected set; }
    }
}
