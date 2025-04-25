using System;
using System.Diagnostics;

namespace MergeSort {
    class Program {
        static Random rand = new Random(Environment.TickCount);

        static void pause() => Console.ReadKey();
        
        static void _MergeSort(ref int[] arr, ref int[] buffer, int start, int end) {
            if (end - start < 2) return; // if len(section to sort) = 0 or 1, return, base case

            int midpoint = (start + end) / 2; // midpoint of region to sort

            _MergeSort(ref arr, ref buffer, start, midpoint);
            _MergeSort(ref arr, ref buffer, midpoint, end);

            int index1 = start, index2 = midpoint;
            for (int i = 0; i < end - start; i++) {
                if (index1 < midpoint && index2 < end) {
                    buffer[i] = (arr[index1] <= arr[index2]) ? arr[index1++] : arr[index2++];
                } else if (index1 >= midpoint) {
                    buffer[i] = arr[index2++];
                } else { // index2 must (should) be >= end
                    buffer[i] = arr[index1++];
                }
            }

            int j = 0;
            for (int i = start; i < end; i++) {
                arr[i] = buffer[j++];
            }
        }

        /**
         * sorts an section of an array from start (inclusive) to end (exclusive)
         * start and end are optional, start will default to 0 and end will default to arr.Length
         */
        static void MergeSort(ref int[] arr, int start = 0, int end = 0) {
            if (start < 0 || start >= arr.Length || end < 0 || end > arr.Length) return; // invalid params
            if (end == 0) end = arr.Length;

            int[] buffer = new int[end - start];
            _MergeSort(ref arr, ref buffer, start, end);
            

            return; // returns
        }

        static void FillArrRand(ref int[] arr) {
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = rand.Next(32768); // C RAND_MAX
            }
        }

        static void Main(string[] args) {
            const int SIZE = 333_333_333;
            int[] to_sort = new int[SIZE];

            double avg = 0;
            Stopwatch sw = new Stopwatch();
            for (int i = 0; i < 100; i++) {
                // FillArrRand(ref to_sort);

                sw.Restart();
                MergeSort(ref to_sort);
                sw.Stop();

                Console.WriteLine("Trail " + i + " time (ms): " + (double)sw.ElapsedMilliseconds);
                avg += sw.ElapsedMilliseconds / 100.0;
            }
            Console.WriteLine("Average for 100 trials: " + avg);

            // Console.WriteLine("{{ {0} }}", string.Join(", ", to_sort));
            pause();
        }
    }
}
