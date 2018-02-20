using System.Collections.Generic;
using Newtonsoft.Json;

namespace PetApi.Sdk
{
	public class PetApiResponse
	{
		public List<Person> PetOwners { get; set; }
	}

	public class Person
	{
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }
		[JsonProperty(PropertyName = "gender")]
		public string Gender { get; set; }
		[JsonProperty(PropertyName = "age")]
		public int Age { get; set; }
		[JsonProperty(PropertyName = "pets")]
		public List<Pet> Pets { get; set; }
	}

	public class Pet
	{
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }
		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }
	}
}
