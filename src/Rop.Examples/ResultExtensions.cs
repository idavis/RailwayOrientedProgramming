using System;
using System.Diagnostics;

namespace Rop.Examples
{
    public static class ResultExtensions
    {
        public static Result<T, string> ToResult<T>(this Option<T> option, string message)
        {
            return ToResult<T, string>(option, message);
        }

        public static Result<TSuccess, TFailure> ToResult<TSuccess, TFailure>(this Option<TSuccess> option, TFailure failure)
        {
            if (option.IsSome())
                return Result<TSuccess, TFailure>.NewSuccess(option.Value);
            return Result<TSuccess, TFailure>.NewFailure(failure);
        }

        public static Result<TTarget, TFailure> OnSuccess<TTarget, TFailure>(this Result<TTarget, TFailure> result, Func<TTarget, TTarget> func)
        {
            return result.IsSuccess
                ? Result<TTarget, TFailure>.NewSuccess(func(((Result<TTarget, TFailure>.Success)result).Item))
                : result;
        }

        public static Result<TSuccess, TFailure> Bind<TSuccess, TFailure>(this Result<TSuccess, TFailure> result, Func<TSuccess, Result<TSuccess, TFailure>> binder)
        {
            if (result.IsSuccess)
                return binder(((Result<TSuccess, TFailure>.Success)result).Item);

            return result;
        }

        public static Func<TSuccess, Result<TSuccess, TFailure>> Kleisli<TSuccess, TFailure>(
            this Func<TSuccess, Result<TSuccess, TFailure>> switch1, Func<TSuccess, Result<TSuccess, TFailure>> switch2)
        {
            return x => switch1(x).Bind(switch2);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<string, string> Plus(this Result<string, string> x,

            Func<Result<string, string>, Result<string, string>> switch1,
            Func<Result<string, string>, Result<string, string>> switch2)
        {
            Func<string, string, string> addSuccess = (a, b) => a;
            Func<string, string, string> addFailure = (f1, f2) => String.Format("{0}{1}{2}", f2, Environment.NewLine, f1);
            return CommonLibrary.Plus(addSuccess, addFailure, switch1, switch2, x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<string, string> Plus(this Result<string, string> x, params Func<Result<string, string>, Result<string, string>>[] switches)
        {
            Func<string, string, string> addSuccess = (a, b) => a;
            Func<string, string, string> addFailure = (f1, f2) => String.Format("{0}{1}{2}", f2, Environment.NewLine, f1);
            return Plus(x, addSuccess, addFailure, switches);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<a, d> Plus<a, d, e>(e x, Func<a, a, a> addSuccess, Func<d, d, d> addFailure, params Func<e, Result<a, d>>[] switches) where e : Result<a, d>
        {
            Result<a, d> previous = x;
            Func<e, Result<a, d>> getPrevious = z => previous;
            
            foreach (var current in switches)
            {
                previous = CommonLibrary.Plus(addSuccess, addFailure, getPrevious, current, x);
            }
            return getPrevious(x);
        }
    }
}