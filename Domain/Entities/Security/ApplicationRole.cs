using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;

namespace Domain.Entities.Security;

public class ApplicationRole : PermissionPolicyRole
{
    public ApplicationRole(Session session) : base(session) { }
}