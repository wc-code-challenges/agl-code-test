using System.Threading.Tasks;

namespace PetApp.DomainLogic
{
	public interface IPetService
    {
		Task<PetOwnerClassificationListing> GetClassifiedCatInformationAsync(string url);
    }
}
