﻿using david.hotelbooking.UnitTests.Services;
using NUnit.Framework;
using System;
using System.Text.Json;

namespace david.hotelbooking.domain.Services.Tests
{
    [TestFixture()]
    public class RoomserviceTests
    {
        private BookingService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new BookingService(new RemoteMySqlDbcontextFactory().GetBookingContext());
        }

        [Test()]
        public void GetAllRoomsTest()
        {
            var result = _service.GetAllRooms().GetAwaiter().GetResult();

            // Assert
            PrintOut(result);
            Assert.That(result, Is.Not.Empty);
        }

        private void PrintOut(Object obj)
        {
            System.Console.WriteLine(Utilities.PrettyJson(JsonSerializer.Serialize(obj)));

        }


    }
}