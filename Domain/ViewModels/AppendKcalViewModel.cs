using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;

namespace Domain.ViewModels;

[DomainComponent]
public class AppendKcalViewModel
{
    [ModelDefault("EditMask", "{F4} Kcal")]
    [ModelDefault("DisplayFormat", "{F4} Kcal")]
    public double Kcal { get; set; }
}