using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using Domain.Entities.Security;

namespace Domain.Entities;

public class WeeklyIntakeConfiguration : BaseEntity
{
    private bool            active;
    private double          dailyMaintenanceIntake;
    private double          friday;
    private string          infoBox;
    private double          monday;
    private double          saturday;
    private double          sunday;
    private double          thursday;
    private double          tuesday;
    private ApplicationUser user;
    private double          wednesday;
    private double          weeklyDeficite;

    public WeeklyIntakeConfiguration(Session session) : base(session) { }

    public double DailyMaintenanceIntake
    {
        get => dailyMaintenanceIntake;
        set => SetPropertyValue(nameof(DailyMaintenanceIntake), ref dailyMaintenanceIntake, value);
    }

    [ImmediatePostData]
    [ModelDefault("AllowEdit", "false")]
    public double WeeklyDeficite
    {
        get => weeklyDeficite;
        set => SetPropertyValue(nameof(WeeklyDeficite), ref weeklyDeficite, value);
    }

    [ImmediatePostData]
    [ModelDefault("RowCount", "4")]
    [ModelDefault("AllowEdit", "false")]
    [Size(SizeAttribute.Unlimited)]
    public string InfoBox
    {
        get => infoBox;
        set => SetPropertyValue(nameof(InfoBox), ref infoBox, value);
    }

    public double Monday
    {
        get => monday;
        set => SetPropertyValue(nameof(Monday), ref monday, value);
    }

    public double Tuesday
    {
        get => tuesday;
        set => SetPropertyValue(nameof(Tuesday), ref tuesday, value);
    }

    public double Wednesday
    {
        get => wednesday;
        set => SetPropertyValue(nameof(Wednesday), ref wednesday, value);
    }

    public double Thursday
    {
        get => thursday;
        set => SetPropertyValue(nameof(Thursday), ref thursday, value);
    }

    public double Friday
    {
        get => friday;
        set => SetPropertyValue(nameof(Friday), ref friday, value);
    }

    public double Saturday
    {
        get => saturday;
        set => SetPropertyValue(nameof(Saturday), ref saturday, value);
    }

    public double Sunday
    {
        get => sunday;
        set => SetPropertyValue(nameof(Sunday), ref sunday, value);
    }

    public bool Active
    {
        get => active;
        set => SetPropertyValue(nameof(Active), ref active, value);
    }

    [Association]
    public ApplicationUser User
    {
        get => user;
        set => SetPropertyValue(nameof(User), ref user, value);
    }

    public double GetCalorieIntakeByDayOfWeek(DayOfWeek dayOfWeek) => dayOfWeek switch
    {
        DayOfWeek.Monday => Monday,
        DayOfWeek.Tuesday => Tuesday,
        DayOfWeek.Wednesday => Wednesday,
        DayOfWeek.Thursday => Thursday,
        DayOfWeek.Friday => Friday,
        DayOfWeek.Saturday => Saturday,
        DayOfWeek.Sunday => Sunday,
        _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null)
    };

    protected override void OnSaving()
    {
        base.OnSaving();

        var monDeficite = dailyMaintenanceIntake - Monday;
        var tueDeficite = dailyMaintenanceIntake - Tuesday;
        var wedDeficite = dailyMaintenanceIntake - Wednesday;
        var thuDeficite = dailyMaintenanceIntake - Thursday;
        var friDeficite = dailyMaintenanceIntake - Friday;
        var satDeficite = dailyMaintenanceIntake - Saturday;
        var sunDeficite = dailyMaintenanceIntake - Sunday;

        WeeklyDeficite = monDeficite + tueDeficite + wedDeficite + thuDeficite + friDeficite + satDeficite + sunDeficite;

        InfoBox = $"At this rate you will lose approximately {WeeklyDeficite / 7700:N}kg fat per Week";
    }
}