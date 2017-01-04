
namespace Bespoke.PosEntt.Integrations.Transforms
{
    public partial class PrsDepositToSnbWebApiDeposit
    {


        partial void BeforeTransform(Bespoke.PosEntt.PrsDeposits.Domain.PrsDeposit item, Bespoke.PosEntt.Adapters.SnbWebApi.PostDepositRequest destination)
        {

        }

        partial void AfterTransform(Bespoke.PosEntt.PrsDeposits.Domain.PrsDeposit item, Bespoke.PosEntt.Adapters.SnbWebApi.PostDepositRequest destination)
        {
            if(item.Assignment =="-" || string.IsNullOrWhiteSpace(item.Assignment))
            {
                destination.Body.Comment = $"Receipt No : {item.Text}";
            }
            else
            {
                var assignment = item.Assignment;
                if(assignment.Length >= 6)
                    destination.Body.Comment = $"Cheque No: {assignment.Substring(assignment.Length -6, 6)}|Receipt No : {item.Text}";
            }

        }

    }
}
