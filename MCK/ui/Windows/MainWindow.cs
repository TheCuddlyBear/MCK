using Adw;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.ProcessBuilder;
using Gtk;
using MCK.ui.Elements;
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

    private async void LaunchMC(ProgressBar bar, string version)
    {
        //var loginHandler = JELoginHandlerBuilder.BuildDefault();
        
        //var app = await MsalClientHelper.BuildApplicationWithCache()
        
        // Only offline sessions till appid is approved.
        var session = MSession.CreateOfflineSession(_settings.OfflineUsername);
        Console.Out.WriteLine($"Login: {session.Username}");
        
        bar.Show();
        var launcher = new MinecraftLauncher();
        launcher.FileProgressChanged += (_, e) =>
        {
            double frac = (double)e.ProgressedTasks / (double)e.TotalTasks;
            
            bar.SetFraction(frac);
            bar.SetText(e.Name);
            
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
        bar.Hide();
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
            //_progress.SetShowText(true);
            _progress.SetText("Test");
            _progress.Hide();
            
            var toolbarView = new ToolbarView();
            toolbarView.AddTopBar(GetHeaderBar());
            toolbarView.AddBottomBar(GetActionBar());
            toolbarView.SetContent(_progress);
            
            var sideBarview = new ToolbarView();
            sideBarview.AddTopBar(GetSideHeaderBar());

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

        Gtk.ActionBar GetActionBar()
        {
            var actionBar = new ActionBar();
            var launcher = new MinecraftLauncher();
            var version = launcher.GetAllVersionsAsync().Result;
            List<string> versions = new List<string>();
            
            foreach (var ver in version)
            {
                versions.Append(ver.Name);
            }

            var versionButton = new Gtk.Button();
            versionButton.SetLabel("Choose version");
            versionButton.OnClicked += (sender, args) =>
                {
                    VersionChooserDialog.ChooseVersion(this ,"1.20.4");
                }
;            
            var launchButton = new Gtk.Button();
            launchButton.SetLabel("Launch");
            launchButton.OnClicked += (_, __) => LaunchMC(_progress, "1.20.4");
            launchButton.WidthRequest = 150;
            
            actionBar.SetCenterWidget(launchButton);
            actionBar.PackStart(versionButton);

            actionBar.HeightRequest = 60;

            launcher = null;
            return actionBar;

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