using System.Configuration;

namespace ContactInformation.Models
{
	public static class Settings
	{
		public static class DBConnectionString
		{
			public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["ContactInformationDataContext"].ToString(); } }
		}
		public static class AppSetting
		{

		}
		public static class DBKeys
		{
		}
	}          
}