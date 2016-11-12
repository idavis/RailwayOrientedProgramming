using System;
using Xunit;
using static Rop.OptionModule;

namespace Rop.OptionModuleTests
{
    public class GetValueTests
    {
        [Fact]
        public void GetValueReturnsTheUnderlyingValueIfSome()
        {
            var expected = Guid.NewGuid().ToString("N");
            var actual = Option<string>.Some(expected).GetValue();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetValueThrowsIfNoneIsPassed()
        {
            var option = Option<string>.None;
            Assert.Throws<ArgumentException>(() => option.GetValue());
        }

        [Fact]
        public void GetValueThrowsIfNullIsPassed()
        {
            Assert.Throws<ArgumentException>(() => ((Option<string>)null).GetValue());
        }
    }

    public class CountTests
    {
        [Fact]
        public void TheCountForNoneIsZero()
        {
            var option = Option<string>.None;
            var expected = 0;
            var actual = option.Count();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TheCountForSomeIsOne()
        {
            var option = Option<string>.Some(Guid.NewGuid().ToString("N"));
            var expected = 1;
            var actual = option.Count();
            Assert.Equal(expected, actual);
        }
    }

    public class FoldTests
    {
        public Func<int, int> SeededAdd(int seed)
        {
            return x => x + seed;
        }

        [Fact]
        public void FoldingNothingReturnsTheStadte()
        {
            var expected = 12;
            var option = Option<int>.Some(7);
            var actual = option.Fold(SeededAdd, 5);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FoldingNothingReturnsTheState()
        {
            var expected = 0;
            var option = Option<int>.None;
            var actual = option.Fold(SeededAdd, 0);
            Assert.Equal(expected, actual);
        }
    }

    public class FoldBackTests
    {
        public Func<int, int> SeededAdd(int seed)
        {
            return x => x + seed;
        }

        [Fact]
        public void FoldingBackNothingReturnsTheState()
        {
            var expected = 0;
            var option = Option<int>.None;
            var actual = FoldBack(SeededAdd, option, 0);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FoldingNothingReturnsTheStadte()
        {
            var expected = 12;
            var option = Option<int>.Some(7);
            var actual = FoldBack(SeededAdd, option, 5);
            Assert.Equal(expected, actual);
        }
    }

    public class ExistsTests
    {
        [Fact]
        public void ExistsReturnsFalseForEmptyOptions()
        {
            var option = Option<int>.None;
            var actual = Exists(_ => true, option);
            Assert.False(actual);
        }

        [Fact]
        public void ExistsReturnsFalseForOptionsThatSatisfyThePredicate()
        {
            var option = Option<int>.Some(7);
            var actual = Exists(x => x % 2 == 0, option);
            Assert.False(actual);
        }

        [Fact]
        public void ExistsReturnsTrueForOptionsThatSatisfyThePredicate()
        {
            var option = Option<int>.Some(7);
            var actual = Exists(x => x % 2 == 1, option);
            Assert.True(actual);
        }
    }

    public class ForAllTests
    {
        [Fact]
        public void ForAllReturnsFalseForEmptyOptions()
        {
            var option = Option<int>.None;
            var actual = ForAll(_ => true, option);
            Assert.True(actual);
        }

        [Fact]
        public void ForAllReturnsFalseForOptionsThatSatisfyThePredicate()
        {
            var option = Option<int>.Some(7);
            var actual = ForAll(x => x % 2 == 0, option);
            Assert.False(actual);
        }

        [Fact]
        public void ForAllReturnsTrueForOptionsThatSatisfyThePredicate()
        {
            var option = Option<int>.Some(7);
            var actual = ForAll(x => x % 2 == 1, option);
            Assert.True(actual);
        }
    }

    public class IterateTests
    {
        [Fact]
        public void WhenTheOptionIsNoneThenIterationDoesNotOccur()
        {
            var wasCalled = false;
            Action<int> action = _ => wasCalled = true;
            Iterate(action, Option<int>.None);
            Assert.False(wasCalled);
        }

        [Fact]
        public void WhenTheOptionIsSomeThenIterationOccurs()
        {
            var wasCalled = false;
            var captured = 0;
            Action<int> action = _ =>
            {
                captured = _;
                wasCalled = true;
            };
            Iterate(action, Option<int>.Some(5));
            Assert.True(wasCalled);
            Assert.Equal(5, captured);
        }
    }
}