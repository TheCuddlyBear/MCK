using Adw;

namespace MCK.ui.Windows;

public class MainWindow : ApplicationWindow
{
    private WindowTitle _windowTitle;
    private ToolbarView _mainToolbarView;
    private StatusPage _noProjectOpenedPage;
    private ToastOverlay _toastOverlay;
    
    public MainWindow(Application application)
    {
        Application = application;
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

        ToolbarView GetToolbarView()
        {
            var toolbarView = new ToolbarView();
            toolbarView.AddTopBar(GetHeaderBar());

            var contentBox = new Gtk.Box();
            contentBox.Homogeneous = true;
            
            return toolbarView;
        }

        HeaderBar GetHeaderBar()
        {
            var headerBar = new HeaderBar();
            var titleBox = new Gtk.Box();
            titleBox.Append(_windowTitle = new WindowTitle()
            {
                Title = "MCK Test",
                Subtitle = "MCK Test"
            });
            headerBar.SetTitleWidget(titleBox);
            
            var aboutButton = new Gtk.Button() { IconName = "help-about", TooltipText = "About program" };
            aboutButton.OnClicked += (_ ,_) => new MCK.ui.Elements.AboutDialog().Show(this);
            headerBar.PackEnd(aboutButton);
            
            return headerBar;
        }
        
    }
}