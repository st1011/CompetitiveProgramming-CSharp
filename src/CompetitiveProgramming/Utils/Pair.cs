using System;
using System.Collections.Generic;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 値を書き換えやすいなTupleぐらいのイメージ
    /// </summary>
    public class Pair<T1, T2> : IComparable<Pair<T1, T2>>, IEquatable<Pair<T1, T2>> where T1 : IComparable<T1>
        where T2 : IComparable<T2>
    {
        public T1 V1 { get; set; }
        public T2 V2 { get; set; }

        public Pair(T1 v1, T2 v2)
        {
            V1 = v1;
            V2 = v2;
        }

        public Pair(Pair<T1, T2> src)
        {
            if (src == null) { throw new ArgumentNullException(nameof(src)); }

            V1 = src.V1;
            V2 = src.V2;
        }

        public int CompareTo(Pair<T1, T2> other)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            var r = V1.CompareTo(other.V1);
            if (r == 0) r = V2.CompareTo(other.V2);

            return r;
        }


        public override string ToString()
            => $"{V1} {V2}";

        public override bool Equals(object obj)
        {
            return Equals(obj as Pair<T1, T2>);
        }

        public bool Equals(Pair<T1, T2> other)
        {
            return other != null &&
                   EqualityComparer<T1>.Default.Equals(V1, other.V1) &&
                   EqualityComparer<T2>.Default.Equals(V2, other.V2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 1989511945;
                hashCode = hashCode * -1521134295 + EqualityComparer<T1>.Default.GetHashCode(V1);
                hashCode = hashCode * -1521134295 + EqualityComparer<T2>.Default.GetHashCode(V2);
                return hashCode;
            }
        }

        public static bool operator ==(Pair<T1, T2> a, Pair<T1, T2> b)
            => a != null && a.Equals(b);
        public static bool operator !=(Pair<T1, T2> a, Pair<T1, T2> b)
            => !(a == b);
        public static bool operator <(Pair<T1, T2> a, Pair<T1, T2> b)
            => a?.CompareTo(b) < 0;
        public static bool operator <=(Pair<T1, T2> a, Pair<T1, T2> b)
            => a?.CompareTo(b) <= 0;
        public static bool operator >(Pair<T1, T2> a, Pair<T1, T2> b)
            => a?.CompareTo(b) > 0;
        public static bool operator >=(Pair<T1, T2> a, Pair<T1, T2> b)
            => a?.CompareTo(b) >= 0;
    }
}
