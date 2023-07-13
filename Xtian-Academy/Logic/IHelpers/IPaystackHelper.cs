using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelpers
{
	public interface IPaystackHelper
	{
        Task<PaystackRepsonse> GeneratePaymentParameters(TrainingCourse payData, ApplicationUser user);
        PaystackRepsonse MakeOrderPayment(TrainingCourse payData, ApplicationUser user);
        bool GetPaymentResponse(Paystack paystack);
        bool CreateSalaryPaymentHistory(Paystack paystack);
        Paystack GetPaystackHistoryByReference(Paystack paystack);
        ReoccuringPayments GetReoccuringPaymentsHistoryByReference(Paystack paystack);
        Task<PaystackRepsonse> VerifyPayment(Paystack payment);
        Paystack UpdateResponse(PaystackRepsonse PaystackRepsonse);
    }
}
