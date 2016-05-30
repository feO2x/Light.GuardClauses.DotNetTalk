using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Examples
{
    public class Account
    {
        public Account(string name)
        {
            name.MustNotBeNull(nameof(name));

            // MustNotBeNull replaces the probably most-common precondition check
            //if (name == null)
            //    throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public string Name { get; }
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
    }
}
