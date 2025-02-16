using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class PiShockAPI
{
    private const string ApiBaseUrl = "https://do.pishock.com/api/";
    private string Username;
    private string Apikey;
    private string Code;
    private string Name;

    public PiShockAPI(string username, string apikey, string code, string name)
    {
        Username = username;
        Apikey = apikey;
        Code = code;
        Name = name;
    }

    private async Task<string> SendPostRequest(string endpoint, string jsonData)
    {
        using (HttpClient client = new HttpClient())
        {
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Send the POST request and get the response
            var response = await client.PostAsync(ApiBaseUrl + endpoint, content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
    }


    public async Task<string> ShockAsync(int duration, int intensity)
    {
        var jsonData = $"{{\"Username\":\"{Username}\",\"Name\":\"{Name}\",\"Code\":\"{Code}\",\"Intensity\":\"{intensity}\",\"Duration\":\"{duration}\",\"Apikey\":\"{Apikey}\",\"Op\":\"0\"}}";
        return await SendPostRequest("apioperate/", jsonData);
    }

    public async Task<string> VibrateAsync(int duration, int intensity)
    {
        var jsonData = $"{{\"Username\":\"{Username}\",\"Name\":\"{Name}\",\"Code\":\"{Code}\",\"Intensity\":\"{intensity}\",\"Duration\":\"{duration}\",\"Apikey\":\"{Apikey}\",\"Op\":\"1\"}}";
        return await SendPostRequest("apioperate/", jsonData);
    }

    public async Task<string> BeepAsync(int duration)
    {
        var jsonData = $"{{\"Username\":\"{Username}\",\"Name\":\"{Name}\",\"Code\":\"{Code}\",\"Duration\":\"{duration}\",\"Apikey\":\"{Apikey}\",\"Op\":\"2\"}}";
        return await SendPostRequest("apioperate/", jsonData);
    }

    public async Task<string> GetShockerInfoAsync()
    {
        var jsonData = $"{{\"Username\":\"{Username}\",\"Code\":\"{Code}\",\"Apikey\":\"{Apikey}\"}}";
        return await SendPostRequest("GetShockerInfo", jsonData);
    }
}
