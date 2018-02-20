namespace PetArranger.Models
{
	/// <summary>
	/// This class contains basic metadata that each page needs e.g. SEO tags, Facebook API key, OG:Image, OG:Description, webanalytics tracking customization etc
	/// In this simple case it will only contain the title
	/// </summary>
	public class PetAppBaseModel
    {
		public PetAppBaseModel(string title = "Welcome to the Pet App") => Title = title;

		public string Title { get; set; }
	}
}
