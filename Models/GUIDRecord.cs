using System;
using System.ComponentModel.DataAnnotations;

namespace EaseioBackendCodingExercise.Models
{
    public class GUIDRecord
    {
        [Key]
        public string Guid { get; set; } = null!;
        public DateTimeOffset Expires { get; set; }
        public string User { get; set; } = null!;
    }
}