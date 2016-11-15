using System;
using System.Collections;
using Xunit;

namespace Rop.OptionTests
{
    public class OptionEqualityOperatorTests
    {
        [Fact]
        public void GivenTwoOptionsWithDifferentValues_ThenTheyShouldNotBeEqual()
        {
            var first = new Option<int>(5);
            var second = new Option<int>(6);
            Assert.False(first == second);
        }

        [Fact]
        public void GivenTwoOptionsWithEqualValues_ThenTheyShouldBeEqual()
        {
            var first = new Option<int>(5);
            var second = new Option<int>(5);
            Assert.True(first == second);
        }
    }

    public class OptionInequalityOperatorTests
    {
        [Fact]
        public void GivenTwoOptionsWithDifferentValues_ThenTheyShouldNotBeEqual()
        {
            var first = new Option<int>(5);
            var second = new Option<int>(6);
            Assert.True(first != second);
        }

        [Fact]
        public void GivenTwoOptionsWithEqualValues_ThenTheyShouldBeEqual()
        {
            var first = new Option<int>(5);
            var second = new Option<int>(5);
            Assert.False(first != second);
        }
    }

    public class OptionCtorTests
    {
        [Fact]
        public void NullShouldPassThrough()
        {
            string expected = null;
            var option = new Option<string>(expected);
            Assert.Equal(expected, option.Value);
        }

        [Fact]
        public void NullShouldPassThroughCreatingNone()
        {
            var actual = new Option<string>(null);
            var none = Option<string>.None;
            Assert.True(none == actual);
        }

        [Fact]
        public void ValuesShouldPassThrough()
        {
            var expected = Guid.NewGuid().ToString("N");
            var option = new Option<string>(expected);
            Assert.Equal(expected, option.Value);
        }
    }

    public class NoneTests
    {
        [Fact]
        public void None_should_equal_none()
        {
            var expected = Option<string>.None;
            var actual = new Option<string>(null);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void None_should_equal_none2()
        {
            Assert.True(Option<string>.IsNone(Option<string>.None));
        }

        [Fact]
        public void None_should_not_equal_some()
        {
            Assert.False(Option<string>.IsSome(Option<string>.None));
        }
    }

    public class EquatableTests
    {
        private class Dummy
        {
            private int value = 5;
        }

        [Fact]
        public void OptionsWithReferenceTypesCompareAsReferenceEqual()
        {
            var expected = new Dummy();
            var first = Option<Dummy>.Some(expected);
            var second = Option<Dummy>.Some(new Dummy());
            var third = Option<Dummy>.Some(expected);
            Assert.NotEqual(first, second);
            Assert.Equal(first, third);
        }

        [Fact]
        public void OptionsWithValueTypesCompareAsEqual()
        {
            var first = Option<int>.Some(5);
            var second = Option<int>.Some(5);
            Assert.Equal(first, second);
        }
    }

    public class StructuralEquatableTests
    {
        public void ObjectsThatAreNotEqualAreStructurallyEqual()
        {
            var first = new[] { 1, 234, 234, 23 };
            var second = new[] { 1, 234, 234, 23 };
            Assert.False(first.Equals(second));
            Assert.True(StructuralComparisons.StructuralEqualityComparer.Equals(first, second));
        }

        [Fact]
        public void ObjectsThatAreNotOfTheSameOptionTypeAreNotTheSameWhenComparedAsStructuralEquatable()
        {
            var first = Option<int>.Some(5);
            var second = Option<short>.Some(5);
            Assert.False(StructuralComparisons.StructuralEqualityComparer.Equals(first, second));
        }

        [Fact]
        public void ObjectsThatAreStructuralEquatableContinuteToBeAsWellWhenWrappedInAnOption()
        {
            var first = Option<int[]>.Some(new[] { 1, 234, 234, 23 });
            var second = Option<int[]>.Some(new[] { 1, 234, 234, 23 });
            Assert.False(first.Equals(second));
            Assert.True(StructuralComparisons.StructuralEqualityComparer.Equals(first, second));
        }
    }
}