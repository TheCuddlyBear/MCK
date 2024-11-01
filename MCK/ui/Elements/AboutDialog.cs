using Gtk;

namespace MCK.ui.Elements;

public class AboutDialog
{
    private readonly Adw.AboutDialog _aboutDialog;
    
    public AboutDialog()
    {
        _aboutDialog = new Adw.AboutDialog();
        BuildUI();
    }

    private void BuildUI()
    {
        _aboutDialog.SetApplicationIcon("utilities-terminal");
        _aboutDialog.SetApplicationName("MCK");
        _aboutDialog.SetComments("A modern minecraft launcher using Libadwaita and GTK4 to fit in with gnome based operating systems");
        _aboutDialog.SetLicenseType(License.Agpl30);
        _aboutDialog.SetVersion("0.0.1-alpha");
        _aboutDialog.SetDeveloperName("TheCuddlyBear");
        _aboutDialog.SetCopyright("Copyright (c) 2024 TheCuddlyBear");
        _aboutDialog.SetDevelopers(new[] { "TheCuddlyBear" });
    }
    
    public void Show(ApplicationWindow parent) => _aboutDialog.Present(parent);
}