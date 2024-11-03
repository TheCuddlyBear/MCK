using Adw;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.ProcessBuilder;
using Gtk;
using MCK.util;
using Microsoft.Extensions.Hosting;
using XboxAuthNet.Game.Msal;
using Application = Adw.Application;
using ApplicationWindow = Adw.ApplicationWindow;
using HeaderBar = Adw.HeaderBar;

namespace MCK.ui.Windows;

public class MainWindow : ApplicationWindow
{
    private WindowTitle _windowTitle;
    private NavigationSplitView _mainToolbarView;
    private StatusPage _noProjectOpenedPage;
    private ToastOverlay _toastOverlay;

    private Gtk.ProgressBar _progress;

    private ISettings _settings;
    
    public MainWindow(Application application, ISettings settings)
    {
        Application = application;
        _settings = settings;
        BuildUi();
    }

    private async void LaunchMC(ProgressBar bar)
    {
        //var loginHandler = JELoginHandlerBuilder.BuildDefault();
        
        //var app = await MsalClientHelper.BuildApplicationWithCache()
        
        // Only offline sessions till appid is approved.
        var session = MSession.CreateOfflineSession(_settings.OfflineUsername);
        Console.Out.WriteLine($"Login: {session.Username}");
        
        var launcher = new MinecraftLauncher();
        launcher.FileProgressChanged += (_, e) =>
        {
            double frac = (double)e.ProgressedTasks / (double)e.TotalTasks;
            
            _progress.SetFraction(frac);
            _progress.SetText(e.Name);
            
            /*Console.WriteLine("Name: " + e.Name);
            Console.WriteLine("EventType: " + e.EventType);
            Console.WriteLine("TotalTasks: " + e.TotalTasks);
            Console.WriteLine("ProgressedTasks: " + e.ProgressedTasks);*/
        };
        var proces = await launcher.InstallAndBuildProcessAsync("1.20.4", new MLaunchOption()
        {
            Session = session
        });
        proces.Start();
    }

    private void BuildUi()
    {
        Title = "MCK";
        SetDefaultSize(800, 600);
        SetSizeRequest(800, 600);

        _mainToolbarView = GetToolbarView();
        SetContent(_mainToolbarView);
        return;

        NavigationSplitView GetToolbarView()
        {
            _progress = new ProgressBar();
            _progress.SetShowText(true);
            _progress.SetText("Test");
            
            var toolbarView = new ToolbarView();
            toolbarView.AddTopBar(GetHeaderBar());
            toolbarView.SetContent(_progress);
            
            var launchButton = new Gtk.Button();
            launchButton.SetLabel("Launch MC");
            launchButton.OnClicked += (_, __) => LaunchMC(_progress);
            
            var sideBarview = new ToolbarView();
            sideBarview.AddTopBar(GetSideHeaderBar());
            sideBarview.SetContent(launchButton);

            var sidePane = new NavigationPage();
            sidePane.SetChild(sideBarview);
            
            var mainPane = new NavigationPage();
            mainPane.SetChild(toolbarView);
            
            var splitView = new NavigationSplitView();
            splitView.SetSidebar(sidePane);
            splitView.SetContent(mainPane);
            
            return splitView;
        }

        HeaderBar GetHeaderBar()
        {
            var headerBar = new HeaderBar();
            var titleBox = new Gtk.Box();
            titleBox.Append(_windowTitle = new WindowTitle()
            {
                Title = "Vanilla",
            });
            headerBar.SetTitleWidget(titleBox);
            
            var settingsButton = new Gtk.Button() { IconName = "org.gnome.Settings-symbolic", TooltipText = "Preferences" };
            settingsButton.OnClicked += (_ ,_) => new MCK.ui.Elements.PreferencesDialog(_settings).Show(this);
            headerBar.PackEnd(settingsButton);
            
            return headerBar;
        }
        
        HeaderBar GetSideHeaderBar()
        {
            var headerBar = new HeaderBar();
            var titleBox = new Gtk.Box();
            titleBox.Append(_windowTitle = new WindowTitle()
            {
                Title = "MCK Test",
            });
            headerBar.SetTitleWidget(titleBox);
            
            var aboutButton = new Gtk.Button() { IconName = "help-about", TooltipText = "About program" };
            aboutButton.OnClicked += (_ ,_) => new MCK.ui.Elements.AboutDialog().Show(this);
            headerBar.PackEnd(aboutButton);
            
            return headerBar;
        }
        
    }
}