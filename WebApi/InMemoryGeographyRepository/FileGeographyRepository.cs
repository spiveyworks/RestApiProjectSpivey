using GeographyRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace FileGeographyRepository
{
    public class FileGeographyRepository : IGeographyRepository
    {
        private string _citiesCsvFilePath;
        private string _statesCsvFilePath;

        public FileGeographyRepository(string citiesCsvFilePath, string statesCsvFilePath)
        {
            this._citiesCsvFilePath = citiesCsvFilePath;
            this._statesCsvFilePath = statesCsvFilePath;
        }

        public async Task<IEnumerable<City>> GetCitiesAsync(string stateAbbreviation)
        {
            var cities = new List<City>();
            string[] lines = null;

            try
            {
                lines = System.IO.File.ReadAllLines(this._citiesCsvFilePath);

                //Remove the first line file header
                if (lines.Length > 0)
                    lines = lines.Skip(0).ToArray();
            }
            catch (Exception exc)
            {

            }

            var currentCityId = 1;

            if (lines != null && lines.Length > 0)
                foreach (var line in lines)
                {
                    try
                    {
                        var data = line.Split(',');
                        var city = new City()
                        {
                            CityId = currentCityId,
                            Name = data[0],
                            StateId = short.Parse(data[1])
                        };
                        cities.Add(city);
                        currentCityId += 1;
                    }
                    catch
                    {
                        //Swallow any problem with an individual record
                    }
                }

            return cities;
        }

        public async Task<IEnumerable<State>> GetStatesAsync()
        {
            var states = new List<State>();
            var lines = System.IO.File.ReadAllLines(this._statesCsvFilePath);
            short currentStateId = 1;

            if (lines != null && lines.Length > 0)
                foreach (var line in lines)
                {
                    var data = line.Split(',');
                    var state = new State()
                    {
                        StateId = currentStateId,
                        Name = data[0],
                        Abbreviation = data[1]
                    };
                    states.Add(state);
                    currentStateId += 1;
                }

            return states;
        }

        public async Task<bool> DoesStateExistAsync(string stateAbbreviation)
        {
            var stateAbbreviationUpper = stateAbbreviation.ToUpper();
            var states = await this.GetStatesAsync();
            return states.Where(state => state.Abbreviation.ToUpper() == stateAbbreviationUpper).FirstOrDefault() != null;
        }
    }
}
