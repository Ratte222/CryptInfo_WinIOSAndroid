namespace MauiCryptApp.Views;
//https://docs.microsoft.com/en-us/dotnet/maui/platform-integration/storage/file-system-helpers?tabs=android
public partial class FilePickerTest : ContentPage
{
    string path = Path.Combine(FileSystem.Current.AppDataDirectory, "TestText.txt");
    public FilePickerTest()
	{
		InitializeComponent();
	}
    
    private void OnReadClicked(object sender, EventArgs e)
    {
        //var filePickerFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        //        {
        //            { DevicePlatform.WinUI, new[] { ".txt" } },
        //            { DevicePlatform.Android, new[] { ".txt" } }
        //        });
        //PickOptions pickOptions = new PickOptions();
        //pickOptions.PickerTitle = "TestText";

        //pickOptions.FileTypes = filePickerFileType;
        //var path1 = FileSystem.Current.AppDataDirectory;
        //var path2 = FileSystem.Current.CacheDirectory;
        
        //var fileResult = FilePicker.Default.PickAsync(pickOptions).GetAwaiter().GetResult();
        var result = File.ReadAllText(path);
        DataFromFile.Text = "Readed";
        Input.Text = result;
    }

    private void OnWriteClicked(object sender, EventArgs e)
    {
        File.WriteAllText(path, Input.Text);
        DataFromFile.Text = "Saved";
    }
}