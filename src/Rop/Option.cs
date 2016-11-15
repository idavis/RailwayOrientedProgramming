using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Rop
{
    [Serializable]
    [DebuggerDisplay("Some({Value})")]
#if USESTRUCT
    public struct Option<T>
#else
    public class Option<T>
#endif
        : IComparable,
            IComparable<Option<T>>,
            IStructuralComparable,
            IStructuralEquatable,
            IEquatable<Option<T>>
    {
        public static Option<T> None { get; } = new Option<T>(default(T));

        public T Value { get; }

        public Option(T value)
        {
            Value = value;
        }

        public static implicit operator Option<T>(T value)
        {
            if(typeof(T).IsValueType)
                return Some(value);
            if (ReferenceEquals(value, null))
                return None;
            return Some(value);
        }

        public static bool operator ==(Option<T> left, Option<T> right)
        {
#if USESTRUCT
            return left.Equals(right);
#else
            return Equals(left, right);
#endif

        }

        public static bool operator !=(Option<T> left, Option<T> right)
        {
#if USESTRUCT
            return !left.Equals(right);
#else
            return !Equals(left, right);
#endif
        }

        public static int GetTag(Option<T> option)
        {
            return IsNone(option) ? Tags.None : Tags.Some;
        }

        public static class Tags
        {
            public const int None = 0;
            public const int Some = 1;
        }

        public bool Equals(object other, IEqualityComparer comparer)
        {
#if !USESTRUCT
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
#endif
            return other is Option<T> && _Equals((Option<T>) other, comparer);
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(Value);
        }

        public bool Equals(Option<T> other)
        {
#if !USESTRUCT
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
#endif
            return _Equals(other, EqualityComparer<T>.Default);
        }

        private bool _Equals(Option<T> other, IEqualityComparer comparer)
        {
            return comparer.Equals(Value, other.Value);
        }

        public static Option<T> Some(T value)
        {
            return new Option<T>(value);
        }

        public static bool IsNone(Option<T> option)
        {
#if USESTRUCT
            return option == default(Option<T>);
#else
            return option == null || ReferenceEquals(None, option);
#endif
        }

        public static bool IsSome(Option<T> option)
        {
#if USESTRUCT
            return option != default(Option<T>);
#else
            return option != null && !ReferenceEquals(None, option);
#endif
        }

        public override bool Equals(object obj)
        {
#if USESTRUCT
            if (ReferenceEquals(null, obj)) return false;        
#endif
            return obj is Option<T> && Equals((Option<T>) obj);
        }

        public override int GetHashCode()
        {
            return GetHashCode(EqualityComparer<T>.Default);
        }

        public override string ToString()
        {
            object value = Value;
            if (value == null)
                return "Some(null)";

            var formattable = value as IFormattable;
            if (formattable != null)
                return $"Some({formattable.ToString(null, CultureInfo.InvariantCulture)})";
            return $"Some({Value})";
        }

        public int CompareTo(object obj)
        {
#if USESTRUCT
            return CompareTo((Option<T>)obj);
#else
            return CompareTo(obj as Option<T>);
#endif
        }

        public int CompareTo(Option<T> other)
        {
            if (other == default(Option<T>))
                return 1;
            return Comparer<T>.Default.Compare(Value, other.Value);
        }

        public int CompareTo(object other, IComparer comparer)
        {
#if USESTRUCT
            var otherOption = (Option<T>)other;
#else
            var otherOption = other as Option<T>;
#endif
            if (otherOption == default(Option<T>))
                return 1;
            return comparer.Compare(Value, otherOption.Value);
        }
    }
}