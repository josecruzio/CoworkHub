﻿using CoworkHub.DAL.Interfaces;
using Moq;
using CoworkHub.Contracts.Models;
using System;
using Xunit;
using CoworkHub.OfficeBooker.BusinessRules;

namespace CoworkHub.OfficeBooker.Testes
{
    public class SpaceBookingRequestTest
    {
        private SpaceBookingRequestExecution _executor;
        private SpaceBookingRequest _userRequest;
        private Mock<ISpaceBookingRepository> _SpaceBookingRepositoryMock;

        public SpaceBookingRequestTest()
        {
            //Arrange
            _userRequest = new SpaceBookingRequest
            {
                FirstName = "Jose",
                LastName = "Cruz",
                Email = "jose.cruz@gmail.com",
                DateRequested = new DateTime(2020, 01, 05)
            };

            _SpaceBookingRepositoryMock = new Mock<ISpaceBookingRepository>();

            _executor = new SpaceBookingRequestExecution(_SpaceBookingRepositoryMock.Object);



        }

        [Fact]
        public void ShouldReturnSpaceResultWithRequestValues()
        {


            //Act
            SpaceBookingResult result = _executor.BookSpace(_userRequest);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(_userRequest.FirstName, result.FirstName);
            Assert.Equal(_userRequest.LastName, result.LastName);
            Assert.Equal(_userRequest.Email, result.Email);
            Assert.Equal(_userRequest.DateRequested, result.DateRequested);
        }

        [Fact]
        public void ShouldThrowExceptionIfNullRequest()
        {
            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => _executor.BookSpace(null));

            //Assert
            Assert.Equal("userRequest", exception.ParamName);
        }

        [Fact]
        public void ShouldSaveSpaceBook()
        {
            //Arrange
            SpaceBooking savedSpaceBooking = null;
            _SpaceBookingRepositoryMock.Setup(x => x.Save(It.IsAny<SpaceBooking>()))
                                            .Callback<SpaceBooking>(spaceBooking =>
                                             {
                                                 savedSpaceBooking = spaceBooking;
                                             });
            //Act
            _executor.BookSpace(_userRequest);
            _SpaceBookingRepositoryMock.Verify(x => x.Save(It.IsAny<SpaceBooking>()), Times.Once);

            //Assert
            Assert.NotNull(savedSpaceBooking);
            Assert.Same(_userRequest.FirstName, savedSpaceBooking.FirstName);
            Assert.Same(_userRequest.LastName, savedSpaceBooking.LastName);
            Assert.Same(_userRequest.Email, savedSpaceBooking.Email);


        }

    }
}
