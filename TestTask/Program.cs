using System;
using System.Collections.Generic;
using System.Net.Http;
using Npgsql;
using System.Configuration;

namespace WeatherApp
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            string apiKey = ConfigurationManager.AppSettings["api_key"];
            int delay = Convert.ToInt32(ConfigurationManager.AppSettings["delay"]);
            TimeSpan time = TimeSpan.FromMilliseconds(delay);
            string updateTime = time.ToString(@"hh\:mm\:ss");
            

            string latitude = ConfigurationManager.AppSettings["latitude"];
            string longitude = ConfigurationManager.AppSettings["longitude"];

            string apiUrl = $"https://api.weather.yandex.ru/v2/forecast?lat={latitude}&lon={longitude}";

            while (true)
            { 
                using (var client = new HttpClient())
                {
                    DateTime dataNow = DateTime.Now;
                    client.DefaultRequestHeaders.Add("X-Yandex-API-Key", apiKey);
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
                        double temperature = data.fact.temp;
                        double wind_speed = data.fact.wind_speed;
                        double humidity = data.fact.humidity;



                        // Подключение к базе данных PostgreSQL
                        string db_user = ConfigurationManager.AppSettings["db_user"];
                        string db_password = ConfigurationManager.AppSettings["db_password"];
                        string db = ConfigurationManager.AppSettings["db"];
                        string connectionString = $"Host=localhost;Username={db_user};Password={db_password};Database={db}";
                        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                        {
                            connection.Open();

                            // Создание таблицы, если она не существует
                            string createTableQuery = "CREATE TABLE IF NOT EXISTS weather (temperature VARCHAR(10)," +
                                " timestamp TIMESTAMP, wind_speed VARCHAR(10), humidity VARCHAR(10))";
                            using (NpgsqlCommand command = new NpgsqlCommand(createTableQuery, connection))
                            {
                                command.ExecuteNonQuery();
                            }

                            // Вставка данных в таблицу
                            string insertQuery = "INSERT INTO weather (temperature, wind_speed, humidity, timestamp)" +
                                " VALUES (@temperature, @wind_speed, @humidity, @timestamp)";
                            using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection))
                            {
                                command.Parameters.AddWithValue("@temperature", temperature);
                                command.Parameters.AddWithValue("@wind_speed", wind_speed);
                                command.Parameters.AddWithValue("@humidity", humidity);
                                command.Parameters.AddWithValue("@timestamp", dataNow);
                                command.ExecuteNonQuery();
                            }


                        }
                        Console.WriteLine($"Температура: {temperature} °C. Скорость ветра: {wind_speed} м/с. Влажность: {humidity}%");
                        Console.WriteLine($"Следующее обновление информации о погоде будет в {dataNow.Add(time)}\n");
                        Thread.Sleep(60000);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to retrieve weather information. Status code: {response.StatusCode}");
                    }
                }
            }
        }
    }
}
