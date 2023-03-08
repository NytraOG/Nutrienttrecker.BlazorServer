using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;

namespace Domain.Entities;

[NonPersistent]
public abstract class BaseEntity : XPBaseObject
{
    private DateTime erstelltAm;
    private DateTime geändertAm;
    private Guid     oid;

    protected BaseEntity(Session session) : base(session) { }

    [Key]
    [Persistent]
    [MemberDesignTimeVisibility(false)]
    public Guid Oid
    {
        get => oid;
        set => SetPropertyValue(nameof(Oid), ref oid, value);
    }

    [Persistent]
    [ModelDefault("DisplayFormat", "dd.MM.yyyy HH:mm:ss")]
    [ModelDefault("EditMask", "dd.MM.yyyy HH:mm:ss")]
    [Appearance(nameof(ErstelltAm), Enabled = false)]
    public DateTime ErstelltAm
    {
        get => erstelltAm;
        set => SetPropertyValue(nameof(ErstelltAm), ref erstelltAm, value);
    }

    [Persistent]
    [ModelDefault("DisplayFormat", "dd.MM.yyyy HH:mm:ss")]
    [ModelDefault("EditMask", "dd.MM.yyyy HH:mm:ss")]
    [Appearance(nameof(GeändertAm), Enabled = false)]
    public DateTime GeändertAm
    {
        get => geändertAm;
        set => SetPropertyValue(nameof(GeändertAm), ref geändertAm, value);
    }

    protected override void OnSaving()
    {
        GeändertAm = DateTime.Now;
        base.OnSaving();
    }

    public override void AfterConstruction()
    {
        base.AfterConstruction();
        oid        = XpoDefault.NewGuid();
        erstelltAm = DateTime.Now;
    }
}