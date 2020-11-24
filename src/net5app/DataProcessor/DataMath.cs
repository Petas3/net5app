using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace net5app
{
    internal static class DataMath
    {
        /// <summary>
        /// Computes simple average
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        internal static double? GetAverage(byte[] array)
        {
            if (array.Length == 0)
                return null;

            ulong sum = 0;
            foreach (byte b in array)
                sum += b;
            return (Convert.ToDouble(sum) / array.Length);
        }

        /// <summary>
        /// Computes simple weighed average
        /// </summary>
        /// <param name="array"></param>
        /// <param name="weighs"></param>
        /// <returns></returns>
        internal static double? GetAverage(double[] array, int[] weighs)
        {
            if (array.Length != weighs.Length)
                throw new Exception("array.Length != weighs.Length");

            if (array.Length == 0)
                return null;

            double sum = 0;
            for (int i = 0; i < array.Length; i++)
                sum += array[i] * weighs[i];

            return sum / weighs.Sum();
        }

        /// <summary>
        /// Returns modus of array given min/max value information
        /// </summary>
        /// <param name="array"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        internal static Collection<byte> GetModus(byte[] array, byte minValue, byte maxValue)
        {
            if (array.Length <= 1)
                return null;

            int len = maxValue - minValue + 1;
            //Autoinit to 0
            int[] bins = new int[len];

            foreach (byte b in array)
                bins[b - minValue] += 1;

            //Find maximum and sweep values to array
            Collection<byte> maxValues = new Collection<byte>();
            int maxValuesMax = 0;

            for (byte i = 0; i < len; i++)
                if (bins[i] > 1)
                {
                    if (bins[i] > maxValuesMax)
                    {
                        maxValues.Clear();
                        maxValues.Add((byte)(i + minValue));
                        maxValuesMax = bins[i];
                    }
                    else if (bins[i] == maxValuesMax)
                    {
                        maxValues.Add((byte)(i + minValue));
                    }
                }

            if (maxValues.Any())
                return maxValues;
            else
                return null;
        }

        internal static double? GetMedian(byte[] array)
        {
            if (array.Length == 0)
                return null;

            if (array.Length == 1)
                return array[0];

            return QuickSelectMedian(array);
        }

        private static double NLognMedian(byte[] array)
        {
            Array.Sort(array);
            return SortedMedian(array);
        }

        private static double NLognMedian(double[] array)
        {
            Array.Sort(array);
            return SortedMedian(array);
        }

        private static double SortedMedian(byte[] array)
        {
            if (array.Length % 2 == 1)
                return array[array.Length / 2];
            else
                return 0.5 * (array[array.Length / 2 - 1] + array[array.Length / 2]);
        }

        private static double SortedMedian(double[] array)
        {
            if (array.Length % 2 == 1)
                return array[array.Length / 2];
            else
                return 0.5 * (array[array.Length / 2 - 1] + array[array.Length / 2]);
        }

        private static double QuickSelectMedian(byte[] array, double? pivotGuess = null)
        {
            if (array.Length % 2 == 1)
                return QuickSelect(array, (byte)Math.Floor((decimal)array.Length / 2), pivotGuess);
            else
                return 0.5 * (QuickSelect(array, (byte)Math.Floor((decimal)array.Length / 2 - 1), pivotGuess) + QuickSelect(array, (byte)Math.Floor((decimal)array.Length / 2), pivotGuess));
        }

        private static double QuickSelectMedian(double[] array)
        {
            if (array.Length % 2 == 1)
                return QuickSelect(array, (byte)Math.Floor((decimal)array.Length / 2));
            else
                return 0.5 * (QuickSelect(array, (byte)Math.Floor((decimal)array.Length / 2 - 1)) + QuickSelect(array, (byte)Math.Floor((decimal)array.Length / 2)));
        }

        private static double QuickSelect(byte[] array, byte k, double? pivotGuess = null)
        {
            if (array.Length == 1)
                return array[0];

            double pivot;
            if (pivotGuess != null)
                pivot = (double)pivotGuess;
            else
                pivot = QuickselectPickPivot(array);

            byte[] lows = array.Where(e => e < pivot).ToArray();
            byte[] highs = array.Where(e => e > pivot).ToArray();
            byte[] pivots = array.Where(e => e == pivot).ToArray();
            
            if (k < lows.Length)
                return QuickSelect(lows, k);
            else if (k < lows.Length + pivots.Length)
                return pivots[0];
            else
                return QuickSelect(highs, (byte)(k - lows.Length - pivots.Length));
        }

        private static double QuickSelect(double[] array, byte k)
        {
            if (array.Length == 1)
                return array[0];

            double pivot = QuickselectPickPivot(array);

            double[] lows = array.Where(e => e < pivot).ToArray();
            double[] highs = array.Where(e => e > pivot).ToArray();
            double[] pivots = array.Where(e => e == pivot).ToArray();

            if (k < lows.Length)
                return QuickSelect(lows, k);
            else if (k < lows.Length + pivots.Length)
                return pivots[0];
            else
                return QuickSelect(highs, (byte)(k - lows.Length - pivots.Length));
        }

        private static double QuickselectPickPivot(byte[] array)
        {
            if (array.Length < 5)
                return NLognMedian(array);

            //Split
            Collection<byte[]> chunks = QuickselectChunked(array, 5);
            
            //Sort
            foreach (byte[] chunk in chunks)
                Array.Sort(chunk);

            //Easy median
            double[] medians = chunks.Select(c => SortedMedian(c)).ToArray();

            //Return median of medians
            return QuickSelectMedian(medians);
        }

        private static double QuickselectPickPivot(double[] array)
        {
            if (array.Length < 5)
                return NLognMedian(array);

            //Split
            Collection<double[]> chunks = QuickselectChunked(array, 5);

            //Sort
            foreach (double[] chunk in chunks)
                Array.Sort(chunk);

            //Easy median
            double[] medians = chunks.Select(c => SortedMedian(c)).ToArray();

            //Return median of medians
            return QuickSelectMedian(medians);
        }

        /// <summary>
        /// Split array to arrays of set length
        /// </summary>
        /// <param name="array"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        private static Collection<byte[]> QuickselectChunked(byte[] array, byte len)
        {
            Collection<byte[]> rt = new Collection<byte[]>();
            int i = 0;
            while (i + len < array.Length)
            {
                byte[] chunk = new byte[len];
                for (int j = 0; j < len; j++)
                {
                    chunk[j] = array[i];
                    i++;
                }
                rt.Add(chunk);
            }

            //Add last chunk if any
            int lastLen = array.Length - i;
            if (lastLen > 0)
            {
                byte[] chunk = new byte[lastLen];
                for (int j = 0; j < lastLen; j++)
                {
                    chunk[j] = array[i];
                    i++;
                }
                rt.Add(chunk);
            }
            
            return rt;
        }

        /// <summary>
        /// Split array to arrays of set length
        /// </summary>
        /// <param name="array"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        private static Collection<double[]> QuickselectChunked(double[] array, byte len)
        {
            Collection<double[]> rt = new Collection<double[]>();
            int i = 0;
            while (i + len < array.Length)
            {
                double[] chunk = new double[len];
                for (int j = 0; j < len; j++)
                {
                    chunk[j] = array[i];
                    i++;
                }
                rt.Add(chunk);
            }

            //Add last chunk if any
            int lastLen = array.Length - i;
            if (lastLen > 0)
            {
                double[] chunk = new double[lastLen];
                for (int j = 0; j < lastLen; j++)
                {
                    chunk[j] = array[i];
                    i++;
                }
                rt.Add(chunk);
            }

            return rt;
        }
    }
}