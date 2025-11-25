using System;
using System.ComponentModel.DataAnnotations;

namespace EaseioBackendCodingExercise.Models
{
    public class CreateGUID
    {
        public DateTimeOffset? Expires { get; set; }

        [Required]
        public string User { get; set; } = null!;
    }
}