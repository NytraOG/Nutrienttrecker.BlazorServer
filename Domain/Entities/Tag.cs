using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Domain.Entities.Security;

namespace Domain.Entities;

[Appearance("GroupVisibility",
            AppearanceItemType.ViewItem,
            nameof(GesamtKcal) + " == 0 AND " + nameof(GesamtProtein) + " == 0 AND " + nameof(GesamtFett) + " == 0",
            TargetItems = nameof(GesamtKcal) + ";" + nameof(GesamtProtein) + ";" + nameof(GesamtFett) + ";" + nameof(GesamtCarbs),
            Visibility = ViewItemVisibility.Hide)]
[Appearance("MachFreeiKcalGrün", AppearanceItemType.ViewItem, nameof(MachFreeKcalGrün), TargetItems = nameof(FreieKcal), FontColor = "green")]
[Appearance("MachFreeiKcalGelb", AppearanceItemType.ViewItem, nameof(MachFreeKcalGelb), TargetItems = nameof(FreieKcal), FontColor = "orange")]
[Appearance("MachFreeiKcalRot", AppearanceItemType.ViewItem, nameof(MachFreeKcalRot), TargetItems = nameof(FreieKcal), FontColor = "red")]
public class Tag : BaseEntity
{
    private DateTime        datum;
    private double          einwaage;
    private double          freieKcal;
    private double          gesamtCarbs;
    private double          gesamtFett;
    private double          gesamtKcal;
    private double          gesamtProtein;
    private ApplicationUser user;

    public Tag(Session session) : base(session) => GegesseneDinge.CollectionChanged += GegesseneDingeOnCollectionChanged;

    [ModelDefault("DisplayFormat", "dd.MM.yyyy")]
    [ModelDefault("EditMask", "dd.MM.yyyy")]
    [Indexed(nameof(User), Unique = true)]
    public DateTime Datum
    {
        get => datum;
        set => SetPropertyValue(nameof(Datum), ref datum, value);
    }

    [RuleRange(0, 999)]
    [ModelDefault("DisplayFormat", "{0:0.00} Kg")]
    public double Einwaage
    {
        get => einwaage;
        set => SetPropertyValue(nameof(Einwaage), ref einwaage, value);
    }

    [Association]
    [ModelDefault("AllowEdit", "false")]
    public ApplicationUser User
    {
        get => user;
        set => SetPropertyValue(nameof(User), ref user, value);
    }

    [XafDisplayName("Kcal")]
    [ModelDefault("AllowEdit", "false")]
    [ModelDefault("DisplayFormat", "{0:n0}")]
    public double GesamtKcal
    {
        get => gesamtKcal;
        set => SetPropertyValue(nameof(GesamtKcal), ref gesamtKcal, value);
    }

    [XafDisplayName("Free Kcal")]
    [ModelDefault("AllowEdit", "false")]
    [ModelDefault("DisplayFormat", "{0:n0}")]
    public double FreieKcal
    {
        get => freieKcal;
        set => SetPropertyValue(nameof(FreieKcal), ref freieKcal, value);
    }

    [XafDisplayName("Protein")]
    [ModelDefault("AllowEdit", "false")]
    [ModelDefault("DisplayFormat", "{0:n0} g")]
    public double GesamtProtein
    {
        get => gesamtProtein;
        set => SetPropertyValue(nameof(GesamtProtein), ref gesamtProtein, value);
    }

    [XafDisplayName("Fett")]
    [ModelDefault("AllowEdit", "false")]
    [ModelDefault("DisplayFormat", "{0:n0} g")]
    public double GesamtFett
    {
        get => gesamtFett;
        set => SetPropertyValue(nameof(GesamtFett), ref gesamtFett, value);
    }

    [XafDisplayName("Carbs")]
    [ModelDefault("AllowEdit", "false")]
    [ModelDefault("DisplayFormat", "{0:n0} g")]
    public double GesamtCarbs
    {
        get => gesamtCarbs;
        set => SetPropertyValue(nameof(GesamtCarbs), ref gesamtCarbs, value);
    }

    [MemberDesignTimeVisibility(false)]
    public bool MachFreeKcalGrün => TodaysCalorieIntake - GesamtKcal > 1000;

    [MemberDesignTimeVisibility(false)]
    public bool MachFreeKcalGelb => TodaysCalorieIntake - GesamtKcal > 50 && TodaysCalorieIntake - GesamtKcal < 1000;

    [MemberDesignTimeVisibility(false)]
    public bool MachFreeKcalRot => TodaysCalorieIntake - GesamtKcal <= 50;

    [MemberDesignTimeVisibility(false)]
    public double TodaysCalorieIntake => User.IntakeConfigurations
                                             .First(c => c.Active)
                                             .GetCalorieIntakeByDayOfWeek(Datum.DayOfWeek);

    [Association]
    public XPCollection<Gegessenes> GegesseneDinge => GetCollection<Gegessenes>();

    protected override void OnSaving()
    {
        base.OnSaving();

        GibAlleNährstoffe();

        FreieKcal = TodaysCalorieIntake - GesamtKcal;
    }

    public override void AfterConstruction()
    {
        base.AfterConstruction();

        Datum = DateTime.Today;

        if (SecuritySystem.CurrentUser is ApplicationUser applicationUser)
            User = Session.GetObjectByKey<ApplicationUser>(applicationUser.Oid);
    }

    public void GibAlleNährstoffe()
    {
        GesamtKcal    = GibKcal();
        GesamtProtein = GibProtein();
        GesamtFett    = GibFett();
        GesamtCarbs   = GibCarbs();
    }

    private double GibFett() => GegesseneDinge.Sum(gg => gg.Menge / 100 * gg.Nahrungsmittel.Fett);

    private double GibProtein() => GegesseneDinge.Sum(gg => gg.Menge / 100 * gg.Nahrungsmittel.Protein);

    private double GibKcal() => GegesseneDinge.Sum(gg => gg.Menge / 100 * gg.Nahrungsmittel.Kcal);

    private double GibCarbs() => GegesseneDinge.Sum(gg => gg.Menge / 100 * gg.Nahrungsmittel.Carbs);

    private void GegesseneDingeOnCollectionChanged(object sender, XPCollectionChangedEventArgs e)
    {
        if (e.CollectionChangedType == XPCollectionChangedType.AfterRemove)
            GibAlleNährstoffe();
    }

    public override string ToString() => $"{Datum:dd.MM.yyyy}";
}