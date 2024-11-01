using Adw;
using CmlLib.Core;
using CmlLib.Core.ProcessBuilder;

namespace MCK.ui.Windows;

public class MainWindow : ApplicationWindow
{
    private WindowTitle _windowTitle;
    private NavigationSplitView _mainToolbarView;
    private StatusPage _noProjectOpenedPage;
    private ToastOverlay _toastOverlay;
    
    public MainWindow(Application application)
    {
        Application = application;
        BuildUi();
    }

    private async void LaunchMC()
    {
        var launcher = new MinecraftLauncher();
        var process = await launcher.InstallAndBuildProcessAsync("1.21", new MLaunchOption());
        process.Start();
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
            var toolbarView = new ToolbarView();
            toolbarView.AddTopBar(GetHeaderBar());

            var launchButton = new Gtk.Button();
            launchButton.SetLabel("Launch MC");
            launchButton.OnClicked += (_, __) => LaunchMC();
            
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
            
            /*var aboutButton = new Gtk.Button() { IconName = "help-about", TooltipText = "About program" };
            aboutButton.OnClicked += (_ ,_) => new MCK.ui.Elements.AboutDialog().Show(this);
            headerBar.PackEnd(aboutButton);*/
            
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