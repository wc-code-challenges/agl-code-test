using System.Net.Http;
using System.Threading.Tasks;

namespace PetApi.Sdk
{
	public interface IHttpRequestProcessor
    {
		Task<T> ProcessRequestAsync<T>(HttpRequestMessage request);
    }
}
