using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class CarParkAvailabilityService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl = "https://api.data.gov.sg/v1/transport/carpark-availability";

    public CarParkAvailabilityService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<JObject> GetCarParkAvailabilityAsync()
    {
        var response = await _httpClient.GetAsync(_apiUrl);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JObject.Parse(content);
    }
}
