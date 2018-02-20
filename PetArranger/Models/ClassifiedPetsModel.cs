using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetArranger.Models
{
    public class ClassifiedPetsModel: PetAppBaseModel
    {
		public ClassifiedPetsModel(string title, List<string> orderedPetNamesForMales, List<string> orderedPetNamesForFemales)
			:base(title)
		{
			OrderedPetNamesForFemales = orderedPetNamesForFemales;
			OrderedPetNamesForMales = orderedPetNamesForMales;
		}

		public List<string> OrderedPetNamesForMales { get; set; }

		public List<string> OrderedPetNamesForFemales { get; set; }
	}
}
