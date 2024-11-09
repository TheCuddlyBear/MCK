using Adw;
using CmlLib.Core;
using Gtk;
using MCK.util;
using ApplicationWindow = Gtk.ApplicationWindow;
using HeaderBar = Adw.HeaderBar;

namespace MCK.ui.Elements;

public class VersionChooserDialog : Adw.Dialog
{
    private ISettings _settings;
    private ListBox _listBox;
    private WindowTitle _windowTitle;

    public VersionChooserDialog(ISettings settings)
    {
        _settings = settings;
        BuildUi();
    }

    private void BuildUi()
    {
        // TODO: Store version information somewhere so it doesn't pull it on press
        HeightRequest = 400;

        var toolBarView = new ToolbarView();
        toolBarView.AddTopBar(GetHeaderBar());
        toolBarView.AddBottomBar(GetActionBar());

        var scrolledWindow = new ScrolledWindow();
        scrolledWindow.SetChild(_listBox = GetListBox());
        
        toolBarView.SetContent(scrolledWindow);

        HeaderBar GetHeaderBar()
        {
            var headerBar = new HeaderBar();
            var titleBox = new Gtk.Box();
            titleBox.Append(_windowTitle = new WindowTitle()
            {
                Title = "Choose version",
            });
            headerBar.SetTitleWidget(titleBox);
            
            return headerBar;
        }

        SetChild(toolBarView);

        ListBox GetListBox()
        {
            
            var listBox = new ListBox();
            var launcher = new MinecraftLauncher();
            var version = launcher.GetAllVersionsAsync().Result;
            List<string> versions = new List<string>();
            
            foreach (var ver in version)
            {
                listBox.Append(Label.New(ver.Name));
            }
            

            listBox.OnRowSelected += (sender, args) =>
            {
                
            };

            launcher = null;
            
            return listBox;
        }
        
        Gtk.ActionBar GetActionBar()
        {
            var actionBar = new ActionBar();

            var button = Button.New();
            button.SetLabel("Choose");
            button.OnClicked += (sender, args) =>
            {
                
                Label selectedlabel = (Label)_listBox.GetSelectedRow().GetChild();

                _settings.Version = selectedlabel.GetLabel();
                
                Program.WriteSettings(_settings);
                Close();
            };
            
            actionBar.PackEnd(button);
            
            return actionBar;

        }

    }

    public static void ChooseVersion(ApplicationWindow parent, ISettings settings)
    {
        var dialog = new VersionChooserDialog(settings);
        dialog.Present(parent);
    }

}