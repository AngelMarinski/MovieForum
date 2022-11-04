using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieForum.Services.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required, DataType(DataType.Password)]
        [StringLength(int.MaxValue, MinimumLength = 8)]
        public string NewPassword { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

        public bool IsSuccess { get; set; }
    }
}
