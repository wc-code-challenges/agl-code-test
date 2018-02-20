using System.Collections.Generic;

namespace PetApp.DomainLogic
{
	public class PetOwnerClassificationListing
    {
		public IEnumerable<string> MaleOwnerCats { get; set; }

		public IEnumerable<string> FemaleOwnerCats { get; set; }
	}
}
