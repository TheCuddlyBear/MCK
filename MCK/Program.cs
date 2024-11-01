using System.Runtime.InteropServices;
using MCK.ui.Windows;

namespace MCK;

class Program
{
    static int Main(string[] args)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            throw new PlatformNotSupportedException();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            const string dllDirectory = @"C:/msys64/mingw64/bin";
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + dllDirectory);
        }
        
        var application = Adw.Application.New("com.sirkadirov.overtest.tde", Gio.ApplicationFlags.FlagsNone);
        application.OnActivate += (sender, _) => new MainWindow((Adw.Application)sender).Show();
        return application.RunWithSynchronizationContext(null);
    }
}