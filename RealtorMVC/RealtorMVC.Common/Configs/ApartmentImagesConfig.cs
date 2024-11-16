namespace RealtorMVC.Common.Configs;

public class ApartmentImagesConfig : ConfigBase
{
    public string Folder { get; set; } = string.Empty;
    public List<string> FileExtensions { get; set; } = null!;
    public string Path { get; set; } = string.Empty;
}