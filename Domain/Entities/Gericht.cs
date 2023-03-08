using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;

namespace Domain.Entities
{
    public class Gericht : BaseEntity
    {
        private double carbs;
        private double fett;
        private double kcal;
        private string name;
        private double protein;

        public Gericht(Session session) : base(session) { }

        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [Association]
        public XPCollection<Zutat> Zutaten => GetCollection<Zutat>(nameof(Zutaten));

        [ModelDefault("AllowEdit", "false")]
        [ModelDefault("DisplayFormat", "{0:n0}")]
        public double Kcal
        {
            get => kcal;
            set => SetPropertyValue(nameof(Kcal), ref kcal, value);
        }

        [ModelDefault("AllowEdit", "false")]
        [ModelDefault("DisplayFormat", "{0:n0} g")]
        public double Protein
        {
            get => protein;
            set => SetPropertyValue(nameof(Protein), ref protein, value);
        }

        [ModelDefault("AllowEdit", "false")]
        [ModelDefault("DisplayFormat", "{0:n0} g")]
        public double Fett
        {
            get => fett;
            set => SetPropertyValue(nameof(Fett), ref fett, value);
        }

        [ModelDefault("AllowEdit", "false")]
        [ModelDefault("DisplayFormat", "{0:n0} g")]
        public double Carbs
        {
            get => carbs;
            set => SetPropertyValue(nameof(Carbs), ref carbs, value);
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            GibAlleNährstoffe();
        }

        public void GibAlleNährstoffe()
        {
            Kcal    = GibKcal();
            Protein = GibProtein();
            Fett    = GibFett();
            Carbs   = GibtCarbs();
        }

        private double GibtCarbs() => Zutaten.Sum(gg => gg.Menge / 100 * gg.Nahrungsmittel.Carbs);

        private double GibFett() => Zutaten.Sum(gg => gg.Menge / 100 * gg.Nahrungsmittel.Fett);

        private double GibProtein() => Zutaten.Sum(gg => gg.Menge / 100 * gg.Nahrungsmittel.Protein);

        private double GibKcal() => Zutaten.Sum(gg => gg.Menge / 100 * gg.Nahrungsmittel.Kcal);
    }
}