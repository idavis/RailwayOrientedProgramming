using System;
using Xunit;

namespace Rop.Examples
{
    public class EmailTests
    {
        [Fact]
        public void Demo()
        {
            var res = Email.Create("asdf");
            Console.WriteLine(res);
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
                .ToResult("Email must have a value") // Display an error if there was no value
                .Bind(MustNotBeEmpty) // a value was provided, but it was emtpy
                .Plus(MustContainAnAtSymbol, MustNotBeTooLong, MustNotBeTooShort)
                .DoubleMap(x => { Console.WriteLine(x); return x; }, x => { Console.Error.WriteLine(x); return x; }) // if success, write to console, write to error on failure
                .Tee(Console.WriteLine)
                .Map(x => new Email(x));
            return result;
        }

        private static Result<string, string> MustNotBeEmpty(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                Result<string, string>.NewFailure("must not be empty");
            return Result<string, string>.NewSuccess(value);
        }

        private static Result<string, string> MustNotBeTooLong(Result<string, string> result)
        {
            var success = result as Result<string, string>.Success;
            if (success == null) return result;

            if (success.Item.Length <= 128) return result;

            return Result<string, string>.NewFailure("must not be too long");
        }

        private static Result<string, string> MustNotBeTooShort(Result<string, string> result)
        {
            var success = result as Result<string, string>.Success;
            if (success == null) return result;

            if (success.Item.Length >= 5) return result;

            return Result<string, string>.NewFailure("must not be too short");
        }

        private static Result<string, string> MustContainAnAtSymbol(Result<string, string> result)
        {
            var success = result as Result<string, string>.Success;
            if (success == null) return result;

            if (success.Item.Contains("@")) return result;

            return Result<string, string>.NewFailure("must contain an @ symbol");
        }
    }
}