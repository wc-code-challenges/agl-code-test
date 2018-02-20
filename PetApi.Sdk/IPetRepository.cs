using System.Threading.Tasks;

namespace PetApi.Sdk
{
	public interface IPetRepository
    {
		Task<PetApiResponse> GetPetOwnersAsync(string url);
	}
}
