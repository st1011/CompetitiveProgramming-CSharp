using System;
using System.Collections;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// BitArrayのラッパー
    /// </summary>
    struct Bitset
    {
        private readonly BitArray _bits;

        public int this[int i]
        {
            set { _bits[i] = value != 0; }
            get { return _bits[i] ? 1 : 0; }
        }

        public Bitset(int n)
        {
            _bits = new BitArray(n);
        }

        public Bitset(BitArray arr)
        {
            _bits = arr.Clone() as BitArray;
        }

        public static implicit operator long(Bitset a)
        {
            int[] num = new int[2];
            a._bits.CopyTo(num, 0);

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

        public void Reset() => _bits.SetAll(false);

        public override string ToString()
        {
            return Convert.ToString((long)this, 2);
        }
    }
}
