using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using Microsoft.AspNetCore.Components;

namespace Host.Blazor.Server.Components.Adapter;

public class NutrientDonuAdapter : IComponentAdapter, IComplexControl
{
    private XafApplication application;
    private RenderFragment component;
    private IObjectSpace   space;

    public void Refresh() { }

    public void Setup(IObjectSpace objectSpace, XafApplication application)
    {
        this.application = application;
        space            = objectSpace;
    }

    public RenderFragment ComponentContent => component ??= builder =>
    {
        builder.OpenComponent<NutrientDonut>(0);
        builder.AddAttribute(1, nameof(NutrientDonut.ObjectSpace), space);
        builder.CloseComponent();
    };

    public object GetValue() => null;

    public void SetValue(object value) { }

    public event EventHandler ValueChanged;
}