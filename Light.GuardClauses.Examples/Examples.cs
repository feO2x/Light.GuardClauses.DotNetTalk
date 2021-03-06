﻿using System;
using System.Web.Mvc;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

// ReSharper disable NotAccessedField.Local

namespace Light.GuardClauses.Examples
{
    public class Account
    {
        public Account(string name)
        {
            name.MustNotBeNullOrWhiteSpace(nameof(name));

            Name = name;
        }

        public string Name { get; }
    }

    public class ConsoleWriter
    {
        private readonly ConsoleColor _color;

        public ConsoleWriter(ConsoleColor color)
        {
            color.MustBeValidEnumValue();

            _color = color;
        }
    }

    public class MovieRatingService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieRatingService(IMovieRepository movieRepository)
        {
            movieRepository.MustNotBeNull(nameof(movieRepository));

            _movieRepository = movieRepository;
        }

        public void RateMovie(Guid movieId, int numberOfStars)
        {
            movieId.MustNotBeEmpty();
            numberOfStars.MustBeIn(Range<int>.FromInclusive(0).ToInclusive(5));

            var movie = _movieRepository.GetById(movieId);
            movie.SetMovieRating(numberOfStars);
        }
    }

    public class ShoppingService
    {
        public CustomerInfo CustomerInfo { get; set; }

        public void CompleteOrder()
        {
            Check.That(CustomerInfo.IsComplete,
                       () => new InvalidOperationException("You cannot complete the order because some customer information is missing."));

            //...
        }
    }

    public class Entity
    {
        public Entity(Guid id)
        {
            id.MustNotBeEmpty(message: "An entity cannot be initialized with an empty GUID.");

            Id = id;
        }

        public Guid Id { get; }
    }

    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            customerRepository.MustNotBeNull(exception: () => new StupidTeamMembersException("Who forgot to register the Customer Repo with the DI container?"));

            _customerRepository = customerRepository;
        }
    }

    public class SomeTests
    {
        [Fact]
        public void ConstructorThrowsExceptionOnNull()
        {
            Action act = () => new Account(null);

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("name");
        }

        [Fact]
        public void ConstructorThrowsOnEmptyString()
        {
            Action act = () => new Account(string.Empty);

            act.ShouldThrow<EmptyStringException>();
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("\t")]
        public void ConstructorThrowsOnWhitespaceString(string invalidString)
        {
            Action act = () => new Account(invalidString);

            act.ShouldThrow<StringIsOnlyWhiteSpaceException>();
        }

        [Fact]
        public void InvalidEnumValue()
        {
            var invalidValue = (ConsoleColor) 15 + 20;

            Action act = () => new ConsoleWriter(invalidValue);

            act.ShouldThrow<EnumValueNotDefinedException>();
        }

        [Fact]
        public void EntityEmptyGuid()
        {
            Action act = () => new Entity(Guid.Empty);

            act.ShouldThrow<EmptyGuidException>()
               .And.Message.Should().Be("An entity cannot be initialized with an empty GUID.");
        }

        [Fact]
        public void ControllerParameterNull()
        {
            Action act = () => new CustomerController(null);

            act.ShouldThrow<StupidTeamMembersException>();
        }
    }
}