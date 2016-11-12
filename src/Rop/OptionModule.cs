using System;
using System.Collections.Generic;

namespace Rop
{
    public static class OptionModule
    {
        public static T GetValue<T>(this Option<T> option)
        {
            if (Option<T>.IsSome(option))
                return option.Value;
            throw new ArgumentException("The option value was None", nameof(option));
        }

        public static bool IsSome<T>(this Option<T> option)
        {
            return Option<T>.IsSome(option);
        }

        public static bool IsNone<T>(this Option<T> option)
        {
            return Option<T>.IsNone(option);
        }

        public static int Count<T>(this Option<T> option)
        {
            return Option<T>.IsSome(option) ? 1 : 0;
        }

        public static TState Fold<T, TState>(this Option<T> option, Func<TState, Func<T, TState>> folder, TState state)
        {
            if (Option<T>.IsNone(option))
                return state;
            return folder(state)(option.Value);
        }

        public static TState FoldBack<T, TState>(Func<T, Func<TState, TState>> folder, Option<T> option, TState state)
        {
            if (Option<T>.IsNone(option))
                return state;
            return folder(option.Value)(state);
        }

        public static bool Exists<T>(Func<T, bool> predicate, Option<T> option)
        {
            if (Option<T>.IsNone(option))
                return false;
            return predicate(option.Value);
        }

        public static bool ForAll<T>(Func<T, bool> predicate, Option<T> option)
        {
            if (Option<T>.IsNone(option))
                return true;
            return predicate.Invoke(option.Value);
        }

        public static void Iterate<T>(Action<T> action, Option<T> option)
        {
            if (Option<T>.IsNone(option))
                return;
            action.Invoke(option.Value);
        }

        public static Option<TResult> Map<T, TResult>(Func<T, TResult> mapping, Option<T> option)
        {
            if (Option<T>.IsNone(option))
                return Option<TResult>.None;
            T func = option.Value;
            return Option<TResult>.Some(mapping.Invoke(func));
        }
        public static Option<TResult> Bind<T, TResult>(this Option<T> option, Func<T, Option<TResult>> binder)
        {
            if (Option<T>.IsNone(option))
                return Option<TResult>.None;
            Option<T> fsharpOption = option;
            return binder.Invoke(fsharpOption.Value);
        }

        public static Option<T> Filter<T>(Func<T, bool> predicate, Option<T> option)
        {
            if (Option<T>.IsNone(option))
                return Option<T>.None;
            T func = option.Value;
            if (predicate.Invoke(func))
                return Option<T>.Some(func);
            return Option<T>.None;
        }

        public static T[] ToArray<T>(Option<T> option)
        {
            if (Option<T>.IsNone(option))
                return new T[0];
            return new[] { option.Value };
        }

        public static List<T> ToList<T>(Option<T> option)
        {
            if (Option<T>.IsSome(option))
                return new List<T>(new[] { option.Value });
            return new List<T>();
        }

        public static LinkedList<T> ToLinkedList<T>(Option<T> option)
        {
            if (Option<T>.IsSome(option))
                return new LinkedList<T>(new[] { option.Value });
            return new LinkedList<T>();
        }

        public static T? ToNullable<T>(Option<T> option) where T : struct
        {
            if (Option<T>.IsSome(option))
                return option.Value;
            return new T?();
        }

        public static T ToObj<T>(Option<T> option) where T : class
        {
            if (Option<T>.IsSome(option))
                return option.Value;
            return default(T);
        }
    }
}