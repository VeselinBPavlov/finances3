namespace Finance.Application.ExpenseCategories.Queries.GetExpensesByCategory
{
    public class ExpenseByCategoryVm
    {
        public int Id { get; set; } = default!;

        public string Name { get; set; } = default!;

        public int TypeId { get; set; }

        public string TypeDescription { get; set; } = default!;

        public decimal Sum { get; set; }
    }
}
