using System.Collections.Generic;
using Xunit;
using PetApp.DomainLogic;
using PetArranger.Controllers;
using PetArranger.Models;
using Moq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PetArranger.Tests
{
	public class HomeControllerTests
    {
		const string url = "https://anexampleurl.com/resource";

		private IConfiguration GetConfiguration()
		{
			var config = new Mock<IConfiguration>();

			config.Setup(x => x[HomeController.ConfigJsonEndpointUrlPath])
				.Returns(url);

			return config.Object;
		}

		[Fact]
		public void Index_CallsApiWithCorrectConfigUrl()
		{
			//Arrange
			string myUrlUsed = null;
			var petService = new Mock<IPetService>();
			petService.Setup(x => x.GetClassifiedCatInformationAsync(It.IsAny<string>()))
				.Callback<string>(s => myUrlUsed = s)
				.Returns(Task.FromResult(new PetOwnerClassificationListing()
				{
					FemaleOwnerCats = new List<string>() { "mouse trap" }
				}));
			var controller = new HomeController(petService.Object, GetConfiguration());

			//Act
			var result = controller.Index().Result;

			//Assert
			Assert.Equal(url, myUrlUsed);
		}

		[Fact]
		public void Index_WhenReturnedFemaleListIsNull_ListIsConvertedToEmptyList()
		{
			//Arrange
			string myUrlUsed = null;
			var petService = new Mock<IPetService>();
			petService.Setup(x => x.GetClassifiedCatInformationAsync(It.IsAny<string>()))
				.Callback<string>(s => myUrlUsed = s)
				.Returns(Task.FromResult(new PetOwnerClassificationListing()
				{
					FemaleOwnerCats = null
				}));
			var controller = new HomeController(petService.Object, GetConfiguration());

			//Act
			var view = controller.Index().Result as ViewResult;
			var result = (view.Model as ClassifiedPetsModel).OrderedPetNamesForFemales.Count;

			//Assert
			Assert.Equal(0, result);
		}

		[Fact]
		public void Index_WhenReturnedMaleListIsNull_ListIsConvertedToEmptyList()
		{
			//Arrange
			string myUrlUsed = null;
			var petService = new Mock<IPetService>();
			petService.Setup(x => x.GetClassifiedCatInformationAsync(It.IsAny<string>()))
				.Callback<string>(s => myUrlUsed = s)
				.Returns(Task.FromResult(new PetOwnerClassificationListing()
				{
					MaleOwnerCats = null
				}));
			var controller = new HomeController(petService.Object, GetConfiguration());

			//Act
			var view = controller.Index().Result as ViewResult;
			var result = (view.Model as ClassifiedPetsModel).OrderedPetNamesForMales.Count;

			//Assert
			Assert.Equal(0, result);
		}

		[Fact]
		public void Index_WhenReturnedMaleListHasValues_ListIsSetOnModelCorrectly()
		{
			//Arrange
			string myUrlUsed = null;
			var petService = new Mock<IPetService>();
			petService.Setup(x => x.GetClassifiedCatInformationAsync(It.IsAny<string>()))
				.Callback<string>(s => myUrlUsed = s)
				.Returns(Task.FromResult(new PetOwnerClassificationListing()
				{
					MaleOwnerCats = new List<string>()
					{
						"a", "b", "c"
					}
				}));
			var controller = new HomeController(petService.Object, GetConfiguration());

			//Act
			var view = controller.Index().Result as ViewResult;
			var result = (view.Model as ClassifiedPetsModel);

			//Assert
			Assert.Equal(0, result.OrderedPetNamesForFemales.Count);
			Assert.Equal(3, result.OrderedPetNamesForMales.Count);
			Assert.Equal("a", result.OrderedPetNamesForMales[0]);
			Assert.Equal("b", result.OrderedPetNamesForMales[1]);
			Assert.Equal("c", result.OrderedPetNamesForMales[2]);
		}

		[Fact]
		public void Index_WhenReturnedFemaleListHasValues_ListIsSetOnModelCorrectly()
		{
			//Arrange
			string myUrlUsed = null;
			var petService = new Mock<IPetService>();
			petService.Setup(x => x.GetClassifiedCatInformationAsync(It.IsAny<string>()))
				.Callback<string>(s => myUrlUsed = s)
				.Returns(Task.FromResult(new PetOwnerClassificationListing()
				{
					FemaleOwnerCats = new List<string>()
					{
						"a", "b", "c"
					}
				}));
			var controller = new HomeController(petService.Object, GetConfiguration());

			//Act
			var view = controller.Index().Result as ViewResult;
			var result = (view.Model as ClassifiedPetsModel);

			//Assert
			Assert.Equal(0, result.OrderedPetNamesForMales.Count);
			Assert.Equal(3, result.OrderedPetNamesForFemales.Count);
			Assert.Equal("a", result.OrderedPetNamesForFemales[0]);
			Assert.Equal("b", result.OrderedPetNamesForFemales[1]);
			Assert.Equal("c", result.OrderedPetNamesForFemales[2]);
		}
	}
}
