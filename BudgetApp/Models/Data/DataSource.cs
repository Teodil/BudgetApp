using BudgetApp.Models.Utilitis;

namespace BudgetApp.Models.Data
{
    public class DataSource
    {
        [PoleDescription(name: "Id", isEditable: false)]
        public int Id { get; set; }
        [PoleDescription(name: "Наименование источника")]
        public string Name { get; set; }

        public List<PoleAccordance> PoleAccordances { get; set; }
    }
}