using Volo.Abp.Settings;

namespace Argus.WMS.Settings;

public class WMSSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(WMSSettings.MySetting1));
    }
}
