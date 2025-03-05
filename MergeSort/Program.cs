using System;
using System.Diagnostics;

namespace MergeSort {
    class Program {
        static Random rand = new Random(Environment.TickCount);

        static void pause() => Console.ReadKey();
        
        /**
         * sorts an array
         * start is inclusive index into arr to start sorting at
         * end is exclusive index to stop sorting at (poor wording ik)
         * start and end are optional, start will default to 0 and end will default to arr.Length
         */
        static void MergeSort(ref int[] arr, int start = 0, int end = 0) {
            if (start < 0 || start >= arr.Length || end < 0 || end > arr.Length) return; // invalid params
            if (end == 0) end = arr.Length;

            if (end - start < 2) return; // if len(section to sort) = 0 or 1, return, base case

            int mid = (start + end) / 2; // midpoint of region to sort

            MergeSort(ref arr, start, mid);
            MergeSort(ref arr, mid, end);

            int[] builder = new int[end - start];

            int index1 = start, index2 = mid;
            for (int i = 0; i < end - start; i++) {
                if (index1 < mid && index2 < end) {
                    builder[i] = (arr[index1] <= arr[index2]) ? arr[index1++] : arr[index2++];
                } else if (index1 >= mid) {
                    builder[i] = arr[index2++];
                } else { // index2 must (should) be >= end
                    builder[i] = arr[index1++];
                }
            }

            int j = 0;
            for (int i = start; i < end; i++) {
                arr[i] = builder[j++];
            }

            return; // returns
        }

        static void FillArrRand(ref int[] arr) {
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = rand.Next(32768); // C RAND_MAX
            }
        }

        static void Main(string[] args) {
            const int SIZE = 5_000_000;
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
