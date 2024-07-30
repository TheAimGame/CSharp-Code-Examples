using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSharp_Code_Examples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Print greeting and date
            run();

            // Fetch and print weather
            await FetchWeatherAsync();
        }

        // Prints a string and shows today's date in the console
        public static void run()
        {
            DateTime date = DateTime.Now;
            Console.WriteLine("Hello Lincoln!");
            Console.WriteLine("Today's date is " + date.ToLongDateString());
        }

        static async Task FetchWeatherAsync()
        {
            string apiKey = "92e5700f25a1a5b165c2090a20932c2e"; // Replace with your OpenWeatherMap API key
            string city = "Lincoln"; // Replace with the city you want to get the weather for

            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Console.WriteLine("Fetching weather data...");
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("Response received:");
                    Console.WriteLine(responseBody); // Print raw JSON response for debugging

                    var weatherData = JsonSerializer.Deserialize<WeatherResponse>(responseBody);

                    if (weatherData?.Main != null && weatherData.Weather != null && weatherData.Weather.Length > 0)
                    {
                        Console.WriteLine($"Weather in {city}:");
                        Console.WriteLine($"Temperature: {weatherData.Main.Temp}°C");
                        Console.WriteLine($"Weather: {weatherData.Weather[0].Description}");
                    }
                    else
                    {
                        Console.WriteLine("Failed to fetch weather data.");
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
                catch (JsonException e)
                {
                    Console.WriteLine("\nJSON Deserialization Exception Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nGeneral Exception Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
        }
    }

    public class WeatherResponse
    {
        public Main? Main { get; set; }
        public Weather[]? Weather { get; set; }
    }

    public class Main
    {
        public float Temp { get; set; }
    }

    public class Weather
    {
        public string? Description { get; set; }
    }
}
    