using PM2P2T1.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PM2P2T1.Controller
{
    public class restAPI
    {
        public static string endpoint = "https://restcountries.com/v3.1/region/";

        public restAPI()
        {

        }

        public async Task<List<Paises>> GetPaises(string region)
        {
            try
            {
                List<Paises> countries = new List<Paises>();

                using (HttpClient client = new HttpClient())
                {
                    var resp = await client.GetAsync(endpoint + region);

                    if (resp.IsSuccessStatusCode)
                    {
                        var content = resp.Content.ReadAsStringAsync().Result;

                        JArray results = JArray.Parse(content);

                        Paises temp = null;

                        foreach (var item in results)
                        {
                            temp = new Paises();

                            var curTemp = new List<Paises.Currencies>();
                            foreach (var current in item["currencies"].Values())
                            {
                                try
                                {
                                    curTemp.Add(new Paises.Currencies()
                                    {
                                        name = current["name"].ToString(),
                                        symbol = current["symbol"].ToString()
                                    });
                                }
                                catch
                                {
                                    curTemp.Add(new Paises.Currencies()
                                    {
                                        name = current["name"].ToString(),
                                        symbol = "--"
                                    });
                                }
                            }

                            var languTemp = new List<string>();
                            foreach (var current in item["languages"].Values())
                                languTemp.Add(current.ToString());

                            var timeTemp = new List<string>();
                            foreach (var current in item["timezones"].Values())
                                timeTemp.Add(current.ToString());

                            var contTemp = new List<string>();
                            foreach (var current in item["continents"].Values())
                            {
                                contTemp.Add(current.ToString());
                            }

                            temp.NameCountry = new Paises.Name()
                            {
                                common = item["name"]["common"].ToString(),
                                official = item["name"]["official"].ToString()

                            };

                            try
                            {
                                temp.independent = item["independent"].ToString(); // EUROPA
                            }
                            catch
                            {
                                temp.independent = "--";
                            }

                            temp.status = item["status"].ToString();
                            temp.currencies = curTemp;

                            try
                            {
                                temp.capital = item["capital"][0].ToString(); // ASIA
                            }
                            catch
                            {
                                temp.capital = "--";
                            }

                            temp.region = item["region"].ToString();
                            temp.subregion = item["subregion"].ToString();
                            temp.languages = languTemp;
                            temp.latlng = new List<double>() { (double)item["latlng"][0], (double)item["latlng"][1] };
                            temp.flag = item["flag"].ToString();
                            temp.maps = new Paises.Maps()
                            {
                                googleMaps = item["maps"]["googleMaps"].ToString(),
                                openStreetMaps = item["maps"]["openStreetMaps"].ToString()
                            };

                            temp.population = (int)item["population"];
                            temp.timezones = timeTemp;
                            temp.continents = contTemp;

                            temp.flags = new Paises.Flags()
                            {
                                png = item["flags"]["png"].ToString(),
                                svg = item["flags"]["svg"].ToString()
                            };

                            temp.startOfWeek = item["startOfWeek"].ToString();

                            try
                            {
                                temp.idd = new Paises.Idd()
                                {
                                    root = item["idd"]["root"].ToString(),
                                    suffixes = item["idd"]["suffixes"][0].ToString()
                                };
                            }
                            catch
                            {
                                temp.idd = new Paises.Idd()
                                {
                                    root = "-",
                                    suffixes = "---"
                                };

                            }
                            countries.Add(temp);
                        }
                    }

                }
                return countries.OrderBy(x => x.NameCountry.common).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return null;
            }

        }
    }
}
