using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

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
    }
}