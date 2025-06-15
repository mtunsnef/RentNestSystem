using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.DTOs
{
    public class AccountRegisterDto
    {
        [Required(ErrorMessage = "Tên người dùng không được để trống")]
        public required string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email không được để trống")]

        public required string Email { get; set; }

        [MinLength(6, ErrorMessage = "Vui lòng nhập mật khẩu trên 6 ký tự")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public required string Password { get; set; }

        [MinLength(6, ErrorMessage = "Vui lòng nhập mật khẩu trên 6 ký tự")]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp")]
        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn vai trò của bạn")]
        public required string Role { get; set; }

        [Required(ErrorMessage = "Họ không được để trống")]
        public required string FirstName { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public required string PhoneNumber { get; set; }
    }
}
