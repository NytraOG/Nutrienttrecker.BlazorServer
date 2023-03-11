using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using Domain.Entities;
using Domain.ViewModels;

namespace Host.Module.Controllers;

public class AppendKcalController : ObjectViewController<DetailView, Tag>
{
    private readonly PopupWindowShowAction action;

    public AppendKcalController() => action = new PopupWindowShowAction(this, nameof(AppendKcalController), PredefinedCategory.Edit)
    {
        Caption = "Append Kcal"
    };

    private void ActionOnCustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
    {
        if (Application.CreateObjectSpace(typeof(AppendKcalViewModel)) is not NonPersistentObjectSpace npSpace)
            return;

        var vm   = npSpace.CreateObject<AppendKcalViewModel>();
        var view = Application.CreateDetailView(npSpace, vm);

        e.View = view;
    }

    private void ActionOnExecute(object sender, PopupWindowShowActionExecuteEventArgs e)
    {
        if(e.PopupWindowViewCurrentObject is not AppendKcalViewModel vm)
            return;

        ViewCurrentObject.GesamtKcal += vm.Kcal;
    }

    protected override void OnActivated()
    {
        base.OnActivated();

        action.CustomizePopupWindowParams += ActionOnCustomizePopupWindowParams;
        action.Execute                    += ActionOnExecute;
    }

    protected override void OnDeactivated()
    {
        action.CustomizePopupWindowParams -= ActionOnCustomizePopupWindowParams;
        action.Execute                    -= ActionOnExecute;

        base.OnDeactivated();
    }
}