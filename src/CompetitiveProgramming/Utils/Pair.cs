using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 値を書き換えやすいなTupleぐらいのイメージ
    /// </summary>
    public class Pair<T, U> : IComparable<Pair<T, U>>, IEquatable<Pair<T, U>>
        where T : IComparable<T>
        where U : IComparable<U>
    {
        public T V1 { get; set; }
        public U V2 { get; set; }

        public Pair(T v1, U v2)
        {
            this.V1 = v1;
            this.V2 = v2;
        }

        public Pair(Pair<T, U> src) : this(src.V1, src.V2) { }

        public int CompareTo(Pair<T, U> other)
        {
            var r = this.V1.CompareTo(other.V1);
            if (r == 0) r = this.V2.CompareTo(other.V2);

            return r;
        }

        public bool Equals(Pair<T, U> other)
            => (other != null && this.CompareTo(other) == 0);

        public override string ToString()
            => $"{V1} {V2}";

        public override bool Equals(object obj)
            => Equals(obj as Pair<T, U>);

        public override int GetHashCode()
        {
            var hashCode = 1989511945;
            hashCode = hashCode * -1521134295 + V1.GetHashCode();
            hashCode = hashCode * -1521134295 + V2.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Pair<T, U> a, Pair<T, U> b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }

            return a.Equals(b);
        }

        public static bool operator !=(Pair<T, U> a, Pair<T, U> b)
            => !(a == b);
        public static bool operator <(Pair<T, U> a, Pair<T, U> b)
            => a.CompareTo(b) < 0;
        public static bool operator <=(Pair<T, U> a, Pair<T, U> b)
            => a.CompareTo(b) <= 0;
        public static bool operator >(Pair<T, U> a, Pair<T, U> b)
            => a.CompareTo(b) > 0;
        public static bool operator >=(Pair<T, U> a, Pair<T, U> b)
            => a.CompareTo(b) >= 0;
    }
}
