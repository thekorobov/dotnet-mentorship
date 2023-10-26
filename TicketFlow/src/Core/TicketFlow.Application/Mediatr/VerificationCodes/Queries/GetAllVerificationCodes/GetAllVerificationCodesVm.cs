namespace TicketFlow.Application.Mediatr.VerificationCodes.Queries.GetAllVerificationCodes;

public class GetAllVerificationCodesVm
{
    public IList<VerificationCodeVm> VerificationCodes { get; set; } = new List<VerificationCodeVm>();
}