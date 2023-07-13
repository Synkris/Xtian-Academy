using Core.Config;
using Core.Database;
using Core.Enum;
using Core.Models;
using Logic.IHelpers;
using Logic.SmtpMailServices;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using Formatting = Newtonsoft.Json.Formatting;

namespace Logic.Helpers
{
    public class PaystackHelper : IPaystackHelper
    {
        private readonly IUserHelper _userHelper;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;
        protected RestRequest request;
        public static string RestUrl = "https://api.paystack.co/";
        private RestClient client;
        static string ApiEndPoint = "";
        private readonly IGeneralConfiguration _generalConfiguration;

        public PaystackHelper(IUserHelper userHelper, IEmailService emailService, AppDbContext context, IGeneralConfiguration generalConfiguration)
        {
            _userHelper = userHelper;
            _emailService = emailService;
            _context = context;
            _generalConfiguration = generalConfiguration;
            client = new RestClient(RestUrl);
        }

        public async Task<PaystackRepsonse> GeneratePaymentParameters(TrainingCourse payData, ApplicationUser user)
        {
            try
            {
                var paystackResponse = MakeOrderPayment(payData, user);
                if (paystackResponse != null)
                {
                    var userPayment = _context.Payments.Where(p => p.InvoiceNumber == paystackResponse.data.reference).FirstOrDefault();
                    Paystack paystack = new Paystack()
                    {
                        PaymentsHistory = userPayment,
                        authorization_url = paystackResponse.data.authorization_url,
                        access_code = paystackResponse.data.access_code,
                        PaymentHistoryId = userPayment.Id,
                        amount = Convert.ToDecimal(payData.Amount),
                        transaction_date = DateTime.Now,
                        reference = paystackResponse.data.reference,
                    };
                    _context.Paystacks.Add(paystack);
                    await _context.SaveChangesAsync();
                    return paystackResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                var toEmail = _generalConfiguration.AdminEmail;
                var subject = "Generate Payment Parameters Method Exception on SmartEnter";
                _emailService.SendEmail(toEmail, subject, ex.Message);
                throw;
            }
        }
        public PaystackRepsonse MakeOrderPayment(TrainingCourse payData, ApplicationUser user)
        {
            try
            {
                PaystackRepsonse paystackRepsonse = null;
                if (payData.Amount > 0)
                {
                    var referenceId = Generate().ToString();
                    var createHistory = new Payments()
                    {
                        UserId = user.Id,
                        CourseId = payData.Id,
                        Source = TransactionType.Paystack,
                        ProveOfPayment = "N/A",
                        Status = PaymentStatus.Pending,
                        Date = DateTime.Now,
                        InvoiceNumber = referenceId,
                    };
                    _context.Payments.Add(createHistory);
                    _context.SaveChanges();

                    
                    
                    long milliseconds = DateTime.Now.Ticks;
                    string testid = milliseconds.ToString();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    ApiEndPoint = "/transaction/initialize";
                    request = new RestRequest(ApiEndPoint, Method.Post);
                    request.AddHeader("accept", "application/json");
                    request.AddHeader("Authorization", "Bearer " + _generalConfiguration.PayStakApiKey);
                    request.AddParameter("reference", referenceId);
                    var total = payData.Amount;
                    request.AddParameter("amount", total * 100);

                    List<CustomeField> myCustomfields = new List<CustomeField>();
                    CustomeField nameCustomeField = new CustomeField()
                    {
                        display_name = "Email",
                        variable_name = "Email",
                        value = user.Email,
                    };
                    myCustomfields.Add(nameCustomeField);


                    Dictionary<string, List<CustomeField>> metadata = new Dictionary<string, List<CustomeField>>();
                    metadata.Add("custom_fields", myCustomfields);
                    var serializedMetadata = JsonConvert.SerializeObject(metadata);
                    request.AddParameter("metadata", serializedMetadata);
                    request.AddParameter("email", user.Email);
                    var serializedRequest = JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    var result = client.ExecuteAsync(request).Result;
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        paystackRepsonse = JsonConvert.DeserializeObject<PaystackRepsonse>(result.Content);
                    }
                }
                return paystackRepsonse;
            }
            catch (Exception ex)
            {
                var toEmail = _generalConfiguration.AdminEmail;
                var subject = "Paystack Response Exception on SmartEnter";
                _emailService.SendEmail(toEmail, subject, ex.Message);
                throw;
            }
        }
        public static int Generate()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(100000000, 999999999);
        }
        public bool GetPaymentResponse(Paystack paystack)
        {
            if(paystack != null)
            {
                if (paystack != null || paystack.PaymentHistoryId > 0)
                {
                    var user = _userHelper.FindByIdAsync(paystack.PaymentsHistory.UserId).Result;
                    var course = _userHelper.GetTrainingCourseById(paystack.PaymentsHistory.CourseId);
                    var completedpayment = VerifyPayment(paystack);

                    string toEmail = _generalConfiguration.AdminEmail;
                    string subject = "PAYMENT NOTIFICATION";
                    string message = "&#8358;" + paystack.amount + " has been credited to your Account by " + user.FirstName +
                         " been payment for " + course.Title + " with Ref No: " + paystack.reference;

                    _emailService.SendEmail(toEmail, subject, message);
                    return true;
                }
            }
                return false;
        }

        public bool CreateSalaryPaymentHistory(Paystack paystack)
        {
            if (paystack != null)
            {
                var reoccuringPayment = _context.ReoccuringPayments.Where(e => e.reference == paystack.reference).Include(u => u.User).FirstOrDefault();
                if(reoccuringPayment != null)
                {
                    var newSalaryHistory = new SalaryRetures()
                    {
                        InvoiceNumber = Generate().ToString(),
                        ReoccuringPaymentsId = reoccuringPayment.MainId,
                        PaymentDate = DateTime.Now,
                    };
                    _context.SalaryRetureHistory.Add(newSalaryHistory);
                    _context.SaveChanges();

                    return true;
                }
            }
            return false;
        }

        public Paystack GetPaystackHistoryByReference(Paystack paystack)
        {
            if(paystack != null)
            {
                var paymentStatus = _context.Paystacks.Where(x => x.reference == paystack.reference).Include(c => c.PaymentsHistory).FirstOrDefault();
                if(paymentStatus != null)
                {
                    return paymentStatus;
                }
            }
            return null;
        }

        public ReoccuringPayments GetReoccuringPaymentsHistoryByReference(Paystack paystack)
        {
            if (paystack != null)
            {
                var paymentStatus = _context.ReoccuringPayments.Where(x => x.reference == paystack.reference).Include(c => c.User).FirstOrDefault();
                if (paymentStatus != null)
                {
                    paymentStatus.IsAuthorized = true;

                    _context.ReoccuringPayments.Update(paymentStatus);
                    _context.SaveChanges();
                    return paymentStatus;
                }
            }
            return null;
        }
        public async Task<PaystackRepsonse> VerifyPayment(Paystack payment)
        {
            PaystackRepsonse paystackRepsonse = null;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ApiEndPoint = "/transaction/verify/" + payment.reference;
                request = new RestRequest(ApiEndPoint, Method.Get);
                request.AddHeader("accept", "application/json");
                request.AddHeader("Authorization", "Bearer " + _generalConfiguration.PayStakApiKey);
                var result = client.ExecuteAsync(request).Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    paystackRepsonse = JsonConvert.DeserializeObject<PaystackRepsonse>(result.Content);
                    var payStack = UpdateResponse(paystackRepsonse);

                }
                return paystackRepsonse;
            }
            catch (Exception ex)
            {
                var toEmail = _generalConfiguration.AdminEmail;
                var subject = "Paystack Main Payment Verification Exception on SmartEnter";
                _emailService.SendEmail(toEmail, subject, ex.Message);
                throw;
            }
        }
        public Paystack UpdateResponse(PaystackRepsonse PaystackRepsonse)
        {
            try
            {
                Paystack paystackEntity = _context.Paystacks.Where(p => p.reference == PaystackRepsonse.data.reference)
                                            ?.Include(s => s.PaymentsHistory)?.OrderBy(s => s.amount)?.FirstOrDefault();
                if (paystackEntity != null)
                {
                    paystackEntity.bank = PaystackRepsonse.data.authorization.bank;
                    paystackEntity.brand = PaystackRepsonse.data.authorization.brand;
                    paystackEntity.card_type = PaystackRepsonse.data.authorization.card_type;
                    paystackEntity.channel = PaystackRepsonse.data.channel;
                    paystackEntity.country_code = PaystackRepsonse.data.authorization.country_code;
                    paystackEntity.currency = PaystackRepsonse.data.currency;
                    paystackEntity.domain = PaystackRepsonse.data.domain;
                    paystackEntity.exp_month = PaystackRepsonse.data.authorization.exp_month;
                    paystackEntity.exp_year = PaystackRepsonse.data.authorization.exp_year;
                    paystackEntity.fees = PaystackRepsonse.data.fees.ToString();
                    paystackEntity.gateway_response = PaystackRepsonse.data.gateway_response;
                    paystackEntity.ip_address = PaystackRepsonse.data.ip_address;
                    paystackEntity.last4 = PaystackRepsonse.data.authorization.last4;
                    paystackEntity.message = PaystackRepsonse.message;
                    paystackEntity.reference = PaystackRepsonse.data.reference;
                    paystackEntity.reusable = PaystackRepsonse.data.authorization.reusable;
                    paystackEntity.signature = PaystackRepsonse.data.authorization.signature;
                    paystackEntity.status = PaystackRepsonse.data.status;
                    paystackEntity.transaction_date = PaystackRepsonse.data.transaction_date;

                    _context.Update(paystackEntity);
                    _context.SaveChanges();

                    var payment = _context.Payments.Where(x => x.InvoiceNumber == PaystackRepsonse.data.reference).FirstOrDefault();
                    if (payment != null)
                    {
                        payment.Status = PaymentStatus.Approved;
                        _context.Payments.Update(payment);
                        _context.SaveChanges();
                    }
                }
                return paystackEntity;
            }
            catch (Exception ex)
            {
                var toEmail = _generalConfiguration.AdminEmail;
                var subject = "Paystack Updating Response Exception on SmartEnter";
                _emailService.SendEmail(toEmail, subject, ex.Message);
                throw;
            }
        }

    }
}
