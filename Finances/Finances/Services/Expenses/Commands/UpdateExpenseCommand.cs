﻿using Finances.Common.Exceptions;
using Finances.Common.Interfaces;
using Finances.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Finance.Application.Expenses.Commands.Update
{
    public class UpdateExpenseCommand : IRequest<int>
    {
        public int Id { get; set; }

        public string Merchant { get; set; } = default!;

        public string Date { get; set; } = default!;

        public decimal Total { get; set; }

        public string Note { get; set; } = default!;

        public int CategoryId { get; set; } = default!;

        public string UserId { get; set; } = default!;
    }

    public class UpdateExpenseCommandValidator : AbstractValidator<UpdateExpenseCommand>
    {
        public UpdateExpenseCommandValidator()
        {
            RuleFor(e => e.Id)
                .NotEmpty()
                .WithMessage("Id is required");

            RuleFor(e => e.Merchant)
                .MaximumLength(100)
                .WithMessage("Invalid merchant");

            RuleFor(e => e.Note)
                .MaximumLength(1000)
                .WithMessage("Invalid note");

            RuleFor(e => e.Date)
                .NotEmpty()
                .Must(BeValidDate)
                .WithMessage("Invalid date");

            RuleFor(e => e.Total)
                .GreaterThan(0.00M)
                .WithMessage("Invalid total");


            RuleFor(e => e.CategoryId)
                .NotEmpty()
                .WithMessage("Category Id is required");


            RuleFor(e => e.UserId)
                .NotEmpty()
                .WithMessage("User Id is required");
        }

        private bool BeValidDate(string date)
        {
            var parsedDate = new DateTime();

            if (DateTime.TryParse(date, out parsedDate))
            {
                return true;
            }

            return false;
        }
    }

    public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, int>
    {
        private readonly IApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public UpdateExpenseCommandHandler(IApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<int> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = await this.context.Expenses.FindAsync(request.Id);

            if (expense == null)
                throw new NotFoundException(nameof(Expense), request.Id);

            var user = await this.userManager.FindByIdAsync(request.UserId);

            if (user == null)
                throw new NotFoundException(nameof(ApplicationUser), request.Id);

            var category = await this.context.ExpenseCategories.FindAsync(request.CategoryId);

            if (category == null)
                throw new NotFoundException(nameof(ExpenseCategory), request.Id);

            expense.Merchant = request.Merchant;
            expense.Date = DateTime.Parse(request.Date);
            expense.Total = request.Total;
            expense.Note = request.Note;
            expense.UserId = request.UserId;
            expense.CategoryId = request.CategoryId;

            this.context.Expenses.Update(expense);

            await this.context.SaveChangesAsync(cancellationToken);

            return expense.Id;
        }
    }
}
