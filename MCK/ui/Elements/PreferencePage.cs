using Adw;

namespace MCK.ui.Elements;

public class PreferencePage : PreferencesPage
{
    public PreferencePage()
    {
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
            
            var testRow = new ActionRow();
            testRow.SetSubtitle("TheCuddlyBear");
            testRow.SetTitle("Current Offline Account");
            testRow.SetCssClasses(new[] {"property"});
            
            accountGroup.Add(testRow);
            
            return accountGroup;
        }
        
    }
    
}