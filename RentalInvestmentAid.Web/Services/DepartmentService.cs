using RentalInvestmentAid.Web.Model;

namespace RentalInvestmentAid.Web.Services
{
    public class DepartmentService
    {
        private List<Department> _departments = new List<Department>
{
    new Department { Name = "Ain", Number = "01" },
    new Department { Name = "Aisne", Number = "02" },
    new Department { Name = "Allier", Number = "03" },
    new Department { Name = "Alpes-de-Haute-Provence", Number = "04" },
    new Department { Name = "Hautes-Alpes", Number = "05" },
    new Department { Name = "Alpes-Maritimes", Number = "06" },
    new Department { Name = "Ardèche", Number = "07" },
    new Department { Name = "Ardennes", Number = "08" },
    new Department { Name = "Ariège", Number = "09" },
    new Department { Name = "Aube", Number = "10" },
    new Department { Name = "Aude", Number = "11" },
    new Department { Name = "Aveyron", Number = "12" },
    new Department { Name = "Bouches-du-Rhône", Number = "13" },
    new Department { Name = "Calvados", Number = "14" },
    new Department { Name = "Cantal", Number = "15" },
    new Department { Name = "Charente", Number = "16" },
    new Department { Name = "Charente-Maritime", Number = "17" },
    new Department { Name = "Cher", Number = "18" },
    new Department { Name = "Corrèze", Number = "19" },
    new Department { Name = "Corse-du-Sud", Number = "2A" },
    new Department { Name = "Haute-Corse", Number = "2B" },
    new Department { Name = "Côte-d'Or", Number = "21" },
    new Department { Name = "Côtes-d'Armor", Number = "22" },
    new Department { Name = "Creuse", Number = "23" },
    new Department { Name = "Dordogne", Number = "24" },
    new Department { Name = "Doubs", Number = "25" },
    new Department { Name = "Drôme", Number = "26" },
    new Department { Name = "Eure", Number = "27" },
    new Department { Name = "Eure-et-Loir", Number = "28" },
    new Department { Name = "Finistère", Number = "29" },
    new Department { Name = "Gard", Number = "30" },
    new Department { Name = "Haute-Garonne", Number = "31" },
    new Department { Name = "Gers", Number = "32" },
    new Department { Name = "Gironde", Number = "33" },
    new Department { Name = "Hérault", Number = "34" },
    new Department { Name = "Ille-et-Vilaine", Number = "35" },
    new Department { Name = "Indre", Number = "36" },
    new Department { Name = "Indre-et-Loire", Number = "37" },
    new Department { Name = "Isère", Number = "38" },
    new Department { Name = "Jura", Number = "39" },
    new Department { Name = "Landes", Number = "40" },
    new Department { Name = "Loir-et-Cher", Number = "41" },
    new Department { Name = "Loire", Number = "42" },
    new Department { Name = "Haute-Loire", Number = "43" },
    new Department { Name = "Loire-Atlantique", Number = "44" },
    new Department { Name = "Loiret", Number = "45" },
    new Department { Name = "Lot", Number = "46" },
    new Department { Name = "Lot-et-Garonne", Number = "47" },
    new Department { Name = "Lozère", Number = "48" },
    new Department { Name = "Maine-et-Loire", Number = "49" },
    new Department { Name = "Manche", Number = "50" },
    new Department { Name = "Marne", Number = "51" },
    new Department { Name = "Haute-Marne", Number = "52" },
    new Department { Name = "Mayenne", Number = "53" },
    new Department { Name = "Meurthe-et-Moselle", Number = "54" },
    new Department { Name = "Meuse", Number = "55" },
    new Department { Name = "Morbihan", Number = "56" },
    new Department { Name = "Moselle", Number = "57" },
    new Department { Name = "Nièvre", Number = "58" },
    new Department { Name = "Nord", Number = "59" },
    new Department { Name = "Oise", Number = "60" },
    new Department { Name = "Orne", Number = "61" },
    new Department { Name = "Pas-de-Calais", Number = "62" },
    new Department { Name = "Puy-de-Dôme", Number = "63" },
    new Department { Name = "Pyrénées-Atlantiques", Number = "64" },
    new Department { Name = "Hautes-Pyrénées", Number = "65" },
    new Department { Name = "Pyrénées-Orientales", Number = "66" },
    new Department { Name = "Bas-Rhin", Number = "67" },
    new Department { Name = "Haut-Rhin", Number = "68" },
    new Department { Name = "Rhône", Number = "69" },
    new Department { Name = "Haute-Saône", Number = "70" },
    new Department { Name = "Saône-et-Loire", Number = "71" },
    new Department { Name = "Sarthe", Number = "72" },
    new Department { Name = "Savoie", Number = "73" },
    new Department { Name = "Haute-Savoie", Number = "74" },
    new Department { Name = "Paris", Number = "75" },
    new Department { Name = "Seine-Maritime", Number = "76" },
    new Department { Name = "Seine-et-Marne", Number = "77" },
    new Department { Name = "Yvelines", Number = "78" },
    new Department { Name = "Deux-Sèvres", Number = "79" },
    new Department { Name = "Somme", Number = "80" },
    new Department { Name = "Tarn", Number = "81" },
    new Department { Name = "Tarn-et-Garonne", Number = "82" },
    new Department { Name = "Var", Number = "83" },
    new Department { Name = "Vaucluse", Number = "84" },
    new Department { Name = "Vendée", Number = "85" },
    new Department { Name = "Vienne", Number = "86" },
    new Department { Name = "Haute-Vienne", Number = "87" },
    new Department { Name = "Vosges", Number = "88" },
    new Department { Name = "Yonne", Number = "89" },
    new Department { Name = "Territoire de Belfort", Number = "90" },
    new Department { Name = "Essonne", Number = "91" },
    new Department { Name = "Hauts-de-Seine", Number = "92" },
    new Department { Name = "Seine-Saint-Denis", Number = "93" },
    new Department { Name = "Val-de-Marne", Number = "94" },
    new Department { Name = "Val-d'Oise", Number = "95" },
    new Department { Name = "Guadeloupe", Number = "971" },
    new Department { Name = "Martinique", Number = "972" },
    new Department { Name = "Guyane", Number = "973" },
    new Department { Name = "La Réunion", Number = "974" },
    new Department { Name = "Mayotte", Number = "976" }
};

        public List<Department> GetDepartments(string search)
        {
            return _departments
                .Where(d => d.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public string GetDepartmentNumber(string name)
        {
            return _departments.FirstOrDefault(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.Number;
        }
    }
}
