using System;
using System.Collections.Generic;
using System.Linq;

namespace CucuTools
{
    public struct CucuMath
    {
        public static void LinSpace(float a, float b, float[] array)
        {
            var resolution = array.Length;

            if (resolution == 0) return;
            if (resolution == 1)
            {
                array[0] = (a + b) * 0.5f;
                return;
            }

            if (resolution == 2)
            {
                array[0] = a;
                array[1] = b;
                return;
            }

            var step = (b - a) / (resolution - 1);
            for (var i = 0; i < resolution; i++)
            {
                array[i] = a + i * step;
            }
        }

        public static void LinSpace(float[] array)
        {
            LinSpace(0f, 1f, array);
        }

        public static float[] LinSpace(float a, float b, int resolution = 8)
        {
            var array = new float[resolution];

            LinSpace(a, b, array);

            return array;
        }

        public static float[] LinSpace(int resolution = 8)
        {
            return LinSpace(0f, 1f, resolution);
        }

        #region Search Range

        public static bool SearchRange<T>(out int left, out int right, T value, params T[] sortedArray)
            where T : IComparable
        {
            left = -1;
            right = sortedArray.Length;

            for (var i = 0; i < sortedArray.Length; i++)
            {
                if (left < 0 && sortedArray[sortedArray.Length - 1 - i].CompareTo(value) <= 0)
                {
                    left = sortedArray.Length - 1 - i;
                }

                if (sortedArray.Length <= right && sortedArray[i].CompareTo(value) >= 0)
                {
                    right = i;
                }

                if (0 <= left && right < sortedArray.Length)
                {
                    if (left == right)
                    {
                        if (left > 0)
                        {
                            left--;
                        }
                        else if (right < sortedArray.Length - 1)
                        {
                            right++;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        public static bool SearchRange<T>(out int left, out int right, T value, List<T> sortedList)
            where T : IComparable
        {
            left = -1;
            right = sortedList.Count;

            for (var i = 0; i < sortedList.Count; i++)
            {
                if (left < 0 && sortedList[sortedList.Count - 1 - i].CompareTo(value) <= 0)
                {
                    left = sortedList.Count - 1 - i;
                }

                if (sortedList.Count <= right && sortedList[i].CompareTo(value) >= 0)
                {
                    right = i;
                }

                if (0 <= left && right < sortedList.Count)
                {
                    if (left == right)
                    {
                        if (left > 0)
                        {
                            left--;
                        }
                        else if (right < sortedList.Count - 1)
                        {
                            right++;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        public static bool SearchRange(out int left, out int right, float value, params float[] values)
        {
            return SearchRange<float>(out left, out right, value, values);
        }

        public static bool SearchRange(out int left, out int right, int value, params int[] values)
        {
            return SearchRange<int>(out left, out right, value, values);
        }

        public static bool SearchRange<T>(out int left, out int right, T value, IEnumerable<T> values)
            where T : IComparable
        {
            return SearchRange<T>(out left, out right, value, values.ToArray());
        }

        #endregion
    }
}