using FluentValidation;
using FreeCourse.Web.Models.Catalog;

namespace FreeCourse.Web.Validators
{
    public class CourseCreateInputValidator:AbstractValidator<CourseCreateInput>
    {
        public CourseCreateInputValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().WithMessage("Name field can not be empty");
            RuleFor(x=>x.Description).NotEmpty().WithMessage("Description field can not be empty");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("Can not be empty");

            //ScalePrecision method means, there will be 2 digits after comma symbol and 4 digits before comma symbol, 6 in total: 2387.99
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price can not be empty").ScalePrecision(2, 6).WithMessage("Wrong number");    

            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("A category must be chosen");
        }
    }
}
