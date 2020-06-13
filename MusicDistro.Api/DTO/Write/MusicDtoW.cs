using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicDistro.Api.DTO.Write
{
    public class MusicDtoW
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ArtistId { get; set; }

        public class Validator : AbstractValidator<MusicDtoW>
        {
            public Validator()
            {
                RuleFor(m => m.Name)
                    .NotEmpty()
                    .MaximumLength(50)
                    .WithMessage("Name length must be less than 50");

                RuleFor(m => m.ArtistId)
                    .NotEmpty()
                    .WithMessage($"Arist id must not be 0");
            }
        }
    }
}
