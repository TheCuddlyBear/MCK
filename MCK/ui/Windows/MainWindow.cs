using Adw;
using CmlLib.Core;
using Gtk;
using MCK.ui.Elements;
using MCK.util;
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

    private ProgressBar _progress;

    private ISettings _settings;
    
    public MainWindow(Application application, ISettings settings)
    {
        Application = application;
        _settings = settings;
        _progress = new ProgressBar();
        BuildUi();
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
            toolbarView.AddBottomBar(GetActionBar());
            
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

            var versionButton = new Gtk.Button();
            versionButton.SetLabel(_settings.Version);
            
            versionButton.OnClicked += (sender, args) =>
                {
                    VersionChooserDialog.ChooseVersion(this ,_settings, sender);
                }
;            
            var launchButton = new Gtk.Button();
            launchButton.SetLabel("Launch");
            launchButton.OnClicked += (_, _) => Utility.LaunchMC(_progress, _settings);
            launchButton.WidthRequest = 150;
            
            actionBar.SetCenterWidget(launchButton);
            actionBar.PackStart(versionButton);

            actionBar.HeightRequest = 60;
            
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