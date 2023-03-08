using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;

namespace Domain.Entities
{
    public class Gegessenes : BaseEntity
    {
        private double         menge;
        private Nahrungsmittel nahrungsmittel;
        private Tag            tag;

        public Gegessenes(Session session) : base(session) { }

        [Association]
        public Nahrungsmittel Nahrungsmittel
        {
            get => nahrungsmittel;
            set => SetPropertyValue(nameof(Nahrungsmittel), ref nahrungsmittel, value);
        }

        [ModelDefault("DisplayFormat", "{0:n0} g")]
        public double Menge
        {
            get => menge;
            set => SetPropertyValue(nameof(Menge), ref menge, value);
        }

        [Association]
        public Tag Tag
        {
            get => tag;
            set => SetPropertyValue(nameof(Tag), ref tag, value);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            var space = XPObjectSpace.FindObjectSpaceByObject(this);

            if (space != null)
            {
                Tag ??= space.GetObjectsQuery<Tag>(true)
                             .FirstOrDefault(t => t.Datum == DateTime.Today);
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            //var logEntry = new Log(Session)
            //{
            //    Carbs = Menge / 100 * Nahrungsmittel.Carbs,
            //    Protein = Menge / 100 * Nahrungsmittel.Protein,
            //    Fett = Menge / 100 * Nahrungsmittel.Fett,
            //    Kcal = Menge / 100 * Nahrungsmittel.Kcal,
            //    Menge = Menge,
            //    Zeitpunkt = DateTime.Now,
            //    ObjektDesKonsums = Nahrungsmittel,

            //};

            
        }
    }
}