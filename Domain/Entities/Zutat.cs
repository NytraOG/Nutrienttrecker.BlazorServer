using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;

namespace Domain.Entities
{
    public class Zutat : BaseEntity
    {
        private double         menge;
        private Nahrungsmittel nahrungsmittel;

        public Zutat(Session session) : base(session) { }

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
        public XPCollection<Gericht> Gerichte => GetCollection<Gericht>(nameof(Gerichte));
    }
}