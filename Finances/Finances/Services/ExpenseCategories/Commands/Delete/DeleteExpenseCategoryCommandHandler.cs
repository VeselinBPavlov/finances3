using Finances.Common.Exceptions;
using Finances.Common.Interfaces;
using Finances.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Finance.Application.Expenses.Commands.Delete
{
    public class DeleteExpenseCategoryCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteExpenseCategoryCommandHandler : IRequestHandler<DeleteExpenseCategoryCommand, bool>
    {
        private readonly IApplicationDbContext context;

        public DeleteExpenseCategoryCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(DeleteExpenseCategoryCommand request, CancellationToken cancellationToken)
        {
            var expenseCategory = await this.context.ExpenseCategories
                .Include(ec => ec.Expenses)
                .SingleOrDefaultAsync(ec => ec.Id == request.Id);

            if (expenseCategory == null)
            {
                throw new NotFoundException(nameof(ExpenseCategory), request.Id);
            }

            this.context.ExpenseCategories.Remove(expenseCategory);
            await this.context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
