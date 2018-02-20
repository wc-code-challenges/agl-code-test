using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PetApp.DomainLogic;
using PetArranger.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace PetArranger.Controllers
{
	public class HomeController : Controller
    {
		private readonly IPetService _petService;
		private readonly IConfiguration _config;
		public const string ConfigJsonEndpointUrlPath = "PetApi:Url";

		public HomeController(IPetService petService, IConfiguration config)
		{
			_petService = petService;
			_config = config;
		}
		
        public async Task<IActionResult> Index()
        {
			var endpointForPetsUrl = _config[ConfigJsonEndpointUrlPath];
			var result = await _petService.GetClassifiedCatInformationAsync(endpointForPetsUrl);

			var model = new ClassifiedPetsModel("Arranged pets index: Pet App", ConvertEnumerableToList(result.MaleOwnerCats)
				, ConvertEnumerableToList(result.FemaleOwnerCats));

            return View(model);
        }

		private List<T> ConvertEnumerableToList<T>(IEnumerable<T> enumerable)
		{
			return enumerable == null ? new List<T>() : enumerable.ToList();
		}

        public IActionResult Error()
        {
            return View();
        }
    }
}
