﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyToDos.Model
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
        public float Temperature { get; protected set; }
        /// <summary>
        /// mm/h
        /// </summary>
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
