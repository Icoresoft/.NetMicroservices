using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Order.Commands.Checkout
{
    public class CheckoutCommandValidator:AbstractValidator<CheckoutCommand>
    {
        public CheckoutCommandValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("UserName must Not be Empty")
                .NotNull().WithMessage("UserName must Not be Null");

            RuleFor(p => p.TotalPrice)
                .GreaterThan(0).WithMessage("Order can not be less than or Equal 0");
        }
    }
}
