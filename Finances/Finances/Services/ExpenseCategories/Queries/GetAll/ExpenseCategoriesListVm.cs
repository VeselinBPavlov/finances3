using AutoMapper;
using Finances.Common.Mapping;
using Finances.Models;

namespace Finance.Application.ExpenseCategories.Queries.GetAll
{
    public class ExpenseCategoriesListVm : IMapFrom<ExpenseCategory>
    {
        public string Id { get; set; } = default!;

        public string Name { get; set; } = default!;

        public int TypeId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ExpenseCategory, ExpenseCategoriesListVm>();
        }
    }
}
