using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace PetApi.Sdk
{
	public class PetRepository : IPetRepository
	{
		private readonly IHttpRequestProcessor _httpRequestProcessor;

		public PetRepository(IHttpRequestProcessor requestProcessor)
		{
			_httpRequestProcessor = requestProcessor;
		}

		public async Task<PetApiResponse> GetPetOwnersAsync(string url)
		{
			if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException($"Url must be a valid Url the value provided is: {url}", "url");

			var request = new HttpRequestMessage(HttpMethod.Get, url);
			var apiResult = await _httpRequestProcessor.ProcessRequestAsync<List<Person>>(request);

			return new PetApiResponse() { PetOwners = apiResult };
		}
	}
}
