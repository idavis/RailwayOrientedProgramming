using System;
using System.Diagnostics;

namespace Rop
{
    [DebuggerStepThrough]
    [DebuggerNonUserCode]
    public static class ResultExtensions
    {
        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static TResult Either<TSuccess, TResult, TFailure>(this Result<TSuccess, TFailure> twoTrackInput, Func<TSuccess, TResult> successFunc, Func<TFailure, TResult> failureFunc)
        {
            return CommonLibrary.Either(successFunc, failureFunc, twoTrackInput);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess, TFailure> Bind<TInput, TSuccess, TFailure>(this Result<TInput, TFailure> input, Func<TInput, Result<TSuccess, TFailure>> f)
        {
            return CommonLibrary.Bind(f)(input);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess2, TFailure> InfixBind<TSuccess1, TFailure, TSuccess2>(this Result<TSuccess1, TFailure> x, Func<TSuccess1, Result<TSuccess2, TFailure>> f)
        {
            return CommonLibrary.Bind(f)(x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess2, TFailure> SwitchComposition<TInput, TSuccess1, TFailure, TSuccess2>(this TInput input, Func<TInput, Result<TSuccess1, TFailure>> s1, Func<TSuccess1, Result<TSuccess2, TFailure>> s2)
        {
            return CommonLibrary.Bind(s2)(s1(input));
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess, TFailure> Switch<TInput, TSuccess, TFailure>(this TInput x, Func<TInput, TSuccess> f)
        {
            return CommonLibrary.Succeed<TSuccess, TFailure>(f(x));
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess, TFailure> Derail<TInput, TFailure, TSuccess>(this TInput x, Func<TInput, TFailure> f)
        {
            return CommonLibrary.Fail<TFailure, TSuccess>(f(x));
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccessOut, TFailure> Map<TSuccessIn, TSuccessOut, TFailure>(this Result<TSuccessIn, TFailure> x, Func<TSuccessIn, TSuccessOut> f)
        {
            return CommonLibrary.Map<TSuccessIn, TSuccessOut, TFailure>(f)(x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static TInput Tee<TInput>(this TInput x, Action<TInput> f)
        {
            return CommonLibrary.Tee(f, x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess, TFailure> TryCatch<TInput, TSuccess, TFailure>(this TInput x, Func<TInput, TSuccess> f, Func<Exception, TFailure> exnHandler)
        {
            return CommonLibrary.TryCatch(f, exnHandler, x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess, TFailure> DoubleMap<TInput1, TSuccess, TInput2, TFailure>(this Result<TInput1, TInput2> x, Func<TInput1, TSuccess> successFunc, Func<TInput2, TFailure> failureFunc)
        {
            return CommonLibrary.DoubleMap(successFunc, failureFunc)(x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Func<TSuccess, Result<TSuccess, TFailure>> Kleisli<TSuccess, TFailure>(this Func<TSuccess, Result<TSuccess, TFailure>> switch1, Func<TSuccess, Result<TSuccess, TFailure>> switch2)
        {
            return CommonLibrary.Kleisli(switch1, switch2);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess, TFailure> Plus<TSuccess1, TSuccess2, TSuccess, TFailure, TInput>( 
            this TInput x,
            Func<TSuccess1, TSuccess2, TSuccess> addSuccess,
            Func<TFailure, TFailure, TFailure> addFailure,
            Func<TInput, Result<TSuccess1, TFailure>> switch1,
            Func<TInput, Result<TSuccess2, TFailure>> switch2)
        {
            return CommonLibrary.Plus(addSuccess, addFailure, switch1, switch2, x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess, TFailure>.Success AsSuccess<TSuccess, TFailure>(this Result<TSuccess,TFailure> result)
        {
            if(result.IsSuccess)
            {
                return result as Result<TSuccess, TFailure>.Success;
            }
            throw new InvalidOperationException();
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess, TFailure>.Failure AsFailure<TSuccess, TFailure>(this Result<TSuccess, TFailure> result)
        {
            if (result.IsFailure)
            {
                return result as Result<TSuccess, TFailure>.Failure;
            }
            throw new InvalidOperationException();
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<T, string> ToResult<T>(this Option<T> option, string message)
        {
            return ToResult<T, string>(option, message);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess, TFailure> ToResult<TSuccess, TFailure>(this Option<TSuccess> option, TFailure failure)
        {
            if (option.IsSome())
                return Result<TSuccess, TFailure>.NewSuccess(option.Value);
            return Result<TSuccess, TFailure>.NewFailure(failure);
        }
    }
}