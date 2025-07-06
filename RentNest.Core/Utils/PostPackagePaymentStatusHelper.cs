using RentNest.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Core.Utils
{
    public static class PostPackagePaymentStatusHelper
    {
        public static string ToDbValue(this PostPackagePaymentStatus status)
        {
            return status switch
            {
                PostPackagePaymentStatus.Pending => "P",
                PostPackagePaymentStatus.Completed => "C",
                PostPackagePaymentStatus.Refuned => "R",
                PostPackagePaymentStatus.Inactive => "I",
                _ => throw new ArgumentOutOfRangeException(nameof(status), $"Unsupported payment status: {status}")
            };
        }

        public static PostPackagePaymentStatus FromDbValue(string value)
        {
            return value switch
            {
                "P" => PostPackagePaymentStatus.Pending,
                "C" => PostPackagePaymentStatus.Completed,
                "R" => PostPackagePaymentStatus.Refuned,
                "I" => PostPackagePaymentStatus.Inactive,
                _ => throw new ArgumentOutOfRangeException(nameof(value), $"Unsupported payment status value: {value}")
            };
        }
    }
}
