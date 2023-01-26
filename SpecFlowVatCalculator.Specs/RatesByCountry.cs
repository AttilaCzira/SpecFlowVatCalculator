using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlowCalculator.Specs;

public sealed class RatesByCountry
{
    private static RatesByCountry? instance = null;
    private static readonly object padlock = new();
    private static readonly string fileName = "RatesByCountry.csv";
    private Dictionary<string, List<int>> _validCountriesWithRates;
    RatesByCountry() 
    {
        _validCountriesWithRates = LoadCountryList();
    }
    public static RatesByCountry Instance
    {
        get
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new RatesByCountry();
                    }
                }
            }
            return instance;
        }
    }
    private Dictionary<string, List<int>> LoadCountryList()
    {
        Dictionary<string, List<int>> countryList = new Dictionary<string, List<int>>();
        var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileName);
        if (File.Exists(filePath))
        {
            StreamReader reader = new(filePath);
            while(!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;
                var values = line.Split(',');
                string country = values[0];
                if (country.Length is 0) continue;
                List<int> rates = new();
                foreach (var value in values.Skip(1))
                {
                    if (int.TryParse(value, out int result))
                    {
                        rates.Add(result);
                    }
                }
                if (rates.Count is 0) continue;
                countryList.Add(country, rates);
            }
            reader.Close();
        }
        return countryList;
    }
    public List<int> ValidRates(string country)
    {
        if (_validCountriesWithRates.ContainsKey(country))
        {
            return _validCountriesWithRates[country];
        }
        throw new ArgumentOutOfRangeException($"{country} is not in the valid countries list!");
    }
}
