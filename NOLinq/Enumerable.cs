using System;
using System.Collections.Generic;

namespace NOLinq
{
    ///<summary></summary>
    public static class Enumerable
    {
        ///<summary>Returns a subset without elements that match the specified criteria.</summary>
        public static IEnumerable<T> Without<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
        {
            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (!predicate(enumerator.Current))
                    {
                        yield return enumerator.Current;
                    }
                }
            }
        }

        ///<summary>Returns a subset without elements that match the specified criteria.</summary>
        public static IEnumerable<T> Without<T>(this IEnumerable<T> enumerable, IndexedPredicate<T> indexedPredicate)
        {
            int counter = 0;

            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (!indexedPredicate(enumerator.Current, counter))
                    {
                        yield return enumerator.Current;
                    }

                    counter++;
                }
            }
        }

        ///<summary>Sorts a sequence of elements of T where T is IComparable of T.</summary>
        public static IEnumerable<T> Sort<T>(this IEnumerable<T> enumerable, SortOption sortOption = SortOption.Default) where T : IComparable<T>
        {
            int count = 0;
            T[] array;

            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {

                while (enumerator.MoveNext())
                {
                    count++;
                }

                array = new T[count];

                enumerator.Reset();

                for (int i = 0; i < count; i++)
                {
                    enumerator.MoveNext();
                    array[i] = enumerator.Current;
                }
            }

            for (int i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < count - 1; j++)
                {
                    int comparerInt = ((IComparable<T>)array[j])?.CompareTo(array[j + 1]) ?? -1;

                    if (comparerInt > 0 && (int)sortOption == 0)
                    {
                        (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    }

                    if (comparerInt < 0 && (int)sortOption == 1)
                    {
                        (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    }
                }
            }

            return array;
        }

        ///<summary>Sorts a sequence of elements of T given IComparer of T.</summary>
        public static IEnumerable<T> Sort<T>(this IEnumerable<T> enumerable, IComparer<T> comparer)
        {
            int count = 0;
            T[] array;

            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {

                while (enumerator.MoveNext())
                {
                    count++;
                }

                array = new T[count];

                enumerator.Reset();

                for (int i = 0; i < count; i++)
                {
                    enumerator.MoveNext();
                    array[i] = enumerator.Current;
                }
            }

            for (int i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < count - 1; j++)
                {
                    if (comparer.Compare(array[j], array[j + 1]) > 0)
                    {
                        (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    }
                }
            }

            return array;
        }

        ///<summary>Returns the specified number of elements from the end of a sequence.</summary>
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> enumerable, int amount)
        {
            int count = 0;

            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    count++;
                }

                enumerator.Reset();

                if (count < amount)
                {
                    amount = count;
                }

                while (count >= amount)
                {
                    enumerator.MoveNext();
                    count--;
                }

                do
                {
                    yield return enumerator.Current;
                } while (enumerator.MoveNext());
            }
        }

        ///<summary>Returns a subset without the specified number of elements at the end.</summary>
        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> enumerable, int amount)
        {
            int count = 0;

            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    count++;
                }

                enumerator.Reset();

                while (enumerator.MoveNext() && count > amount)
                {
                    yield return enumerator.Current;
                    count--;
                }
            }
        }

        ///<summary>Returns a subset of elements until the specified criteria is true.</summary>
        public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
        {
            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (!predicate(enumerator.Current))
                    {
                        yield return enumerator.Current;
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
        }

        ///<summary>Returns a subset of elements until the specified criteria is true.</summary>
        public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> enumerable, IndexedPredicate<T> indexedPredicate)
        {
            int counter = 0;

            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (!indexedPredicate(enumerator.Current, counter))
                    {
                        yield return enumerator.Current;
                    }
                    else
                    {
                        yield break;
                    }

                    counter++;
                }
            }
        }

        ///<summary>Returns a subset of elements while the specified criteria is true.</summary>
        public static IEnumerable<T> SkipUntil<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
        {
            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                do
                {
                    if (!enumerator.MoveNext())
                    {
                        yield break;
                    }
                } while (!predicate(enumerator.Current));

                do
                {
                    yield return enumerator.Current;
                } while (enumerator.MoveNext());
            }
        }

        ///<summary>Returns a subset of elements while the specified criteria is true.</summary>
        public static IEnumerable<T> SkipUntil<T>(this IEnumerable<T> enumerable, IndexedPredicate<T> indexedPredicate)
        {
            int counter = -1;

            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                do
                {
                    if (!enumerator.MoveNext())
                    {
                        yield break;
                    }

                    counter++;
                } while (!indexedPredicate(enumerator.Current, counter));

                do
                {
                    yield return enumerator.Current;
                } while (enumerator.MoveNext());
            }
        }

        ///<summary>Applied the specified action for every element in a sequence.</summary>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T variable in enumerable)
            {
                action(variable);
            }
        }

        ///<summary>Applied the specified action for every element in a sequence.</summary>
        public static void ForEach<T>(this IEnumerable<T> enumerable, IndexedAction<T> indexedAction)
        {
            int count = 0;

            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    count++;
                }

                enumerator.Reset();

                for (int i = 0; i < count; i++)
                {
                    enumerator.MoveNext();
                    indexedAction(enumerator.Current, i);
                }
            }
        }

        ///<summary>Returns a subset of a sequence given start index and length.</summary>
        public static IEnumerable<T> Subset<T>(this IEnumerable<T> enumerable, int startIndex, int length)
        {
            int currentIndex = -1;

            if (startIndex + length <= 0)
            {
                yield break;
            }

            if (startIndex < 0)
            {
                (startIndex, length) = (0, startIndex + length);
            }

            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                do
                {
                    if (enumerator.MoveNext())
                    {
                        currentIndex++;
                    }
                    else
                    {
                        yield break;
                    }
                } while (currentIndex < startIndex);

                do
                {
                    yield return enumerator.Current;
                    currentIndex++;
                } while (enumerator.MoveNext() && currentIndex < startIndex + length);
            }
        }

        ///<summary>Returns a sequence without the subset with given start index and length.</summary>
        public static IEnumerable<T> RemoveSubset<T>(this IEnumerable<T> enumerable, int startIndex, int length)
        {
            int currentIndex = -1;

            if (startIndex + length <= 0)
            {
                yield break;
            }

            if (startIndex < 0)
            {
                (startIndex, length) = (0, startIndex + length);
            }

            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                while (currentIndex < startIndex - 1)
                {
                    if (enumerator.MoveNext())
                    {
                        currentIndex++;
                        yield return enumerator.Current;
                    }
                    else
                    {
                        yield break;
                    }
                }

                do
                {
                    currentIndex++;
                    if (!enumerator.MoveNext())
                    {
                        yield break;
                    }
                } while (currentIndex < startIndex + length);

                do
                {
                    yield return enumerator.Current;
                } while (enumerator.MoveNext());
            }
        }
    }

    ///<summary>An enum with sorting options.</summary>
    public enum SortOption
    {
        ///<summary>The default is ascending.</summary>
        Default = 0,
        ///<summary>Sort elements ascendingly</summary>
        Ascending = 0,
        ///<summary>The default is descendingly.</summary>
        Descending = 1
    }

    ///<summary></summary>
    public delegate bool Predicate<in T>(T item);

    ///<summary></summary>
    public delegate bool IndexedPredicate<in T>(T item, int index);

    ///<summary></summary>
    public delegate void Action<in T>(T item);

    ///<summary></summary>
    public delegate void IndexedAction<in T>(T item, int index);
}
