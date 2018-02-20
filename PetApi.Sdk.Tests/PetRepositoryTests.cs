using Xunit;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace PetApi.Sdk.Tests
{
	public class PetRepositoryTests
    {
		public PetRepositoryTests()
		{
			correctUrlResponse = new List<Person>()
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
							Name = "Rex",
							Type = "cat"
						}
					}
				}
			};
		}

		#region setup

		const string url = "http://anyexample.com/resource";

		readonly List<Person> correctUrlResponse; 

		private Task<List<Person>> MockRequestFunction(HttpRequestMessage request)
		{
			if (request.RequestUri.ToString().ToUpper() == url.ToUpper())
				return Task.FromResult(correctUrlResponse);

			return Task.FromResult(new List<Person>());
		}

		#endregion

		[Fact]
		public void GetPetOwnersAsync_NullUrl_ThrowsArgumentException()
		{
			//Arrange
			var requestProcessor = new Mock<IHttpRequestProcessor>();
			requestProcessor.Setup(x => x.ProcessRequestAsync<List<Person>>(It.IsAny<HttpRequestMessage>()))
				.Returns(Task.FromResult<List<Person>>(null));
			var petRepository = new PetRepository(requestProcessor.Object);

			//Act & Assert
			Assert.ThrowsAsync<ArgumentException>(() => petRepository.GetPetOwnersAsync(null));
		}

		[Fact]
		public void GetPetOwnersAsync_EmptyStringUrl_ThrowsArgumentException()
		{
			//Arrange
			var requestProcessor = new Mock<IHttpRequestProcessor>();
			requestProcessor.Setup(x => x.ProcessRequestAsync<List<Person>>(It.IsAny<HttpRequestMessage>()))
				.Returns(Task.FromResult<List<Person>>(null));
			var petRepository = new PetRepository(requestProcessor.Object);

			//Act & Assert
			Assert.ThrowsAsync<ArgumentException>(() => petRepository.GetPetOwnersAsync(string.Empty));
		}

		[Fact]
		public void GetPetOwnersAsync_SpacesOnlyUrl_ThrowsArgumentException()
		{
			//Arrange
			var requestProcessor = new Mock<IHttpRequestProcessor>();
			requestProcessor.Setup(x => x.ProcessRequestAsync<List<Person>>(It.IsAny<HttpRequestMessage>()))
				.Returns(Task.FromResult<List<Person>>(null));
			var petRepository = new PetRepository(requestProcessor.Object);

			//Act & Assert
			Assert.ThrowsAsync<ArgumentException>(() => petRepository.GetPetOwnersAsync("\t  "));
		}

		[Fact]
		public void GetPetOwnersAsync_CorrectUrlPassed_RetrievesFromUrl()
		{
			//Arrange
			var requestProcessor = new Mock<IHttpRequestProcessor>();
			requestProcessor.Setup(x => x.ProcessRequestAsync<List<Person>>(It.IsAny<HttpRequestMessage>()))
				.Returns((HttpRequestMessage request) => MockRequestFunction(request));
			var petRepository = new PetRepository(requestProcessor.Object);

			//Act
			var result = petRepository.GetPetOwnersAsync(url).Result;

			//assert
			Assert.Equal(1, result.PetOwners.Count);
		}
	}
}
