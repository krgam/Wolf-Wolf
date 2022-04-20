namespace TicketSales.User.Services
{
    public class ServiceResponse
    {
        public bool IsSuccess { get => !ErrorCode.HasValue; }

        public ErrorCode? ErrorCode { get; }

        public ServiceResponse()
        {
        }

        public ServiceResponse(ErrorCode errorCode)
        {
            ErrorCode = errorCode;
        }
    }

    public enum ErrorCode
    {
        NotEnoughTicketsForConcert = 1,

        UserNotFound = 2,

        ConcertNotFound = 3,
    }
}
