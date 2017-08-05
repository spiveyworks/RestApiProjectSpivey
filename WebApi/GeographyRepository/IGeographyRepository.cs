using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeographyRepository
{
    public interface IGeographyRepository
    {
        Task<bool> DoesStateExistAsync(string stateAbbreviation);
        Task<IEnumerable<State>> GetStatesAsync();
        Task<IEnumerable<City>> GetCitiesAsync(string stateAbbreviation);
        Task<City> GetCityAsync(int cityId);
        Task<State> GetStateAsync(short stateId);
    }
}
