using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Rop
{
    [DebuggerDisplay("{__DebugDisplay(),nq}")]
    [Serializable]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    public abstract class Result<TSuccess, TFailure> : IEquatable<Result<TSuccess, TFailure>>,
        IStructuralEquatable, IComparable<Result<TSuccess, TFailure>>, IComparable, IStructuralComparable
    {
        [DebuggerNonUserCode]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Tag
        {
            [DebuggerNonUserCode]
            get { return this is Failure ? 1 : 0; }
        }

        [DebuggerNonUserCode]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsSuccess
        {
            [DebuggerNonUserCode]
            get { return this is Success; }
        }

        [DebuggerNonUserCode]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsFailure
        {
            [DebuggerNonUserCode]
            get { return this is Failure; }
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        internal Result()
        {
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess, TFailure> NewSuccess(TSuccess item)
        {
            return new Success(item);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Result<TSuccess, TFailure> NewFailure(TFailure item)
        {
            return new Failure(item);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        internal object __DebugDisplay()
        {
            if (this is Success)
            {
                var success = this as Success;
                return string.Format("Success[{0}]", success.Value);
            }
            else
            {
                var failure = this as Failure;
                return string.Format("Failure[{0}]", failure.Value);
            }
        }

        public virtual int CompareTo(Result<TSuccess, TFailure> obj)
        {
            if (obj == null)
                return 1;
            int num1 = !(this is Failure) ? 0 : 1;
            int num2 = !(obj is Failure) ? 0 : 1;
            if (num1 != num2)
                return num1 - num2;
            if (this is Success)
                return Comparer<TSuccess>.Default.Compare(((Success)this)._Value, ((Success)obj)._Value);

            return Comparer<TFailure>.Default.Compare(((Failure)this)._Value, ((Failure)obj)._Value);
        }

        public virtual int CompareTo(object obj)
        {
            return CompareTo((Result<TSuccess, TFailure>)obj);
        }

        public virtual int CompareTo(object obj, IComparer comp)
        {
            Result<TSuccess, TFailure> result = (Result<TSuccess, TFailure>)obj;
            if ((Result<TSuccess, TFailure>)obj == null)
                return 1;
            int num1 = !(this is Failure) ? 0 : 1;
            int num2 = !(result is Failure) ? 0 : 1;
            if (num1 != num2)
                return num1 - num2;
            if (this is Success)
            {
                Success success1 = (Success)this;
                Success success2 = (Success)result;
                return comp.Compare(success1._Value, success2._Value);
            }
            Failure failure1 = (Failure)this;
            Failure failure2 = (Failure)result;
            return comp.Compare(failure1._Value, failure2._Value);
        }

        public virtual int GetHashCode(IEqualityComparer comp)
        {
            if (this is Success)
            {
                Success success = (Success)this;
                int num = 0;
                return comp.GetHashCode(success._Value) + ((num << 6) + (num >> 2)) - 1640531527;
            }
            Failure failure = (Failure)this;
            int num1 = 1;
            return comp.GetHashCode(failure._Value) + ((num1 << 6) + (num1 >> 2)) - 1640531527;
        }

        public sealed override int GetHashCode()
        {
            if (this is Success)
                return GetHashCode(EqualityComparer<TSuccess>.Default);
            return GetHashCode(EqualityComparer<TFailure>.Default);
        }

        public virtual bool Equals(object obj, IEqualityComparer comp)
        {
            Result<TSuccess, TFailure> result1 = obj as Result<TSuccess, TFailure>;
            if (result1 == null)
                return false;
            Result<TSuccess, TFailure> result2 = result1;
            if ((!(this is Failure) ? 0 : 1) !=
                (!(result2 is Failure) ? 0 : 1))
                return false;
            if (this is Success)
            {
                Success success1 = (Success)this;
                Success success2 = (Success)result2;
                return comp.Equals(success1._Value, success2._Value);
            }
            Failure failure1 = (Failure)this;
            Failure failure2 = (Failure)result2;
            return comp.Equals(failure1._Value, failure2._Value);
        }

        public virtual bool Equals(Result<TSuccess, TFailure> obj)
        {
            if (obj == null ||
                (!(this is Failure) ? 0 : 1) !=
                (!(obj is Failure) ? 0 : 1))
                return false;
            if (this is Success)
                return EqualityComparer<TSuccess>.Default.Equals(((Success)this)._Value, ((Success)obj)._Value);
            return EqualityComparer<TFailure>.Default.Equals(((Failure)this)._Value, ((Failure)obj)._Value);
        }

        public sealed override bool Equals(object obj)
        {
            Result<TSuccess, TFailure> result = obj as Result<TSuccess, TFailure>;
            if (result != null)
                return Equals(result);
            return false;
        }

        public static class Tags
        {
            public const int Success = 0;
            public const int Failure = 1;
        }

        [DebuggerTypeProxy("SuccessDebugTypeProxy")]
        [DebuggerDisplay("{__DebugDisplay(),nq}")]
        [Serializable]
        public class Success : Result<TSuccess, TFailure>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal readonly TSuccess _Value;

            [DebuggerNonUserCode]
            public TSuccess Value
            {
                [DebuggerNonUserCode]
                get { return _Value; }
            }

            [DebuggerStepThrough]
            [DebuggerNonUserCode]
            internal Success(TSuccess value)
            {
                _Value = value;
            }
        }

        [DebuggerTypeProxy("FailureDebugTypeProxy")]
        [DebuggerDisplay("{__DebugDisplay(),nq}")]
        [Serializable]
        public class Failure : Result<TSuccess, TFailure>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal readonly TFailure _Value;

            [DebuggerNonUserCode]
            public TFailure Value
            {
                [DebuggerNonUserCode]
                get { return _Value; }
            }

            [DebuggerStepThrough]
            [DebuggerNonUserCode]
            internal Failure(TFailure value)
            {
                _Value = value;
            }
        }

        internal class SuccessDebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal Success _Success;

            [DebuggerNonUserCode]
            public TSuccess Value
            {
                [DebuggerNonUserCode]
                get { return _Success._Value; }
            }

            [DebuggerStepThrough]
            [DebuggerNonUserCode]
            public SuccessDebugTypeProxy(Success success)
            {
                _Success = success;
            }
        }

        internal class FailureDebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal Failure _Failure;

            [DebuggerNonUserCode]
            public TFailure Value
            {
                [DebuggerNonUserCode]
                get { return _Failure._Value; }
            }

            [DebuggerStepThrough]
            [DebuggerNonUserCode]
            public FailureDebugTypeProxy(Failure failure)
            {
                _Failure = failure;
            }
        }
    }
}