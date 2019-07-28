using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// BitArrayのラッパー
    /// </summary>
    struct Bitset
    {
        readonly BitArray Bits;

        public int this[int i]
        {
            set { Bits[i] = value != 0; }
            get { return Bits[i] ? 1 : 0; }
        }

        public Bitset(int n)
        {
            Bits = new BitArray(n);
        }

        Bitset(BitArray arr)
        {
            Bits = arr.Clone() as BitArray;
        }

        public static implicit operator long(Bitset a)
        {
            int[] num = new int[2];
            a.Bits.CopyTo(num, 0);

            long s = 0;
            for (int i = num.Length - 1; i >= 0; i--)
            {
                s <<= 32;
                s += num[i];
            }
            return s;
        }

        public static implicit operator Bitset(long a)
        {
            var bits = new BitArray(new int[] { (int)(a & UInt32.MaxValue), (int)(a >> 32) });

            return new Bitset(bits);
        }

        public void Reset() => Bits.SetAll(false);

        public override string ToString()
        {
            return Convert.ToString((long)this, 2);
        }
    }
}
