using TicketingSolution.Core.Enums;
using TicketingSolution.Domain.BaseModels;

namespace TicketingSolution.Core
{
    public class ServiceBookingResult : ServiceBookingBase
    {
        public BookingResultFlag Flag { get; set; }

        public int? TicketBookingId { get; set; }
    }
}