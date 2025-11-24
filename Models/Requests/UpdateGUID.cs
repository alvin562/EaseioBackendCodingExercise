using System;
using System.ComponentModel.DataAnnotations;

namespace EaseioBackendCodingExercise.Models
{
    public class UpdateGUID
    {
        [Required]
        public DateTimeOffset Expires { get; set; }
    }
}