using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.ProcessBuilder;
using Gtk;
using Newtonsoft.Json;

namespace MCK.util;

public class Utility
{

    public static async void FirstStartupWindows()
    {
        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        
        if (!Directory.Exists(Path.Combine(folder, ".mck")))
        {
            Console.WriteLine("No app directory!");
            Directory.CreateDirectory(Path.Combine(folder, ".mck"));

            HttpClient client = new HttpClient();

            HttpResponseMessage responseMessage = await client.GetAsync("https://pastebin.com/raw/GxUShSk1");
            string json = await responseMessage.Content.ReadAsStringAsync();

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(folder, ".mck\\appsettings.json")))
            {
                outputFile.WriteLine(json);
            }
        }
        
    }
    
    public static void FirstStartupLinux()
    {
        
    }
    
    public static async void LaunchMC(ProgressBar bar, ISettings settings)
    {
        //var loginHandler = JELoginHandlerBuilder.BuildDefault();
        
        //var app = await MsalClientHelper.BuildApplicationWithCache()
        
        // Only offline sessions till appid is approved.
        var session = MSession.CreateOfflineSession(settings.OfflineUsername);
        Console.Out.WriteLine($"Login: {session.Username}");
        
        bar.Show();
        var launcher = new MinecraftLauncher();
        launcher.FileProgressChanged += (_, e) =>
        {
            double frac = (double)e.ProgressedTasks / (double)e.TotalTasks;
            
            bar.SetFraction(frac);
            //bar.SetText(e.Name);
            
            /*Console.WriteLine("Name: " + e.Name);
            Console.WriteLine("EventType: " + e.EventType);
            Console.WriteLine("TotalTasks: " + e.TotalTasks);
            Console.WriteLine("ProgressedTasks: " + e.ProgressedTasks);*/
        };
        var proces = await launcher.InstallAndBuildProcessAsync(settings.Version, new MLaunchOption()
        {
            Session = session
        });
        proces.Start();
        bar.Hide();
    }
}