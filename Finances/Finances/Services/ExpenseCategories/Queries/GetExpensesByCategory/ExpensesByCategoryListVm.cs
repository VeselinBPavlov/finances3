namespace Finance.Application.ExpenseCategories.Queries.GetExpensesByCategory
{
    using System.Collections.Generic;

    public class ExpensesByCategoryListVm
    {
        public IList<ExpenseByCategoryVm> ExpenseCategories { get; set; }

        public decimal Totals { get; set; }
    }
}
