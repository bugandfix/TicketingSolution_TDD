using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using Moq;
using TicketingSolution.Core.DataServices;
using TicketingSolution.Core.Enums;
using TicketingSolution.Domain;

namespace TicketingSolution.Core
{
    public class Ticket_Booking_Request_Handler_Test
    {
        private readonly TicketBookingRequestHandler _handler;
        private readonly TicketBookingRequest _request;
        private readonly Mock<ITicketBookingService> _ticketBookingServiceMock;
        private List<Ticket> _availableTickets;

        public Ticket_Booking_Request_Handler_Test()
        {
            //Arrange
            _request = new TicketBookingRequest
            {
                Name = "Test Name",
                Family = "Test Family",
                Email = "TestEmail",
                Date = DateTime.Now
            };

            _availableTickets = new List<Ticket>() { new Ticket() { Id = 1 } };
            _ticketBookingServiceMock = new Mock<ITicketBookingService>();
            _ticketBookingServiceMock.Setup(q => q.GetAvailabeTickets(_request.Date))
                .Returns(_availableTickets);

            _handler = new TicketBookingRequestHandler(_ticketBookingServiceMock.Object);

        }

        [Fact]
        public void Should_Return_Ticket_Booking_Response_With_Request_Values()
        {

            //Act
            ServiceBookingResult Result = _handler.BookService(_request);

            //Assert
            //Assert.NotNull(Result);
            //Assert.Equal(Result.Name, BookingRequest.Name);
            //Assert.Equal(Result.Family, BookingRequest.Family);
            //Assert.Equal(Result.Email, BookingRequest.Email);

            //Assert by Shouldly
            Result.ShouldNotBeNull();
            Result.Name.ShouldBe(_request.Name);
            Result.Family.ShouldBe(_request.Family);
            Result.Email.ShouldBe(_request.Email);
        }

        [Fact]
        public void Should_Throw_Exception_For_Null_Request()
        {
            var exception = Should.Throw<ArgumentNullException>(() => _handler.BookService(null));
            exception.ParamName.ShouldBe("bookingRequest");
        }

        [Fact]
        public void Should_Save_Ticket_Booking_Request()
        {
            TicketBooking SavedBooking = null;
            _ticketBookingServiceMock.Setup(x => x.Save(It.IsAny<TicketBooking>()))
                 .Callback<TicketBooking>(booking =>
                 {
                     SavedBooking = booking;
                 });

            _handler.BookService(_request);

            _ticketBookingServiceMock.Verify(x => x.Save(It.IsAny<TicketBooking>()), Times.Once);

            SavedBooking.ShouldNotBeNull();
            SavedBooking.Name.ShouldBe(_request.Name);
            SavedBooking.Family.ShouldBe(_request.Family);
            SavedBooking.Email.ShouldBe(_request.Email);
            SavedBooking.TicketID.ShouldBe(_availableTickets.First().Id);
        }
        
        [Fact]
        public void Should_Not_Save_Ticket_Booking_Request_If_None_Available()
        {
            _availableTickets.Clear();
            _handler.BookService(_request);
            _ticketBookingServiceMock.Verify(x => x.Save(It.IsAny<TicketBooking>()), Times.Never);
        }

        [Theory]
        [InlineData(BookingResultFlag.Failure, false)]
        [InlineData(BookingResultFlag.Success, true)]
        public void Should_Return_SuccessOrFailure_Flag_In_Result(BookingResultFlag bookingSuccessFlag, bool isAvailable)
        {
            if (!isAvailable)
            {
                _availableTickets.Clear();
            }

            var result = _handler.BookService(_request);
            bookingSuccessFlag.ShouldBe(result.Flag);

        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(null, false)]
        public void Should_Return_TicketBookingId_In_Result(int? ticketBookingId, bool isAvailable)
        {
            if (!isAvailable)
            {
                _availableTickets.Clear();
            }
            else
            {
                _ticketBookingServiceMock.Setup(x => x.Save(It.IsAny<TicketBooking>()))
              .Callback<TicketBooking>(booking =>
              {
                  TicketBooking.Id = ticketBookingId.Value;
              });

            }

            var result = _handler.BookService(_request);

            result.TicketBookingId.ShouldBe(ticketBookingId);

        }

    }
}
