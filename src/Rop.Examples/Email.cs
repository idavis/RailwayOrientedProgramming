using System;
using Xunit;

namespace Rop.Examples
{
    public class EmailTests
    {
        [Fact]
        public void Creating_An_Email_From_An_Empty_String_Option_Fails_With_MustNotBeEmpty_Message()
        {
            var res = Email.Create("");
            Assert.True(res.IsFailure);
            var failure = res.AsFailure();
            Assert.Equal("must not be empty", failure.Value);
        }

        [Fact]
        public void Creating_An_Email_From_An_NullOption_Fails_With_Email_MustHaveAValue_Message()
        {
            var res = Email.Create(null);
            Assert.True(res.IsFailure);
            var failure = res.AsFailure();
            Assert.Equal("Email must have a value", failure.Value);
        }

        [Fact]
        public void Creating_An_Email_From_An_NullString_Fails_With_Email_MustHaveAValue_Message()
        {
            string dummy = null;
            var res = Email.Create(dummy);
            Assert.True(res.IsFailure);
            var failure = res.AsFailure();
            Assert.Equal("Email must have a value", failure.Value);
        }
    }

    public class Email
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Result<Email, string> Create(Option<string> value)
        {
            var result = value
                .ToResult("Email must have a value")
                .Bind(MustNotBeEmpty)
                .Combine(MustContainAnAtSymbol, MustNotBeTooLong, MustNotBeTooShort)
                .DoubleMap(x => { Console.WriteLine(x); return x; }, x => { Console.Error.WriteLine(x); return x; }) // if success, write to console, write to error on failure
                .Tee(Console.WriteLine)
                .Map(x => new Email(x));
            return result;
        }

        private static Result<string, string> MustNotBeEmpty(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return Result<string, string>.NewFailure("must not be empty");
            return Result<string, string>.NewSuccess(value);
        }

        private static Result<string, string> MustNotBeTooLong(string value)
        {
            if (value.Length <= 128) return Result<string, string>.NewSuccess(value);

            return Result<string, string>.NewFailure("must not be too long");
        }

        private static Result<string, string> MustNotBeTooShort(string value)
        {
            if (value.Length >= 5) return Result<string, string>.NewSuccess(value);

            return Result<string, string>.NewFailure("must not be too short");
        }

        private static Result<string, string> MustContainAnAtSymbol(string value)
        {
            if (value.Contains("@")) return Result<string, string>.NewSuccess(value);

            return Result<string, string>.NewFailure("must contain an @ symbol");
        }
    }
}