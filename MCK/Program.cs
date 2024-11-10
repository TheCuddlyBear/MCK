using System.Configuration;
using System.Runtime.InteropServices;
using MCK.ui.Windows;
using MCK.util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace MCK;

class Program
{
    static int Main(string[] args)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            throw new PlatformNotSupportedException();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Utility.FirstStartupWindows();
            const string dllDirectory = @"C:/msys64/mingw64/bin";
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + dllDirectory);
        }

        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string path = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? Path.Combine(folder, ".mck\\appsettings.json") : "//appsettings.json";
        var jsonReader = new JsonTextReader(new StreamReader(path));
        var JsonSerializer = new JsonSerializer();

        ISettings settings = JsonSerializer.Deserialize<ISettings>(jsonReader);
        
        jsonReader.Close();
        
        var application = Adw.Application.New("com.sirkadirov.overtest.tde", Gio.ApplicationFlags.FlagsNone);
        application.OnActivate += (sender, _) => new MainWindow((Adw.Application)sender, settings).Show();
        return application.RunWithSynchronizationContext(null);

    }

    public static void WriteSettings(ISettings settings)
    {
        string output = JsonConvert.SerializeObject(settings);
        
        using(StreamWriter r = new StreamWriter(System.IO.Directory.GetCurrentDirectory() + "\\appsettings.json", false))
        {
            r.Write(output);
        }
        
    }

    static bool IsDebugRelease
    {
        get
        {
            #if DEBUG
            return true;
            #else
            return false;
            #endif
        }
    }
}