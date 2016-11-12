using System;
using System.Diagnostics;

namespace Rop
{
    [DebuggerStepThrough]
    [DebuggerNonUserCode]
    public static class CommonLibrary
    {
        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<a, b> Succeed<a, b>(a x)
        {
            return Result<a, b>.NewSuccess(x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<b, a> Fail<a, b>(a x)
        {
            return Result<b, a>.NewFailure(x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static b Either<a, b, c>(Func<a, b> successFunc, Func<c, b> failureFunc, Result<a, c> twoTrackInput)
        {
            Result<a, c> result = twoTrackInput;
            if (result is Result<a, c>.Failure)
            {
                c func1 = ((Result<a, c>.Failure)result).item;
                return failureFunc(func1);
            }
            a func = ((Result<a, c>.Success)result).item;
            return successFunc(func);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Func<Result<a, c>, Result<b, c>> Bind<a, b, c>(Func<a, Result<b, c>> f)
        {
            return (twoTrackInput) => Either(f, Fail<c, b>, twoTrackInput);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<c, b> InfixBind<a, b, c>(Result<a, b> x, Func<a, Result<c, b>> f)
        {
            return Bind(f)(x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Func<a, Result<d, c>> SwitchComposition<a, b, c, d>(Func<a, Result<b, c>> s1,
            Func<b, Result<d, c>> s2)
        {
            return input => Bind(s2)(s1(input));
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Func<a, Result<b, c>> Switch<a, b, c>(Func<a, b> f)
        {
            return x => Succeed<b, c>(f(x));
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Func<a, Result<c, b>> Derail<a, b, c>(Func<a, b> f)
        {
            return x => Fail<b, c>(f(x));
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Func<Result<a, c>, Result<b, c>> Map<a, b, c>(Func<a, b> f)
        {
            return x => Either(Switch<a, b, c>(f), Fail<c, b>, x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static a Tee<a>(Action<a> f, a x)
        {
            f(x);
            return x;
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<b, c> TryCatch<a, b, c>(Func<a, b> f, Func<Exception, c> exnHandler, a x)
        {
            try
            {
                return Succeed<b, c>(f(x));
            }
            catch (Exception ex)
            {
                return Fail<c, b>(exnHandler(ex));
            }
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Func<Result<a, c>, Result<b, d>> DoubleMap<a, b, c, d>(
            Func<a, b> successFunc, Func<c, d> failureFunc)
        {
            return x => Either(Switch<a, b, d>(successFunc), Derail<c, d, b>(failureFunc), x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<c, d> Plus<a, b, c, d, e>(Func<a, b, c> addSuccess, Func<d, d, d> addFailure, Func<e, Result<a, d>> switch1, Func<e, Result<b, d>> switch2, e x)
        {
            Tuple<Result<a, d>, Result<b, d>> tuple = new Tuple<Result<a, d>, Result<b, d>>(switch1(x), switch2(x));
            if (tuple.Item1 is Result<a, d>.Failure)
            {
                Result<a, d>.Failure failure = (Result<a, d>.Failure)tuple.Item1;
                if (tuple.Item2 is Result<b, d>.Failure)
                {
                    d d1 = ((Result<b, d>.Failure)tuple.Item2).item;
                    d d2 = failure.item;
                    return Result<c, d>.NewFailure(addFailure(d2, d1));
                }
                return Result<c, d>.NewFailure(failure.item);
            }

            if (tuple.Item2 is Result<b, d>.Failure)
                return Result<c, d>.NewFailure(((Result<b, d>.Failure)tuple.Item2).item);
            b b1 = ((Result<b, d>.Success)tuple.Item2).item;
            Result<a, d>.Success success1 = (Result<a, d>.Success)tuple.Item1;
            a a1 = success1.item;
            return Result<c, d>.NewSuccess(addSuccess(a1, b1));
        }
    }
}
