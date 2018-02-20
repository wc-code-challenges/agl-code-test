using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PetApi.Sdk
{
	public class HttpRequestProcessor : IHttpRequestProcessor
	{
		#region Http Client
		/// <summary>
		/// even though Http client is disposeable in a we environment one should use a single instance
		/// Please see the following documentation: https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
		/// So the Http Client will use the Singleton pattern in our application
		/// </summary>
		private static readonly HttpClient _httpClientInstance;

		/// <summary>
		/// We always expect Json in this implementation
		/// </summary>
		const string JsonContentMimeType = "application/json";

		/// <summary>
		/// Please see the following article for a detailed discussion on implementation of singleton patterns
		/// http://csharpindepth.com/Articles/General/Singleton.aspx for a discussion on static constructors see
		/// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-constructors
		/// </summary>
		static HttpRequestProcessor()
		{
			_httpClientInstance = new HttpClient();
			//add a default expected content type
			_httpClientInstance.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(JsonContentMimeType));
		}

		#endregion

		public async Task<T> ProcessRequestAsync<T>(HttpRequestMessage request)
		{
			var httpResponse = await _httpClientInstance.SendAsync(request);
			httpResponse.EnsureSuccessStatusCode();

			var content = await httpResponse.Content.ReadAsStringAsync();

			var result = JsonConvert.DeserializeObject<T>(content);

			return result;
		} 
	}
}
