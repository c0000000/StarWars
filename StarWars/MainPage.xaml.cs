using Newtonsoft.Json;
using StarWars.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StarWars
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private HttpClient _httpClient = new();
        public MainPage()
        {
            this.InitializeComponent();
            Init();
        }

        public async void Init()
        {
            List<Character> characters = await GetCharactersAsync();

            List<CharcterModel> listCharcterModels = await SetListCharcterModels(characters);

            MyListView.ItemsSource = listCharcterModels;

            
            TxtLoading.Visibility = Visibility.Collapsed;
        }
        public async Task<List<Character>> GetCharactersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://swapi.dev/api/people");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                ResponseCharacter responseCharacter = JsonConvert.DeserializeObject<ResponseCharacter>(jsonResponse);

                return responseCharacter?.Results;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Errore durante la chiamata a Get People: " + ex.Message, ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Errore nella deserializzazione della risposta: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Errore imprevisto: " + ex.Message, ex);
            }

        }

        public async Task<T> GetAsync<T>(string urlGet)
        {
            try
            {
                var response = await _httpClient.GetAsync(urlGet);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(jsonResponse);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Errore durante la chiamata: " + $"\nGET - {urlGet} | " + ex.Message, ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Errore nella deserializzazione della risposta: " + $"\nGET - {urlGet} |" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Errore imprevisto: " + ex.Message, ex);
            }
        }

        private async Task<List<CharcterModel>> SetListCharcterModels(List<Character> characters)
        {
            List<CharcterModel> result = [];

            foreach (Character character in characters)
            {

                Planet homeworld = await GetAsync<Planet>(character.Homeworld);

                List<Starship> starships = [];

                character.Starships.ForEach(async urlGetStarship =>
                {
                    Starship starship = await GetAsync<Starship>(urlGetStarship);

                    starships.Add(starship);
                });

                List<Vehicle> vehicles = new List<Vehicle>();
                character.Vehicles.ForEach(async urlGetVehicless =>
                {
                    Vehicle vehicle = await GetAsync<Vehicle>(urlGetVehicless);
                    vehicles.Add(vehicle);

                });
                CharcterModel charcterModel = new();

                charcterModel.Character = character;
                charcterModel.Planet = homeworld;


                charcterModel.Starships = starships;
                charcterModel.Vehicles = vehicles;

                result.Add(charcterModel);
            }

            return result;
        }
    }
}
