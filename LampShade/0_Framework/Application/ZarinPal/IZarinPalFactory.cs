using _0_Framework.Application.ZarinPal;

namespace _0_Framework.Application.ZarinPal
{
    public interface IZarinPalFactory
    {
        /// <summary>
        /// ایجاد درخواست پرداخت در زرین‌پال
        /// </summary>
        /// <param name="amount">
        /// مبلغ به تومان، رشته‌ای که می‌تواند شامل جداکننده‌ی هزارگان (،) باشد.
        /// </param>
        /// <param name="mobile">شماره موبایل کاربر (اختیاری)</param>
        /// <param name="email">ایمیل کاربر (اختیاری)</param>
        /// <param name="description">توضیحات سفارش</param>
        /// <param name="orderId">شناسه داخلی سفارش در سیستم شما</param>
        /// <returns>شیء PaymentResponse با اطلاعات Authority و لینک پرداخت</returns>
        ZarinPalPaymentResponse CreatePaymentRequest(
            string amount,
            string mobile,
            string email,
            string description,
            long orderId);

        /// <summary>
        /// تأیید وضعیت تراکنش پس از بازگشت از درگاه
        /// </summary>
        /// <param name="authority">کد Authority برگشتی از CreatePaymentRequest</param>
        /// <param name="amount">
        /// مبلغ همان‌گونه که در درخواست اولیه فرستاده شده است (رشته‌ای ممکن است دارای جداکننده‌ی هزارگان باشد)
        /// </param>
        /// <returns>شیء VerificationResponse با وضعیت پرداخت</returns>
        VerificationResponse CreateVerificationRequest(string authority,string amount);

        string GetStartPayUrl(string authority);
    }
}