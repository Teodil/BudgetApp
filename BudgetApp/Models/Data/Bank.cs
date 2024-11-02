using BudgetApp.Models.Utilitis;

namespace BudgetApp.Models.Data
{
    public class Bank
    {
        [PoleDescription(name: "Id", isEditable: false)]
        public int Id { get; set; }
        [PoleDescription(name: "Наименование банка")]
        public string Name { get; set; }
    }
}