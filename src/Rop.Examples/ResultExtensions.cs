using System;
using System.Diagnostics;

namespace Rop.Examples
{
    public static class ResultExtensions
    {
        public static Result<TTarget, TFailure> OnSuccess<TTarget, TFailure>(this Result<TTarget, TFailure> result, Func<TTarget, TTarget> func)
        {
            return result.IsSuccess
                ? Result<TTarget, TFailure>.NewSuccess(func(((Result<TTarget, TFailure>.Success)result).Value))
                : result;
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<string, string> Plus(this Result<string, string> x,
            Func<string, Result<string, string>> switch1,
            Func<string, Result<string, string>> switch2)
        {
            if (x.IsFailure)
                return x;
            var success = x as Result<string, string>.Success;

            Func<string, string, string> addSuccess = (a, b) => a;
            Func<string, string, string> addFailure = (f1, f2) => String.Format("{0}{1}{2}", f2, Environment.NewLine, f1);
            return CommonLibrary.Plus(addSuccess, addFailure, switch1, switch2, success.Value);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<string, string> Combine(this Result<string, string> x, params Func<string, Result<string, string>>[] switches)
        {
            if (x.IsFailure)
                return x;
            var success = x as Result<string, string>.Success;

            Func<string, string, string> addSuccess = (a, b) => a;
            Func<string, string, string> addFailure = (f1, f2) => string.Format("{0}{1}{2}", f2, Environment.NewLine, f1);
            return CommonLibrary.Combine(success.Value, addSuccess, addFailure, switches);
        }
    }
}