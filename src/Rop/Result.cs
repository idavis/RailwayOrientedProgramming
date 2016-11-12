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
                return string.Format("Success[{0}]", success.Item);
            }
            else
            {
                var failure = this as Failure;
                return string.Format("Failure[{0}]", failure.Item);
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
                return Comparer<TSuccess>.Default.Compare(((Success)this).item, ((Success)obj).item);

            return Comparer<TFailure>.Default.Compare(((Failure)this).item, ((Failure)obj).item);
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
                return comp.Compare(success1.item, success2.item);
            }
            Failure failure1 = (Failure)this;
            Failure failure2 = (Failure)result;
            return comp.Compare(failure1.item, failure2.item);
        }

        public virtual int GetHashCode(IEqualityComparer comp)
        {
            if (this is Success)
            {
                Success success = (Success)this;
                int num = 0;
                return comp.GetHashCode(success.item) + ((num << 6) + (num >> 2)) - 1640531527;
            }
            Failure failure = (Failure)this;
            int num1 = 1;
            return comp.GetHashCode(failure.item) + ((num1 << 6) + (num1 >> 2)) - 1640531527;
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
                return comp.Equals(success1.item, success2.item);
            }
            Failure failure1 = (Failure)this;
            Failure failure2 = (Failure)result2;
            return comp.Equals(failure1.item, failure2.item);
        }

        public virtual bool Equals(Result<TSuccess, TFailure> obj)
        {
            if (obj == null ||
                (!(this is Failure) ? 0 : 1) !=
                (!(obj is Failure) ? 0 : 1))
                return false;
            if (this is Success)
                return EqualityComparer<TSuccess>.Default.Equals(((Success)this).item, ((Success)obj).item);
            return EqualityComparer<TFailure>.Default.Equals(((Failure)this).item, ((Failure)obj).item);
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
            internal readonly TSuccess item;

            [DebuggerNonUserCode]
            public TSuccess Item
            {
                [DebuggerNonUserCode]
                get { return item; }
            }

            [DebuggerStepThrough]
            [DebuggerNonUserCode]
            internal Success(TSuccess item)
            {
                this.item = item;
            }
        }

        [DebuggerTypeProxy("FailureDebugTypeProxy")]
        [DebuggerDisplay("{__DebugDisplay(),nq}")]
        [Serializable]
        public class Failure : Result<TSuccess, TFailure>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal readonly TFailure item;

            [DebuggerNonUserCode]
            public TFailure Item
            {
                [DebuggerNonUserCode]
                get { return item; }
            }

            [DebuggerStepThrough]
            [DebuggerNonUserCode]
            internal Failure(TFailure item)
            {
                this.item = item;
            }
        }

        internal class SuccessDebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal Success _obj;

            [DebuggerNonUserCode]
            public TSuccess Item
            {
                [DebuggerNonUserCode]
                get { return _obj.item; }
            }

            [DebuggerStepThrough]
            [DebuggerNonUserCode]
            public SuccessDebugTypeProxy(Success obj)
            {
                _obj = obj;
            }
        }

        internal class FailureDebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal Failure _obj;

            [DebuggerNonUserCode]
            public TFailure Item
            {
                [DebuggerNonUserCode]
                get { return _obj.item; }
            }

            [DebuggerStepThrough]
            [DebuggerNonUserCode]
            public FailureDebugTypeProxy(Failure obj)
            {
                _obj = obj;
            }
        }
    }
}