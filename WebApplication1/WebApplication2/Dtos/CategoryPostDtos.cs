using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Dtos
{
    public class CategoryPostDtos
    {
        public IFormFile ImageFile { get; set; }
        public string Name { get; set; }
    }

    public class CategoryPostValidator : AbstractValidator<CategoryPostDtos>
    {
        public CategoryPostValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("bos ola bilmez").MaximumLength(50).WithMessage("50 karakterden uzun ola bilmez");
        }
    }
}
