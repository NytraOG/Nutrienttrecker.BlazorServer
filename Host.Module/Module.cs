using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Dashboards;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Objects;
using DevExpress.ExpressApp.Office;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using Updater = Host.Module.DatabaseUpdate.Updater;

namespace Host.Module;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
public sealed class HostModule : ModuleBase
{
    public HostModule()
    {
        //
        // HostModule
        //
        AdditionalExportedTypes.Add(typeof(ModelDifference));
        AdditionalExportedTypes.Add(typeof(ModelDifferenceAspect));
        AdditionalExportedTypes.Add(typeof(BaseObject));
        AdditionalExportedTypes.Add(typeof(FileData));
        AdditionalExportedTypes.Add(typeof(FileAttachmentBase));
        RequiredModuleTypes.Add(typeof(SystemModule));
        RequiredModuleTypes.Add(typeof(SecurityModule));
        RequiredModuleTypes.Add(typeof(BusinessClassLibraryCustomizationModule));
        RequiredModuleTypes.Add(typeof(ConditionalAppearanceModule));
        // RequiredModuleTypes.Add(typeof(DashboardsModule));
        // RequiredModuleTypes.Add(typeof(OfficeModule));
        // RequiredModuleTypes.Add(typeof(ReportsModuleV2));
        RequiredModuleTypes.Add(typeof(ValidationModule));
        RequiredModuleTypes.Add(typeof(Domain.Module));
    }

    public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
    {
        ModuleUpdater updater = new Updater(objectSpace, versionFromDB);
        return new[] { updater };
    }

    public override void Setup(XafApplication application) => base.Setup(application);

    // Manage various aspects of the application UI and behavior at the module level.
    public override void CustomizeTypesInfo(ITypesInfo typesInfo)
    {
        base.CustomizeTypesInfo(typesInfo);
        CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
    }
}