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
        public static b Either<a, b, c>(this Result<a, c> twoTrackInput, Func<a, b> successFunc, Func<c, b> failureFunc)
        {
            return CommonLibrary.Either(successFunc, failureFunc, twoTrackInput);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<b, c> Bind<a, b, c>(this Result<a, c> input, Func<a, Result<b, c>> f)
        {
            return CommonLibrary.Bind(f)(input);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<c, b> InfixBind<a, b, c>(this Result<a, b> x, Func<a, Result<c, b>> f)
        {
            return CommonLibrary.Bind(f)(x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<d, c> SwitchComposition<a, b, c, d>(this a input, Func<a, Result<b, c>> s1, Func<b, Result<d, c>> s2)
        {
            return CommonLibrary.Bind(s2)(s1(input));
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<b, c> Switch<a, b, c>(this a x, Func<a, b> f)
        {
            return CommonLibrary.Succeed<b, c>(f(x));
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<c, b> Derail<a, b, c>(this a x, Func<a, b> f)
        {
            return CommonLibrary.Fail<b, c>(f(x));
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<b, c> Map<a, b, c>(this Result<a, c> x, Func<a, b> f)
        {
            return CommonLibrary.Map<a, b, c>(f)(x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static a Tee<a>(this a x, Action<a> f)
        {
            return CommonLibrary.Tee(f, x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<b, c> TryCatch<a, b, c>(this a x, Func<a, b> f, Func<Exception, c> exnHandler)
        {
            return CommonLibrary.TryCatch(f, exnHandler, x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<b, d> DoubleMap<a, b, c, d>(this Result<a, c> x, Func<a, b> successFunc, Func<c, d> failureFunc)
        {
            return CommonLibrary.DoubleMap(successFunc, failureFunc)(x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<c, d> Plus<a, b, c, d, e>(this e x,
            Func<a, b, c> addSuccess,
            Func<d, d, d> addFailure,
            Func<e, Result<a, d>> switch1,
            Func<e, Result<b, d>> switch2)
        {
            return CommonLibrary.Plus(addSuccess, addFailure, switch1, switch2, x);
        }
    }
}