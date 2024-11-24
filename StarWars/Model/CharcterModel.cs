using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Model
{
    public class CharacterModel
    {
        public Character Character { get; set; }
        public Planet Planet { get; set; }
        public List<Vehicle> Vehicles { get; set; } = [];
        public List<Starship> Starships { get; set; } = [];

        public int CountStarships { get => Starships.Count; }
        public int CountVehicles { get => Vehicles.Count; }

    }
}
