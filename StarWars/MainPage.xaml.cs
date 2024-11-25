using Newtonsoft.Json;
using StarWars.Model;
using StarWars.Template;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
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

        private List<CharacterModel> result = [];

        public ObservableCollection<CharacterModel> CharacterModelsList { get; set; } = [];



        public MainPage()
        {
            this.InitializeComponent();

            Init();
        }

        public async void Init()
        {
            List<Character> characters = await GetCharactersAsync();

            List<CharacterModel> listCharcterModels = await SetListCharcterModels(characters); // slow


            listCharcterModels.ForEach(character =>
            {
                CharacterModelsList.Add(character);
            });


            MyListView.ItemsSource = CharacterModelsList;


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

        private async Task<List<CharacterModel>> SetListCharcterModels(List<Character> characters)
        {

            foreach (Character character in characters)
            {
                Planet homeworld = await GetAsync<Planet>(character.Homeworld);

                List<Starship> starships = [];
                List<Vehicle> vehicles = [];

                foreach (var urlGetStarship in character.Starships)
                {
                    Starship starship = await GetAsync<Starship>(urlGetStarship);
                    starships.Add(starship);
                }

                foreach (var urlGetVehicle in character.Vehicles)
                {
                    Vehicle vehicle = await GetAsync<Vehicle>(urlGetVehicle);
                    vehicles.Add(vehicle);
                }

                CharacterModel characterModel = new CharacterModel
                {
                    Character = character,
                    Planet = homeworld,
                    Starships = starships,
                    Vehicles = vehicles
                };

                result.Add(characterModel);
            }

            return result;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string word = SearchTextBox.Text;
            CharacterModelsList.Clear();

            result.ForEach(ch =>
            {
                if (ch.Character.Name.Contains(word, StringComparison.OrdinalIgnoreCase) ||
                    ch.Planet.Name.Contains(word, StringComparison.OrdinalIgnoreCase))
                {
                    CharacterModelsList.Add(ch);
                }
            });

            // MyListView.ItemsSource = CharacterModelsList;

        }

        private void MyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyListView.SelectedItem is CharacterModel selectedCharacter)
            {
                LocalProfileController.DataContext = selectedCharacter;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            result.ForEach(ch =>
            {
                CharacterModelsList.Add(ch);
            });
            MyListView.SelectedItem = CharacterModelsList;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtBlcokMessage.Text = "";

            string word = SearchTextBox.Text;
            if (string.IsNullOrEmpty(word.Trim()))
            {
                result.ForEach(ch =>
                {
                    CharacterModelsList.Add(ch);
                });
                MyListView.SelectedItem = CharacterModelsList;
                return;
            }
            CharacterModelsList.Clear();

            result.ForEach(ch =>
            {
                if (ch.Character.Name.Contains(word, StringComparison.OrdinalIgnoreCase) ||
                    ch.Planet.Name.Contains(word, StringComparison.OrdinalIgnoreCase))
                {
                    CharacterModelsList.Add(ch);
                }
            });

        }
        private void SalvaListaCharctersJson()
        {
            try
            {
                string NAME_FILES = "characters.json";
                StorageFile fileJson = ApplicationData.Current.LocalFolder.CreateFileAsync(NAME_FILES, CreationCollisionOption.OpenIfExists).GetAwaiter().GetResult();

                string json = FileIO.ReadTextAsync(fileJson).GetAwaiter().GetResult();

                List<CharacterModel> characterModels = [.. CharacterModelsList];


                string jsonPersone = JsonConvert.SerializeObject(characterModels);

                FileIO.WriteTextAsync(fileJson, jsonPersone).GetAwaiter().GetResult();

                txtBlcokMessage.Text = $"Salvataggio completato nel percorso: \n{fileJson.Path}";
                txtBlcokMessage.Foreground = new SolidColorBrush(Colors.White);

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Errore nel salvataggio del file: {e.Message}");
                txtBlcokMessage.Text = "Errore nel salvataggio del file. Riprovare.";
                txtBlcokMessage.Foreground = new SolidColorBrush(Colors.Red);
            }


        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SalvaListaCharctersJson();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            txtBlcokMessage.Text = "Attenzione: Ancora da implementare...";
        }
    }



}
