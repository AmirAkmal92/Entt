
namespace Bespoke.PosEntt.Integrations.Transforms
{
    public partial class IposDepositPaymentToSnbWebApi
    {


        partial void BeforeTransform(Bespoke.PosEntt.IposDepositPayments.Domain.IposDepositPayment item, Bespoke.PosEntt.Adapters.SnbWebApi.PostDepositRequest destination)
        {

        }

        partial void AfterTransform(Bespoke.PosEntt.IposDepositPayments.Domain.IposDepositPayment item, Bespoke.PosEntt.Adapters.SnbWebApi.PostDepositRequest destination)
        {
            if (item.PaymentMethod == "Cash")
            {
                destination.Body.Comment = $"Receipt No : {item.ReceiptNo}";
            }
            else
            {
                var checkNo = item.CheckNo;
                if (checkNo.Length >= 6)
                    destination.Body.Comment = $"Cheque No: {checkNo.Substring(checkNo.Length - 6, 6)} | Receipt No : {item.ReceiptNo}";
            }
        }

    }
}
