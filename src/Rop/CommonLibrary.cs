using System;
using System.Diagnostics;
using System.Linq;

namespace Rop
{
    [DebuggerStepThrough]
    [DebuggerNonUserCode]
    public static class CommonLibrary
    {
        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess, TFailure> Succeed<TSuccess, TFailure>(TSuccess value)
        {
            return Result<TSuccess, TFailure>.NewSuccess(value);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess, TFailure> Fail<TFailure, TSuccess>(TFailure value)
        {
            return Result<TSuccess, TFailure>.NewFailure(value);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static TResult Either<TSuccess, TResult, TFailure>(Func<TSuccess, TResult> successFunc, Func<TFailure, TResult> failureFunc, Result<TSuccess, TFailure> twoTrackInput)
        {
            Result<TSuccess, TFailure> result = twoTrackInput;
            if (result is Result<TSuccess, TFailure>.Failure)
            {
                TFailure failure = ((Result<TSuccess, TFailure>.Failure)result)._Value;
                return failureFunc(failure);
            }
            TSuccess success = ((Result<TSuccess, TFailure>.Success)result)._Value;
            return successFunc(success);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Func<Result<TInput, TFailure>, Result<TSuccess, TFailure>> Bind<TInput, TSuccess, TFailure>(Func<TInput, Result<TSuccess, TFailure>> f)
        {
            return (twoTrackInput) => Either(f, Fail<TFailure, TSuccess>, twoTrackInput);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess2, TFailure> InfixBind<TSuccess1, TFailure, TSuccess2>(Result<TSuccess1, TFailure> x, Func<TSuccess1, Result<TSuccess2, TFailure>> f)
        {
            return Bind(f)(x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Func<TInput, Result<TSuccess2, Failure>> SwitchComposition<TInput, TSuccess1, Failure, TSuccess2>(Func<TInput, Result<TSuccess1, Failure>> s1, Func<TSuccess1, Result<TSuccess2, Failure>> s2)
        {
            return input => Bind(s2)(s1(input));
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Func<TInput, Result<TSuccess, TFailure>> Switch<TInput, TSuccess, TFailure>(Func<TInput, TSuccess> f)
        {
            return input => Succeed<TSuccess, TFailure>(f(input));
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Func<TInput, Result<TSuccess, TFailure>> Derail<TInput, TFailure, TSuccess>(Func<TInput, TFailure> f)
        {
            return input => Fail<TFailure, TSuccess>(f(input));
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Func<Result<TSuccessIn, TFailure>, Result<TSuccessOut, TFailure>> Map<TSuccessIn, TSuccessOut, TFailure>(Func<TSuccessIn, TSuccessOut> f)
        {
            return input => Either(Switch<TSuccessIn, TSuccessOut, TFailure>(f), Fail<TFailure, TSuccessOut>, input);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static TInput Tee<TInput>(Action<TInput> action, TInput input)
        {
            action(input);
            return input;
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess, TFailure> TryCatch<TInput, TSuccess, TFailure>(Func<TInput, TSuccess> f, Func<Exception, TFailure> exnHandler, TInput input)
        {
            try
            {
                return Succeed<TSuccess, TFailure>(f(input));
            }
            catch (Exception ex)
            {
                return Fail<TFailure, TSuccess>(exnHandler(ex));
            }
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Func<Result<TInput1, TInput2>, Result<TSuccess, TFailure>> DoubleMap<TInput1, TSuccess, TInput2, TFailure>(Func<TInput1, TSuccess> successFunc, Func<TInput2, TFailure> failureFunc)
        {
            return x => Either(Switch<TInput1, TSuccess, TFailure>(successFunc), Derail<TInput2, TFailure, TSuccess>(failureFunc), x);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Func<TSuccess, Result<TSuccess, TFailure>> Kleisli<TSuccess, TFailure>(Func<TSuccess, Result<TSuccess, TFailure>> switch1, Func<TSuccess, Result<TSuccess, TFailure>> switch2)
        {
            return input => switch1(input).Bind(switch2);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess, TFailure> Plus<TSuccess1, TSuccess2, TSuccess, TFailure, TInput>(
            Func<TSuccess1, TSuccess2, TSuccess> addSuccess, 
            Func<TFailure, TFailure, TFailure> addFailure, 
            Func<TInput, Result<TSuccess1, TFailure>> switch1, 
            Func<TInput, Result<TSuccess2, TFailure>> switch2, 
            TInput x)
        {
            Tuple<Result<TSuccess1, TFailure>, Result<TSuccess2, TFailure>> tuple = new Tuple<Result<TSuccess1, TFailure>, Result<TSuccess2, TFailure>>(switch1(x), switch2(x));
            if (tuple.Item1 is Result<TSuccess1, TFailure>.Failure)
            {
                Result<TSuccess1, TFailure>.Failure failure = (Result<TSuccess1, TFailure>.Failure)tuple.Item1;
                if (tuple.Item2 is Result<TSuccess2, TFailure>.Failure)
                {
                    TFailure d1 = ((Result<TSuccess2, TFailure>.Failure)tuple.Item2)._Value;
                    TFailure d2 = failure._Value;
                    return Result<TSuccess, TFailure>.NewFailure(addFailure(d2, d1));
                }
                return Result<TSuccess, TFailure>.NewFailure(failure._Value);
            }

            if (tuple.Item2 is Result<TSuccess2, TFailure>.Failure)
                return Result<TSuccess, TFailure>.NewFailure(((Result<TSuccess2, TFailure>.Failure)tuple.Item2)._Value);
            TSuccess2 b1 = ((Result<TSuccess2, TFailure>.Success)tuple.Item2)._Value;
            Result<TSuccess1, TFailure>.Success success1 = (Result<TSuccess1, TFailure>.Success)tuple.Item1;
            TSuccess1 a1 = success1._Value;
            return Result<TSuccess, TFailure>.NewSuccess(addSuccess(a1, b1));
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess, TFailure> Combine<TSuccess, TFailure, TInput>(TInput input, Func<TSuccess, TSuccess, TSuccess> addSuccess, Func<TFailure, TFailure, TFailure> addFailure, params Func<TInput, Result<TSuccess, TFailure>>[] switches)
        {
            if (switches.Length == 0)
                throw new InvalidOperationException();

            if (switches.Length == 1)
                return switches[0](input);

            if (switches.Length == 2)
                return Plus(addSuccess, addFailure, switches[0], switches[1], input);

            var previous = switches[0](input);
            Func<TInput, Result<TSuccess, TFailure>> getPrevious = z => previous;

            for(int i = 1; i < switches.Length; i++)
            {
                previous = Plus(addSuccess, addFailure, getPrevious, switches[i], input);
            }
            return getPrevious(input);
        }
    }
}
