using System.Collections.Specialized;
using Adw;
using MCK.util;

namespace MCK.ui.Elements;

public class PreferencePage : PreferencesPage
{
    private ISettings _settings;
    private NameValueCollection _appSettings;
    
    public PreferencePage(ISettings settings)
    {
        _settings = settings;
        BuildUi();
    }

    private void BuildUi()
    {

        Add(GetAccountGroup());

        PreferencesGroup GetAccountGroup()
        {
            var accountGroup = new PreferencesGroup();
            accountGroup.SetTitle("Account");
            accountGroup.SetDescription("Change minecraft account settings");

            var testRow = new EntryRow();
            testRow.SetTitle("Offline Username");
            testRow.SetText(_settings.OfflineUsername);
            testRow.SetShowApplyButton(true);
            testRow.OnApply += (sender, args) =>
            {
                _settings.OfflineUsername = testRow.GetText();
                Program.WriteSettings(_settings);
            };
            testRow.OnEntryActivated += (sender, args) =>
            {
                _settings.OfflineUsername = testRow.GetText();
                Program.WriteSettings(_settings);
            };
            
            accountGroup.Add(testRow);
            
            return accountGroup;
        }
        
    }
    
}