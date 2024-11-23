using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Model
{

    public class Vehicle
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        [JsonProperty("cost_in_credits")]
        public string CostInCredits { get; set; }
        public float Length { get; set; }
        public int MaxAtmospheringSpeed { get; set; }
        public int Crew { get; set; }
        public int Passengers { get; set; }
        [JsonProperty("cargo_capacity")]
        public string CargoCapacity { get; set; }
        public string Consumables { get; set; }

        [JsonProperty("vehicle_class")]
        public string VehicleClass { get; set; }
        public List<string> Pilots { get; set; } = new List<string>();
        public List<string> Films { get; set; } = new List<string>();
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public string Url { get; set; }
    }

    public class ResponseVehicle
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public object Previous { get; set; }
        public List<Vehicle> Results { get; set; } = new List<Vehicle>();
    }
}


