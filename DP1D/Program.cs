/**
 *   SHANE GOODRICK
 *   
 *   1-D DYNAMIC PROGRAMMING
 */



using System;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace DP1D {
    class Program {

        static int LongestIncreasingSubsequence(int[] seq) {
            if (seq.Length == 0) return 0;
            int[] lengths = new int[seq.Length];
            lengths[0] = 1;

            int max = 1;
            for (int i = 1; i < seq.Length; i++) {
                // find longest previous sequence where all numbers in sequence are less than current number
                for (int j = 0; j < i; j++) {
                    // if current number is greater than the last number, than it is still increasing
                    if (seq[i] > seq[j]) {
                        // "borrowed" from Caden
                        // if new sequence we found is longer than others, than it is the new longest subsequence
                        lengths[i] = Math.Max(lengths[i], lengths[j] + 1);
                    }
                }

                // keep track of longest subsequence
                if (lengths[i] > max) max = lengths[i];
            }

            return max;
        }

        /**
         * Returns the shortest amount of operations required to get to n from one by either tripling or adding one to the previous number
         * 
         * @param n The number to get to from one, must be a positive integer
         * @returns The shortest amount of operations required, or -1 if n is invalid
         */
        static int TripleOrPlusPlus(int n) {
            if (n <= 0) return -1;

            int[] op_count = new int[n];
            
            for (int i = 1; i < n; i++) {
                if ((i + 1) % 3 == 0) {
                    op_count[i] = Math.Min(op_count[(i + 1) / 3 - 1], op_count[i - 1]) + 1;
                } else {
                    op_count[i] = op_count[i - 1] + 1;
                }
            }

            return op_count[n - 1];
        }

        /**
         * Returns the shortest amount of operations required to get to n from zero by either tripling, adding one, or adding five to the previous number
         * 
         * @param n The number to get to from zero, must be a non-negative integer
         * @returns The shortest amount of operations required, or -1 if n is negative
         */
        static int TripleOrPlusPlusV2(int n) {
            if (n < 0) return -1;

            int[] op_count = new int[n + 1];

            for (int i = 1; i <= n; i++) {
                op_count[i] = op_count[i - 1] + 1;

                if (i % 3 == 0)
                    op_count[i] = Math.Min(op_count[i], op_count[i / 3] + 1);

                if (i >= 5)
                    op_count[i] = Math.Min(op_count[i], op_count[i - 5] + 1);
            }

            return op_count[n];
        }

        static int NumberOfSteps(int n) {
            if (n < 1) return -1;

            int[] steps = new int[n + 1]; // steps[i] is the number of ways to get to (i) steps
            steps[0] = 1;

            for (int i = 1; i <= n; i++) {
                for (int j = Math.Max(0, i - 4); j < i; j++) { // adds up ways to get to all previous 4 steps (or from 0 if at <4 steps)
                    steps[i] += steps[j];
                }
            }
            return steps[n];
        }

        static bool SubsetSum(int[] nums, int sum) {
            if (sum < 0) return false;
            if (sum == 0) return true;

            bool[] can_make_sum = new bool[sum];

            can_make_sum[0] = true; // assume we can always make a sum of zero

            for (int i = 0; i < nums.Length; i++) {
                if (nums[i] == sum) return true; // found it, return

                // go backwards to prevent number from being reused, only works because negative numbers aren't allowed
                for (int j = sum - 1; j >= 0; j--) {
                    if (can_make_sum[j]) {
                        if (nums[i] + j == sum) return true; // found it, return

                        if (nums[i] + j < sum)
                            can_make_sum[nums[i] + j] = true;
                    }
                }
            }
            
            return false; // if we found the sum, it would have been during the loop

            // old recursive code
            /*
            if (sum < 0) return false;

            for (int i = 0; i < nums.Length; i++) {
                if (nums[i] == sum) return true;
                if (i == nums.Length - 1) return false;
                if (SubsetSum(nums.Skip(i + 1).Take(nums.Length - i - 1).ToArray(), sum - nums[i])) return true;
            }

            return false;*/
        }

        static string ReadIntArrayFile(out int[] integers, string path) {
            try {
                StreamReader sr = new StreamReader(path);

                string[] to_convert = sr.ReadToEnd().Split(',');
                integers = new int[to_convert.Length];

                for (int i = 0; i < to_convert.Length; i++) {
                    integers[i] = int.Parse(to_convert[i]);
                }

            } catch (Exception e) {
                integers = null;
                return e.Message;
            }
            return "Success!";
        }

        static void TestLongestIncreasingSubsequence() {
            Console.WriteLine("LongestIncreasingSubsequence:");
            Console.WriteLine("{ }: " + LongestIncreasingSubsequence(new int[] { }) + " expecting 0");
            Console.WriteLine("{ 1 }: " + LongestIncreasingSubsequence(new int[] { 1 }) + " expecting 1");
            Console.WriteLine("{ 3, 1 }: " + LongestIncreasingSubsequence(new int[] { 3, 1 }) + " expecting 1");
            Console.WriteLine("{ 1, 3 }: " + LongestIncreasingSubsequence(new int[] { 1, 3 }) + " expecting 2");
            Console.WriteLine("{ 1, 5, 1, 1, 5 }: " + LongestIncreasingSubsequence(new int[] { 1, 5, 1, 1, 5 }) + " expecting 2");
            Console.WriteLine("{ INT_MIN, INT_MAX }: " + LongestIncreasingSubsequence(new int[] { int.MinValue, int.MaxValue }) + " expecting 2");
            Console.WriteLine("{ INT_MAX, INT_MIN }: " + LongestIncreasingSubsequence(new int[] { int.MaxValue, int.MinValue }) + " expecting 1");
            Console.WriteLine("{ 0, 0, 0, 0, }: " + LongestIncreasingSubsequence(new int[] { 0, 0, 0, 0 }) + " expecting 1");
            Console.WriteLine();
        }

        static void TestTripleOrPlusPlus() {
            Console.WriteLine("TripleOrPlusPlus:");
            Console.WriteLine("0: " + TripleOrPlusPlus(0) + " expecting -1");
            Console.WriteLine("1: " + TripleOrPlusPlus(1) + " expecting 0");
            Console.WriteLine("5: " + TripleOrPlusPlus(5) + " expecting 3");
            Console.WriteLine("532: " + TripleOrPlusPlus(532) + " expecting 10");
            try {
                Console.WriteLine("INT_MAX: " + TripleOrPlusPlus(int.MaxValue) + " expecting 43");
            } catch (OutOfMemoryException e) {
                Console.WriteLine("INT_MAX: " + e.Message + " expecting OutOfMemoryException");
            }
            Console.WriteLine();
        }

        static void TestTripleOrPlusPlusV2() {
            Console.WriteLine("TripleOrPlusPlus:");
            Console.WriteLine("0: " + TripleOrPlusPlusV2(0) + " expecting 0");
            Console.WriteLine("5: " + TripleOrPlusPlusV2(5) + " expecting 2");
            Console.WriteLine("-2: " + TripleOrPlusPlusV2(-2) + " expecting -1");
            Console.WriteLine("32: " + TripleOrPlusPlusV2(32) + " expecting 5");
            Console.WriteLine("23: " + TripleOrPlusPlusV2(23) + " expecting 4");
            Console.WriteLine();
        }

        static void TestNumberOfSteps() {
            Console.WriteLine("NumberOfSteps:");
            Console.WriteLine("0: " + NumberOfSteps(0) + " expecting -1");
            Console.WriteLine("1: " + NumberOfSteps(1) + " expecting 1");
            Console.WriteLine("4: " + NumberOfSteps(4) + " expecting 8");
            Console.WriteLine("5: " + NumberOfSteps(5) + " expecting 15");
            Console.WriteLine("32: " + NumberOfSteps(32) + " expecting 747044834");
            Console.WriteLine("34: " + NumberOfSteps(34) + " expecting < 0 (int overflow)");
            Console.WriteLine();
        }

        static void TestSubsetSum() {
            Console.WriteLine("SubsetSum:");
            Console.WriteLine("{ }, 0: " + SubsetSum(new int[] { }, 0) + " expecting True");
            Console.WriteLine("{ }, 17: " + SubsetSum(new int[] { }, 17) + " expecting False");
            Console.WriteLine("{ 2, 7, 3 }, 0: " + SubsetSum(new int[] { 2, 7, 3 }, 0) + " expecting True");
            Console.WriteLine("{ 2, 7, 3 }, 9: " + SubsetSum(new int[] { 2, 7, 3 }, 9) + " expecting True");
            Console.WriteLine("{ 2, 7, 3 }, 6: " + SubsetSum(new int[] { 2, 7, 3 }, 6) + " expecting False");
            try {
                Console.WriteLine("{ 2, 7, 3 }, INT_MAX: " + SubsetSum(new int[] { 2, 7, 3 }, int.MaxValue) + " expecting False");
            } catch (OutOfMemoryException e) {
                Console.WriteLine("{ 2, 7, 3 }, INT_MAX: " + e.Message + " expecting OutOfMemoryException");
            }
            Console.WriteLine();
        }

        static void Main(string[] args) {
            TestLongestIncreasingSubsequence();
            TestTripleOrPlusPlus();
            TestTripleOrPlusPlusV2();
            TestNumberOfSteps();
            TestSubsetSum();

            Console.ReadKey();
        }
    }
}
