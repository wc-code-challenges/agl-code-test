using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PetApi.Sdk;

namespace PetApp.DomainLogic
{
	public class PetService : IPetService
	{
		public const String PetTypeCat = "cat";
		public const String GenderMale = "male";
		public const String GenderFemale = "female";

		private readonly IPetRepository _petRepository;

		public PetService(IPetRepository petRepository) => _petRepository = petRepository;
		
		/// <summary>
		/// This returns each Cat under the gender of it's owner sorted in lexicographical order while ignoring case
		/// </summary>
		/// <param name="url">The full url to the endpoint containg the JSON file</param>
		/// <returns></returns>
		public async Task<PetOwnerClassificationListing> GetClassifiedCatInformationAsync(string url)
		{
			IEnumerable<string> GetGenderCats(string gender, IEnumerable<Tuple<string, IEnumerable<string>>> genderClassifiedCatNames)
			{
				var tuple = genderClassifiedCatNames.FirstOrDefault(x => x.Item1 == gender);
				return tuple?.Item2;
			}

			var petApiResponse = await _petRepository.GetPetOwnersAsync(url);
			
			if (petApiResponse == null || petApiResponse.PetOwners == null) return null;
			//Assumption: Normal users do not see A < a, they assume these are both equal thus the application will behave as such
			//male same [Male, mAle e.t.c] and the same will go for the pet/cat names under this assumption
			//Upper case characters come before lower case characters please see for further details: https://cs.fit.edu/~ryan/cse1002/lectures/lexicographic.pdf
			var catsClassifiedByGender = from owner in petApiResponse.PetOwners
										 where owner.Pets != null
										 let pets = owner.Pets
										 from pet in pets
										 where pet.Type.Equals(PetTypeCat, StringComparison.OrdinalIgnoreCase)
										 let genderPetName = new { PetName = pet.Name, OwnerGender = owner.Gender }
										 group genderPetName by genderPetName.OwnerGender.ToLower() into g
										 select new Tuple<string, IEnumerable<string>>(g.Key, g.OrderBy(x => x.PetName.ToLower()).Select(x => x.PetName));
			//return a domain model
			return new PetOwnerClassificationListing()
			{
				FemaleOwnerCats = GetGenderCats(GenderFemale, catsClassifiedByGender),
				MaleOwnerCats = GetGenderCats(GenderMale, catsClassifiedByGender)
			};
		}
	}
}
