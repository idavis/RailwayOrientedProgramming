using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rop
{
    [DebuggerStepThrough]
    [DebuggerNonUserCode]
    public static class OptionExtensions
    {
        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static T GetValue<T>(this Option<T> option)
        {
            if (Option<T>.IsSome(option))
                return option.Value;
            throw new ArgumentException("The option value was None", nameof(option));
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static bool IsSome<T>(this Option<T> option)
        {
            return Option<T>.IsSome(option);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static bool IsNone<T>(this Option<T> option)
        {
            return Option<T>.IsNone(option);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static int Count<T>(this Option<T> option)
        {
            return Option<T>.IsSome(option) ? 1 : 0;
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static TState Fold<T, TState>(this Option<T> option, Func<TState, Func<T, TState>> folder, TState state)
        {
            if (Option<T>.IsNone(option))
                return state;
            return folder(state)(option.Value);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static TState FoldBack<T, TState>(this Option<T> option, Func<T, Func<TState, TState>> folder, TState state)
        {
            if (Option<T>.IsNone(option))
                return state;
            return folder(option.Value)(state);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static bool Exists<T>(this Option<T> option, Func<T, bool> predicate)
        {
            if (Option<T>.IsNone(option))
                return false;
            return predicate(option.Value);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static bool ForAll<T>(this Option<T> option, Func<T, bool> predicate)
        {
            if (Option<T>.IsNone(option))
                return true;
            return predicate.Invoke(option.Value);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static void Iterate<T>(this Option<T> option, Action<T> action)
        {
            if (Option<T>.IsNone(option))
                return;
            action.Invoke(option.Value);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Option<TResult> Map<T, TResult>(this Option<T> option, Func<T, TResult> mapping)
        {
            if (Option<T>.IsNone(option))
                return Option<TResult>.None;
            T func = option.Value;
            return Option<TResult>.Some(mapping.Invoke(func));
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Option<TResult> Bind<T, TResult>(this Option<T> option, Func<T, Option<TResult>> binder)
        {
            if (Option<T>.IsNone(option))
                return Option<TResult>.None;
            Option<T> fsharpOption = option;
            return binder.Invoke(fsharpOption.Value);
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static Option<T> Filter<T>(this Option<T> option, Func<T, bool> predicate)
        {
            if (Option<T>.IsNone(option))
                return Option<T>.None;
            T func = option.Value;
            if (predicate.Invoke(func))
                return Option<T>.Some(func);
            return Option<T>.None;
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static T[] ToArray<T>(this Option<T> option)
        {
            if (Option<T>.IsNone(option))
                return new T[0];
            return new[] { option.Value };
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static List<T> ToList<T>(this Option<T> option)
        {
            if (Option<T>.IsSome(option))
                return new List<T>(new[] { option.Value });
            return new List<T>();
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static LinkedList<T> ToLinkedList<T>(this Option<T> option)
        {
            if (Option<T>.IsSome(option))
                return new LinkedList<T>(new[] { option.Value });
            return new LinkedList<T>();
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static T? ToNullable<T>(this Option<T> option) where T : struct
        {
            if (Option<T>.IsSome(option))
                return option.Value;
            return new T?();
        }

        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        public static T ToValueOrDefault<T>(this Option<T> option) where T : class
        {
            if (Option<T>.IsSome(option))
                return option.Value;
            return default(T);
        }
    }
}