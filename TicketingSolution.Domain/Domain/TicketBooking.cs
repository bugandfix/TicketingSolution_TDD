using System.ComponentModel.DataAnnotations;
using TicketingSolution.Domain.BaseModels;

namespace TicketingSolution.Domain
{
    public class TicketBooking : ServiceBookingBase
    {
        
        public int Id { get; set; }

        public Ticket Ticket { get; set; }

        public int TicketID { get; set; }
    }
}