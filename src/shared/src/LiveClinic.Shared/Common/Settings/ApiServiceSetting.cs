namespace LiveClinic.Shared.Common.Settings
{
    public class ApiServiceSetting
    {
        public const string Key = "ApiService";
        public string Name { get; set; }
        public string LocalPath { get; set; } 
        public string ApiAddress { get; set; }
    }
}