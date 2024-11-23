using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Model
{
    public class Starship
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string CostInCredits { get; set; }
        public float Length { get; set; }
        [JsonProperty("max_atmosphering_speed")]
        public int MaxAtmospheringSpeed { get; set; }
        public int Crew { get; set; }
        public int Passengers { get; set; }
        [JsonProperty("cargo_capacity")]
        public string CargoCapacity { get; set; }
        public string Consumables { get; set; }
        [JsonProperty("hyperdrive_rating")]
        public string HyperdriveRating { get; set; }
        public string MGLT { get; set; }

        [JsonProperty("starship_class")]
        public string StarshipClass { get; set; }
        public List<string> Pilots { get; set; } = new List<string>();
        public List<string> Films { get; set; } = new List<string>();
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public string Url { get; set; }
    }
}

