namespace Finances.Services.Incomes.Commands
{
    using Finances.Models;
    using FluentValidation;
    using MediatR;

    public class CreateIncomeCommand : IRequest
    {
        public string? Merchant { get; set; }

        public DateTime Date { get; set; }

        public decimal Total { get; set; }

        public string? Note { get; set; }

        public int CategoryId { get; set; }

        public string UserId { get; set; } = default!;

    }

    public class CreateIncomeCommandValidator : AbstractValidator<CreateIncomeCommand>
    {
        public CreateIncomeCommandValidator()
        {
            RuleFor(e => e.Merchant)
                .MaximumLength(100)
                .WithMessage("Invalid Mechant");

            RuleFor(e => e.Note)
                .MaximumLength(1000)
                .WithMessage("Invalid Note");

            RuleFor(e => e.Date)
                .NotEmpty()
                .Must(BeValidDate)
                .WithMessage("Invaid Date");

            RuleFor(e => e.Total)
                .GreaterThan(0)
                .WithMessage("Invalid Total");


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

    public class CreateIncomeCommandHandler : IRequestHandler<CreateIncomeCommand, Unit>
    {
        private const string User = "User";
        private const string Category = "Category";
        private const string ErrorMessage = "Cannot create entity of type Income, because {0} does not exsists.";

        private readonly IFinanceDbContext context;

        public CreateIncomeCommandHandler(IFinanceDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
        {
            var user = await context.FinanceUsers.FindAsync(request.UserId);

            if (user == null)
            {
                throw new CreateFailureException(User, request.UserId, string.Format(ErrorMessage, User));
            }

            var category = await context.IncomeCategories.FindAsync(request.CategoryId);

            if (category == null)
            {
                throw new CreateFailureException(Category, request.CategoryId, string.Format(ErrorMessage, Category));
            }

            var income = new Income
            {
                Merchant = request.Merchant,
                Date = DateTime.Parse(request.Date),
                Note = request.Note,
                Total = request.Total,
                UserId = request.UserId,
                CategoryId = request.CategoryId,
                CreatedOn = DateTime.UtcNow
            };

            context.Incomes.Add(income);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
