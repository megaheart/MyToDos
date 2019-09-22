using Storage;
using System;
using MyToDos.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.ViewModel.AppServices
{
    class WeatherUpdate
    {
        public static async void Start(AppServiceRunArgs args)
        {
            var dailyWeathers = DataManager.Current.DailyWeathers;
            if(dailyWeathers.Count == 0 || dailyWeathers[0].Time.Date < DateTime.Now.Date)
            {
                while (true)
                {
                    var client = App.HttpClient;
                    client.DefaultRequestHeaders.Clear();
                    using (var response = await client.GetAsync(@"http://dataservice.accuweather.com/currentconditions/v1/356187?apikey=R6Ky9UrLdErKo8LBtpyyGU7Ifx2e9M4M&language=vi-vn&details=true"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string json = await response.Content.ReadAsStringAsync();
                            await DataManager.Current.UpdateDailyWeathersAsync(json);
                            break;
                        }
                    }
                    await Task.Delay(15000);
                }
            }
            args.Sender.ExecuteOncePerDay -= Start;
            args.Sender.ExecuteOncePerDay += Start;
        }
    }
}
