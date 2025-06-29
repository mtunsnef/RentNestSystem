﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.DTOs
{
    public class AccountLoginDto
    {
        [Required(ErrorMessage = "Tên người dùng hoặc Email không được để trống.")]
        public required string EmailOrUsername { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
