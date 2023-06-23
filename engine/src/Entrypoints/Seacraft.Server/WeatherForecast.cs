// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'WeatherForecast.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

namespace Seacraft.Server
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}