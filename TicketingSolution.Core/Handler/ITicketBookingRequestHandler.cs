namespace TicketingSolution.Core
{
    public interface ITicketBookingRequestHandler
    {
        ServiceBookingResult BookService(TicketBookingRequest bookingRequest);
    }
}