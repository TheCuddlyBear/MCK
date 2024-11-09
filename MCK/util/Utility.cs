using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.ProcessBuilder;
using Gtk;

namespace MCK.util;

public class Utility
{
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