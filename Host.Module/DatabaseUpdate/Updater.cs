using System.Reflection;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Domain.Entities;
using Domain.Entities.Security;
using Host.Module.DatabaseUpdate.Data;
using Newtonsoft.Json;

namespace Host.Module.DatabaseUpdate;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater
{
    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) { }

    public override void UpdateDatabaseAfterUpdateSchema()
    {
        base.UpdateDatabaseAfterUpdateSchema();

        SeedDefaultApplicationSettings();
        //SeedNahrungsmittel();
    }

    private void SeedNahrungsmittel()
    {
        var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

        if (basePath is null)
            throw new DirectoryNotFoundException("Directory weg :C");

        var localPath = new Uri(basePath).LocalPath;

        var dataPath = Path.Combine(localPath, "DatabaseUpdate", "Data");

        var file    = Directory.GetFiles(dataPath, "Nahrungsmittel.json")[0];
        var content = File.ReadAllText(file);

        var nahrungsmittelModels = JsonConvert.DeserializeObject<List<NahrungsmittelModel>>(content);

        foreach (var model in nahrungsmittelModels)
        {
            if (ObjectSpace.GetObjectsQuery<Nahrungsmittel>().Any(n => n.Name == model.Name))
                continue;

            var filedata = ObjectSpace.CreateObject<FileData>();
            filedata.Content  = model.Content;
            filedata.FileName = model.FileName;

            var nahrungsmittel = ObjectSpace.CreateObject<Nahrungsmittel>();
            nahrungsmittel.Name    = model.Name;
            nahrungsmittel.Carbs   = model.Carbs;
            nahrungsmittel.Kcal    = model.Kcal;
            nahrungsmittel.Fett    = model.Fat;
            nahrungsmittel.Protein = model.Protein;
            nahrungsmittel.Datei   = filedata;
        }

        ObjectSpace.CommitChanges();
    }

    private void SeedDefaultApplicationSettings()
    {
        var defaultRole = CreateDefaultRole();

        var userAdmin = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == "Admin");

        if (userAdmin == null)
        {
            userAdmin          = ObjectSpace.CreateObject<ApplicationUser>();
            userAdmin.UserName = "Admin";
            userAdmin.SetPassword("");

            ObjectSpace.CommitChanges();
        }

        var adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "Administrators");

        if (adminRole == null)
        {
            adminRole      = ObjectSpace.CreateObject<PermissionPolicyRole>();
            adminRole.Name = "Administrators";
        }

        adminRole.IsAdministrative = true;
        userAdmin.Roles.Add(adminRole);

        ObjectSpace.CommitChanges();
    }

    public override void UpdateDatabaseBeforeUpdateSchema() => base.UpdateDatabaseBeforeUpdateSchema();

    //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
    //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
    //}
    private PermissionPolicyRole CreateDefaultRole()
    {
        var defaultRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(role => role.Name == "Default");

        if (defaultRole == null)
        {
            defaultRole      = ObjectSpace.CreateObject<PermissionPolicyRole>();
            defaultRole.Name = "Default";

            defaultRole.AddObjectPermissionFromLambda<ApplicationUser>(SecurityOperations.Read, cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "StoredPassword", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
            defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
        }

        return defaultRole;
    }
}