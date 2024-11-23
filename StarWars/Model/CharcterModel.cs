using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Model
{
    internal class CharcterModel
    {
        public Character Character { get; set; }
        public Planet Planet { get; set; }
        public List<Vehicle> Vehicles { get; set; } = [];
        public List<Starship> Starships { get; set; } = [];


    }
}
