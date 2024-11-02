using Gtk;

namespace MCK.ui.Elements;

public class PreferencesDialog
{
    private readonly Adw.PreferencesDialog _preferencesDialog;

    public PreferencesDialog()
    {
        _preferencesDialog = new Adw.PreferencesDialog();

        var prefPage = new MCK.ui.Elements.PreferencePage();
        
        _preferencesDialog.Add(prefPage);
        

    }
    
    public void Show(ApplicationWindow parent) => _preferencesDialog.Present(parent);
    
}