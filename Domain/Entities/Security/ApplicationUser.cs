using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;

namespace Domain.Entities.Security;

public class ApplicationUser : PermissionPolicyUser
{
    public ApplicationUser(Session session) : base(session) { }

    [Association]
    public XPCollection<Tag> Tage => GetCollection<Tag>();

    [Association]
    public XPCollection<WeeklyIntakeConfiguration> IntakeConfigurations => GetCollection<WeeklyIntakeConfiguration>();
}