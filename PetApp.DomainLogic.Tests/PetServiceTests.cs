using Xunit;
using Moq;
using PetApi.Sdk;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace PetApp.DomainLogic.Tests
{
	public class PetServiceTests
    {
		#region utility methods
		const string url = "https://anexampleurl.com/resource";

		private PetApiResponse GetSingleGenderValue()
		{
			return new PetApiResponse()
			{
				PetOwners = new List<Person>()
					{
						new Person()
						{
							Age = 25,
							Gender = "Male",
							Name = "Peter Pan",
							Pets = new List<Pet>()
							{
								new Pet()
								{
									Name = "chowder",
									Type = "cat"
								},
								new Pet()
								{
									Name = "mouse trap",
									Type = "cat"
								},
								new Pet()
								{
									Name = "Rex",
									Type = "dog"
								}

							}
						},
						new Person()
						{
							Age = 55,
							Gender = "Male",
							Name = "Captain Hook",
							Pets = new List<Pet>()
							{
								new Pet()
								{
									Name = "wallace",
									Type = "cat"
								}
							}
						}
					}
			};
		}

		private PetApiResponse GetSingleGenderValueWithDifferingNameCases()
		{
			
			return new PetApiResponse()
			{
				PetOwners = new List<Person>()
					{
						new Person()
						{
							Age = 25,
							Gender = "Male",
							Name = "Peter Pan",
							Pets = new List<Pet>()
							{
								new Pet()
								{
									Name = "chowder",
									Type = "cat"
								},
								new Pet()
								{
									//if case is not ignored this would come above chowder
									Name = "Mouse trap",
									Type = "cat"
								},
								new Pet()
								{
									Name = "Rex",
									Type = "dog"
								}

							}
						},
						new Person()
						{
							Age = 55,
							Gender = "Male",
							Name = "Captain Hook",
							Pets = new List<Pet>()
							{
								new Pet()
								{
									Name = "wallace",
									Type = "cat"
								}
							}
						}
					}
			};
		}

		private PetApiResponse GetSingleGenderValueWithDifferingTypeCase()
		{
			return new PetApiResponse()
			{
				PetOwners = new List<Person>()
					{
						new Person()
						{
							Age = 25,
							Gender = "Male",
							Name = "Peter Pan",
							Pets = new List<Pet>()
							{
								new Pet()
								{
									Name = "chowder",
									Type = "Cat"
								},
								new Pet()
								{
									Name = "mouse trap",
									Type = "cAt"
								},
								new Pet()
								{
									Name = "Rex",
									Type = "dOg"
								}

							}
						},
						new Person()
						{
							Age = 55,
							Gender = "Male",
							Name = "Captain Hook",
							Pets = new List<Pet>()
							{
								new Pet()
								{
									Name = "wallace",
									Type = "caT"
								}
							}
						}
					}
			};
		}

		private PetApiResponse GetTwoGenderValues()
		{
			return new PetApiResponse()
			{
				PetOwners = new List<Person>()
					{
						new Person()
						{
							Age = 21,
							Gender = "Female",
							Name = "Tinkerbell",
							Pets = new List<Pet>()
							{
								new Pet()
								{
									Name = "anastasia",
									Type = "Cat"
								},
								new Pet()
								{
									Name = "milton",
									Type = "Cat"
								}

							}
						},
						new Person()
						{
							Age = 25,
							Gender = "Male",
							Name = "Peter Pan",
							Pets = new List<Pet>()
							{
								new Pet()
								{
									Name = "chowder",
									Type = "Cat"
								},
								new Pet()
								{
									Name = "mouse trap",
									Type = "Cat"
								},
								new Pet()
								{
									Name = "Rex",
									Type = "Dog"
								}

							}
						},
						new Person()
						{
							Age = 35,
							Gender = "Female",
							Name = "Little Mermaid",
							Pets = new List<Pet>()
							{
								new Pet()
								{
									Name = "nicola",
									Type = "Cat"
								},
								new Pet()
								{
									Name = "Shumba",
									Type = "dog"
								}

							}
						},

						new Person()
						{
							Age = 55,
							Gender = "Male",
							Name = "Captain Hook",
							Pets = new List<Pet>()
							{
								new Pet()
								{
									Name = "wallace",
									Type = "caT"
								}
							}
						}
					}
			};
		}

		private PetApiResponse GetTwoGenderValuesWithDifferingGenderCase()
		{
			return new PetApiResponse()
			{
				PetOwners = new List<Person>()
					{
						new Person()
						{
							Age = 21,
							Gender = "Female",
							Name = "Tinkerbell",
							Pets = new List<Pet>()
							{
								new Pet()
								{
									Name = "anastasia",
									Type = "Cat"
								},
								new Pet()
								{
									Name = "milton",
									Type = "Cat"
								}

							}
						},
						new Person()
						{
							Age = 25,
							Gender = "male",
							Name = "Peter Pan",
							Pets = new List<Pet>()
							{
								new Pet()
								{
									Name = "chowder",
									Type = "Cat"
								},
								new Pet()
								{
									Name = "mouse trap",
									Type = "Cat"
								},
								new Pet()
								{
									Name = "Rex",
									Type = "Dog"
								}

							}
						},
						new Person()
						{
							Age = 35,
							Gender = "femaLe",
							Name = "Little Mermaid",
							Pets = new List<Pet>()
							{
								new Pet()
								{
									Name = "nicola",
									Type = "Cat"
								},
								new Pet()
								{
									Name = "Shumba",
									Type = "dog"
								}

							}
						},

						new Person()
						{
							Age = 55,
							Gender = "Male",
							Name = "Captain Hook",
							Pets = new List<Pet>()
							{
								new Pet()
								{
									Name = "wallace",
									Type = "caT"
								}
							}
						}
					}
			};
		}
		#endregion

		[Fact]
		public void GetClassifiedCatInformationAsync_NullResponseInPersonsList_ReturnsEmptyObject()
		{
			//Arrange
			var petRepository = new Mock<IPetRepository>();
			petRepository.Setup(x => x.GetPetOwnersAsync(url))
				.Returns(Task.FromResult(new PetApiResponse()));
			var service = new PetService(petRepository.Object);

			//Act
			var result = service.GetClassifiedCatInformationAsync(url).Result;

			//Assert
			Assert.Null(result);
		}

		[Fact]
		public void GetClassifiedCatInformationAsync_NullResponse_ReturnsEmptyObject()
		{
			//Arrange
			var petRepository = new Mock<IPetRepository>();
			petRepository.Setup(x => x.GetPetOwnersAsync(url))
				.Returns(Task.FromResult<PetApiResponse>(null));
			var service = new PetService(petRepository.Object);

			//Act
			var result = service.GetClassifiedCatInformationAsync(url).Result;

			//Assert
			Assert.Null(result);
		}

		[Fact]
		public void GetClassifiedCatInformationAsync_EmptyListInResponse_ReturnsEmptyValuesInObject()
		{
			//Arrange
			var petRepository = new Mock<IPetRepository>();
			petRepository.Setup(x => x.GetPetOwnersAsync(url))
				.Returns(Task.FromResult<PetApiResponse>(new PetApiResponse() { PetOwners = new List<Person>() }));
			var service = new PetService(petRepository.Object);

			//Act
			var result = service.GetClassifiedCatInformationAsync(url).Result;

			//Assert
			Assert.Null(result.FemaleOwnerCats);
			Assert.Null(result.MaleOwnerCats);
		}

		[Fact]
		public void GetClassifiedCatInformationAsync_OnlyOneGenderInList_ReturnsEmptyForNotProvidedGender()
		{
			//Arrange
			var petRepository = new Mock<IPetRepository>();
			petRepository.Setup(x => x.GetPetOwnersAsync(url))
				.Returns(Task.FromResult(GetSingleGenderValue()));
			var service = new PetService(petRepository.Object);

			//Act
			var result = service.GetClassifiedCatInformationAsync(url).Result;

			//Assert
			Assert.Null(result.FemaleOwnerCats);
		}

		[Fact]
		public void GetClassifiedCatInformationAsync_OnlyOneGenderInList_ReturnsCorrectOrderForProvidedGender()
		{
			//Arrange
			var petRepository = new Mock<IPetRepository>();
			petRepository.Setup(x => x.GetPetOwnersAsync(url))
				.Returns(Task.FromResult(GetSingleGenderValue()));
			var service = new PetService(petRepository.Object);

			//Act
			var result = service.GetClassifiedCatInformationAsync(url).Result.MaleOwnerCats.ToList();

			//Assert
			Assert.Equal(3, result.Count);
			Assert.Equal("chowder", result[0]);
			Assert.Equal("mouse trap", result[1]);
			Assert.Equal("wallace", result[2]);
		}

		[Fact]
		public void GetClassifiedCatInformationAsync_OnlyOneGenderInListWithDifferentTypeCases_CanFindAllThePetsRegardlessOfGender()
		{
			//Arrange
			var petRepository = new Mock<IPetRepository>();
			petRepository.Setup(x => x.GetPetOwnersAsync(url))
				.Returns(Task.FromResult(GetSingleGenderValueWithDifferingTypeCase()));
			var service = new PetService(petRepository.Object);

			//Act
			var result = service.GetClassifiedCatInformationAsync(url).Result.MaleOwnerCats.ToList();

			//Assert
			Assert.Equal(3, result.Count);
			Assert.Equal("chowder", result[0]);
			Assert.Equal("mouse trap", result[1]);
			Assert.Equal("wallace", result[2]);
		}

		[Fact]
		public void GetClassifiedCatInformationAsync_OnlyOneGenderInListWithDifferentNameCases_CanFindAllThePetsRegardlessOfGender()
		{
			//Arrange
			var petRepository = new Mock<IPetRepository>();
			petRepository.Setup(x => x.GetPetOwnersAsync(url))
				.Returns(Task.FromResult(GetSingleGenderValueWithDifferingNameCases()));
			var service = new PetService(petRepository.Object);

			//Act
			var result = service.GetClassifiedCatInformationAsync(url).Result.MaleOwnerCats.ToList();

			//Assert
			Assert.Equal(3, result.Count);
			Assert.Equal("chowder", result[0].ToLower());
			Assert.Equal("mouse trap", result[1].ToLower());
			Assert.Equal("wallace", result[2].ToLower());
		}

		[Fact]
		public void GetClassifiedCatInformationAsync_TwoGendersInList_CanFindAllTheCatsForMaleGender()
		{
			//Arrange
			var petRepository = new Mock<IPetRepository>();
			petRepository.Setup(x => x.GetPetOwnersAsync(url))
				.Returns(Task.FromResult(GetTwoGenderValues()));
			var service = new PetService(petRepository.Object);

			//Act
			var result = service.GetClassifiedCatInformationAsync(url).Result.MaleOwnerCats.ToList();

			//Assert
			Assert.Equal(3, result.Count);
			Assert.Equal("chowder", result[0]);
			Assert.Equal("mouse trap", result[1]);
			Assert.Equal("wallace", result[2]);
		}

		[Fact]
		public void GetClassifiedCatInformationAsync_TwoGendersInList_CanFindAllTheCatsForFemaleGender()
		{
			//Arrange
			var petRepository = new Mock<IPetRepository>();
			petRepository.Setup(x => x.GetPetOwnersAsync(url))
				.Returns(Task.FromResult(GetTwoGenderValues()));
			var service = new PetService(petRepository.Object);

			//Act
			var result = service.GetClassifiedCatInformationAsync(url).Result.FemaleOwnerCats.ToList();

			//Assert
			Assert.Equal(3, result.Count);
			Assert.Equal("anastasia", result[0]);
			Assert.Equal("milton", result[1]);
			Assert.Equal("nicola", result[2]);
		}

		[Fact]
		public void GetClassifiedCatInformationAsync_TwoGendersInListWithDifferingGenderCase_CanFindAllTheCatsForFemaleGender()
		{
			//Arrange
			var petRepository = new Mock<IPetRepository>();
			petRepository.Setup(x => x.GetPetOwnersAsync(url))
				.Returns(Task.FromResult(GetTwoGenderValuesWithDifferingGenderCase()));
			var service = new PetService(petRepository.Object);

			//Act
			var result = service.GetClassifiedCatInformationAsync(url).Result.FemaleOwnerCats.ToList();

			//Assert
			Assert.Equal(3, result.Count);
			Assert.Equal("anastasia", result[0]);
			Assert.Equal("milton", result[1]);
			Assert.Equal("nicola", result[2]);
		}

		[Fact]
		public void GetClassifiedCatInformationAsync_TwoGendersInListWithDifferingGenderCase_CanFindAllTheCatsForMaleGender()
		{
			//Arrange
			var petRepository = new Mock<IPetRepository>();
			petRepository.Setup(x => x.GetPetOwnersAsync(url))
				.Returns(Task.FromResult(GetTwoGenderValuesWithDifferingGenderCase()));
			var service = new PetService(petRepository.Object);

			//Act
			var result = service.GetClassifiedCatInformationAsync(url).Result.MaleOwnerCats.ToList();

			//Assert
			Assert.Equal(3, result.Count);
			Assert.Equal("chowder", result[0]);
			Assert.Equal("mouse trap", result[1]);
			Assert.Equal("wallace", result[2]);
		}

	}
}
