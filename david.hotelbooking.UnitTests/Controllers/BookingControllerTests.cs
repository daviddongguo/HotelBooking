using david.hotelbooking.api.Controllers;
using david.hotelbooking.api.SchedulerModels;
using david.hotelbooking.domain;
using david.hotelbooking.domain.Entities;
using david.hotelbooking.domain.Entities.Hotel;
using david.hotelbooking.domain.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace david.hotelbooking.UnitTests.Controllers
{
    [TestFixture]
    public class BookingControllerTests
    {
        private BookingsController _controller;
        private Mock<IBookingService> _mockService;
        private IQueryable<Booking> _bookingsList;
        private Booking _firstBooking;
        //private Booking _secondBooking;
        //private readonly JsonDeserializer _serializer = new JsonDeserializer();


        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IBookingService>();
            DateTime firstDay = DateTime.Now.Date + new TimeSpan(14, 0, 0);
            _firstBooking =
                new Booking { Id = 1, GuestId = 1, Guest = new Guest { Id = 1, Name = "Alice" }, RoomId = 1, Room = new Room { Id = 1, RoomNumber = "801" }, FromDate = firstDay.AddDays(0), ToDate = firstDay.AddDays(1).Date + new TimeSpan(10, 0, 0) };
            //_secondBooking =
                //new Booking { Id = 0, GuestId = 2, Guest = new Guest { Id = 2, Name = "Alex" }, RoomId = 2, Room = new Room { Id = 2, RoomNumber = "222" }, FromDate = firstDay.AddDays(0), ToDate = firstDay.AddDays(1).Date + new TimeSpan(10, 0, 0) };

            _bookingsList = new List<Booking>
            {
                _firstBooking
            }.AsQueryable();
            _controller = new BookingsController(_mockService.Object);
        }


        [TestCase(200)]
        public void GetAllBookings_WhenDatabaseIsEmpty_ReturnEmpty(int expectedStatusCode)
        {
            _mockService.Setup(s => s.GetAllBookings()).Returns(Task.FromResult(new List<Booking>().AsQueryable()));
            var response = _controller.GetAllBookings().GetAwaiter().GetResult();

            // Assert
            var result = response.Result as ObjectResult;
            Assert.That(result.StatusCode == expectedStatusCode);
            Utilities.PrintOut(response);
        }

        [TestCase(404)]
        public void GetAllBookings_WhenDatabaseError_ReturnNotFound(int expectedStatusCode)
        {
            _mockService.Setup(s => s.GetAllBookings()).Throws(new Exception());
            var response = _controller.GetAllBookings().GetAwaiter().GetResult();

            // Assert
            var result = response.Result as ObjectResult;
            Assert.That(result.StatusCode == expectedStatusCode);
            var resultValue = result.Value as ServiceResponse<List<BookingEvent>>;
            Assert.That(resultValue.Message, Is.Not.Null);
            Assert.That(resultValue.Data, Is.Null);
            Utilities.PrintOut(response);
        }

        [TestCase(3, 200)]
        [TestCase(9999, 200)]
        public void GetAllBookings_ReturnBookings(int roomId, int expectedStatusCode)
        {
            _bookingsList.ToList()[0].RoomId = roomId;
            _mockService.Setup(s => s.GetAllBookings()).Returns(Task.FromResult(_bookingsList));
            var response = _controller.GetAllBookings().GetAwaiter().GetResult();

            // Assert
            var result = response.Result as ObjectResult;
            Assert.That(result.StatusCode == expectedStatusCode);
            var resultValue = result.Value as ServiceResponse<List<BookingEvent>>;
            Assert.That(resultValue.Message, Is.Null);
            Assert.That(resultValue.Data.Count >= 1);
            Assert.That(resultValue.Data.FirstOrDefault().Resource.Equals(roomId.ToString()));
            Utilities.PrintOut(result);
        }

        [TestCase(1, 200)]
        [TestCase(3, 404)]
        public void GetBookingById_(int bookingId, int expectedStatusCode)
        {
            _mockService.Setup(s => s.GetBookingById(1)).Returns(Task.FromResult(_bookingsList.ToList()[0]));
            var response = _controller.GetBookingById(bookingId).GetAwaiter().GetResult();

            Assert.That(response, Is.InstanceOf(typeof(ActionResult<ServiceResponse<BookingEvent>>)));
            var result = response.Result as ObjectResult;
            Assert.That(result.StatusCode == expectedStatusCode);
            var resultValue = result.Value as ServiceResponse<BookingEvent>;
            Utilities.PrintOut(result);
        }


        [TestCase(1, 404)]
        public void GetBookingById_WhenDatabaseThrowsException(int bookingId, int expectedStatusCode)
        {
            _mockService.Setup(s => s.GetBookingById(1)).Throws<Exception>();
            var response = _controller.GetBookingById(bookingId).GetAwaiter().GetResult();

            Assert.That(response, Is.InstanceOf(typeof(ActionResult<ServiceResponse<BookingEvent>>)));
            var result = response.Result as ObjectResult;
            Assert.That(result.StatusCode == expectedStatusCode);
            var resultValue = result.Value as ServiceResponse<BookingEvent>;
            Utilities.PrintOut(result);
        }

        [TestCase("1991", "Alice@ho.t", "2020-1-1", "2020-1-2", 201)]
        public void AddBooking(string roomId, string guestEmail, string fromDateStr, string toDateStr, int expectedStatusCode)
        {
            var ev = new BookingEvent
            {
                Resource = roomId,
                Text = guestEmail,
                Start = fromDateStr,
                End = toDateStr,
            };
            _mockService.Setup(s => s.GetGuestByEmail(guestEmail)).Returns(
                Task.FromResult(new Guest { Id = 1, Email = guestEmail }));
            _mockService.Setup(s => s.AddBooking(It.IsAny<Booking>())).Returns(
                Task.FromResult(new Booking { Guest = new Guest { Email = guestEmail } }));

            var response = _controller.AddBooking(ev).GetAwaiter().GetResult();

            // Assert
            var result = response.Result as ObjectResult;
            Assert.That(result.StatusCode == expectedStatusCode);
            var resultValue = result.Value as ServiceResponse<Booking>;
            Assert.That(resultValue.Data.RoomId.ToString(), Is.EqualTo(roomId));
        }

        [TestCase(404)]
        public void DeleteBooking_WhenNoBooking_ReturnNotFound(int expectedStatusCode)
        {
            _mockService.Setup(s => s.GetBookingById(1)).Returns(Task.FromResult((Booking)null));

            var response = _controller.DeleteBooking(1).GetAwaiter().GetResult();

            // Assert
            var result = response as NotFoundResult;
            Assert.That(result.StatusCode == expectedStatusCode);
        }

        [TestCase(204)]
        public void DeleteBookin(int expectedStatusCode)
        {
            _mockService.Setup(s => s.GetBookingById(It.IsAny<int>())).Returns(Task.FromResult(new Booking()));
            _mockService.Setup(s => s.DeleteBooking(1)).Returns(Task.CompletedTask);

            var response = _controller.DeleteBooking(1).GetAwaiter().GetResult();

            // Assert
            var result = response as NoContentResult;
            Assert.That(result.StatusCode == expectedStatusCode);
        }

        [TestCase(500)]
        public void DeleteBooking_WhenDBError_Returns500(int expectedStatusCode)
        {
            _mockService.Setup(s => s.GetBookingById(It.IsAny<int>())).Returns(Task.FromResult(new Booking()));
            _mockService.Setup(s => s.DeleteBooking(It.IsAny<int>())).Throws(new Exception());

            var response = _controller.DeleteBooking(1).GetAwaiter().GetResult();

            // Assert
            var result = response as ObjectResult;
            Assert.That(result.StatusCode == expectedStatusCode);
        }

    }
}
