/**
 *   SHANE GOODRICK
 *   
 *   1-D DYNAMIC PROGRAMMING PART 2
 */

using System;
using System.Collections.Generic;

namespace DP1Dp2 {
    class Program {
        static void pause() {
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }

        static int[] LongestIncreasingSubsequence(int[] seq) {
            if (seq.Length == 0) return new int[] { };
            (int length, int prev_index)[] subseq = new (int, int)[seq.Length];

            subseq[0].length = 1;

            int max = 1;
            int max_index = 0;
            for (int i = 1; i < seq.Length; i++) {
                subseq[i].length = 0;
                for (int j = 0; j < i; j++) {
                    if (seq[i] > seq[j] && subseq[i].length < subseq[j].length) {
                        subseq[i].length = subseq[j].length;
                        subseq[i].prev_index = j;
                    }
                }
                subseq[i].length++;

                // keep track of final index of longest subsequence
                if (subseq[i].length > max) {
                    max = subseq[i].length;
                    max_index = i;
                }
            }

            int[] ret = new int[max];
            int index = max_index;
            for (int i = max - 1; i >= 0; i--) {
                ret[i] = index;
                index = subseq[index].prev_index;
            }

            return ret;
        }

        enum Steps {
            ADD_1, ADD_4, DOUBLE
        }
        static Steps[] DoubleOrPlusPlusOrPlusEqualsFour(int n) {
            (int min_steps, Steps step_from_prev)[] steps = new (int, Steps)[n + 1];

            steps[0].min_steps = 0;

            for (int i = 1; i <= n; i++) {
                steps[i].min_steps = steps[i - 1].min_steps;
                steps[i].step_from_prev = Steps.ADD_1;

                if (i >= 4 && steps[i - 4].min_steps < steps[i].min_steps) {
                    steps[i].min_steps = steps[i - 4].min_steps;
                    steps[i].step_from_prev = Steps.ADD_4;
                }

                if (i % 2 == 0 && steps[i / 2].min_steps < steps[i].min_steps) {
                    steps[i].min_steps = steps[i / 2].min_steps;
                    steps[i].step_from_prev = Steps.DOUBLE;
                }

                steps[i].min_steps++;
            }

            Steps[] ret = new Steps[steps[n].min_steps];
            int index = n;
            for (int i = steps[n].min_steps - 1; i >= 0; i--) {
                ret[i] = steps[index].step_from_prev;

                switch (steps[index].step_from_prev) {
                    case Steps.ADD_1:
                        index = index - 1;
                        break;
                    case Steps.ADD_4:
                        index = index - 4;
                        break;
                    case Steps.DOUBLE:
                        index = index / 2;
                        break;
                }
            }

            return ret;
        }

        static int[] SubsetSum(int[] nums, int sum) {
            // sum must be non-negative
            if (sum < 0) return new int[] { -1 };

            // special zero check
            if (sum == 0)
                return new int[] { Array.IndexOf(nums, 0) }; // IndexOf returns -1 if element not found so this works regardless of zero being in nums

            // nums_index[i] is the index into nums
            int[] nums_index = new int[sum + 1];
            for (int i = 1; i <= sum; i++) {
                nums_index[i] = -1;
            }

            for (int i = 0; i < nums.Length; i++) {
                if (nums[i] == 0) continue; // 0 is bad

                for (int j = sum - nums[i]; j >= 0; j--) {
                    if (nums_index[j] != -1) {
                        nums_index[nums[i] + j] = i;
                    }
                }
            }

            if (nums_index[sum] == -1) return new int[] { -1 };

            Stack<int> ret = new Stack<int>();
            int index = sum;
            while (index != 0) {
                ret.Push(nums_index[index]);

                index -= nums[nums_index[index]];
            }

            return ret.ToArray();
        }

        static void TestLongestIncreasingSubsequence() {
            Console.WriteLine("LongestIncreasingSubsequence:");
            Console.WriteLine("{ }: { " + string.Join(", ", LongestIncreasingSubsequence(new int[] { })) + " } expecting { }");
            Console.WriteLine("{ 1 }: { " + string.Join(", ", LongestIncreasingSubsequence(new int[] { 1 })) + " } expecting { 0 }");
            Console.WriteLine("{ 3, 1 }: { " + string.Join(", ", LongestIncreasingSubsequence(new int[] { 3, 1 })) + " } expecting { 0 } OR { 1 }");
            Console.WriteLine("{ 1, 3 }: { " + string.Join(", ", LongestIncreasingSubsequence(new int[] { 1, 3 })) + " } expecting { 0, 1 }");
            Console.WriteLine("{ 1, 5, 1, 1, 5 }: { " + string.Join(", ", LongestIncreasingSubsequence(new int[] { 1, 5, 1, 1, 5 })) + " } expecting { 0, 1 } OR { 2, 4 } OR { 3, 4 }");
            Console.WriteLine("{ INT_MIN, INT_MAX }: { " + string.Join(", ", LongestIncreasingSubsequence(new int[] { int.MinValue, int.MaxValue })) + " } expecting { 0, 1 }");
            Console.WriteLine("{ INT_MAX, INT_MIN }: { " + string.Join(", ", LongestIncreasingSubsequence(new int[] { int.MaxValue, int.MinValue })) + " } expecting { 0 } OR { 1 }");
            Console.WriteLine("{ 0, 0, 0, 0, }: { " + string.Join(", ", LongestIncreasingSubsequence(new int[] { 0, 0, 0, 0 })) + " } expecting { 0 } OR { 1 } OR { 2 } OR { 3 }");
            Console.WriteLine();
        }

        static void TestDoubleOrPlusPlusOrPlusEqualsFour() {
            Console.WriteLine("TripleOrPlusPlus:");
            Console.WriteLine("0: { " + string.Join(", ", DoubleOrPlusPlusOrPlusEqualsFour(0)) + " } expecting { }");
            Console.WriteLine("1: { " + string.Join(", ", DoubleOrPlusPlusOrPlusEqualsFour(1)) + " } expecting { ADD_1 }");
            Console.WriteLine("5: { " + string.Join(", ", DoubleOrPlusPlusOrPlusEqualsFour(5)) + " } expecting { ADD_1, ADD_4 } OR { ADD_4, ADD_1 }");
            Console.WriteLine("532: { " + string.Join(", ", DoubleOrPlusPlusOrPlusEqualsFour(532)) + " } expecting 10 elements");
            Console.WriteLine();
        }

        static void TestSubsetSum() {
            Console.WriteLine("SubsetSum:");
            Console.WriteLine("{ }, 0: { " + string.Join(", ", SubsetSum(new int[] { }, 0)) + " } expecting { -1 }");
            Console.WriteLine("{ }, 17: { " + string.Join(", ", SubsetSum(new int[] { }, 17)) + " } expecting { -1 }");
            Console.WriteLine("{ 2, 7, 3 }, 0: { " + string.Join(", ", SubsetSum(new int[] { 2, 7, 3 }, 0)) + " } expecting { -1 }");
            Console.WriteLine("{ 2, 7, 3 }, 9: { " + string.Join(", ", SubsetSum(new int[] { 2, 7, 3 }, 9)) + " } expecting { 0, 1 }");
            Console.WriteLine("{ 2, 7, 3 }, 10: { " + string.Join(", ", SubsetSum(new int[] { 2, 7, 3 }, 10)) + " } expecting { 1, 2 }");
            Console.WriteLine();
        }

        static void Main(string[] args) {
            TestLongestIncreasingSubsequence();
            TestDoubleOrPlusPlusOrPlusEqualsFour();
            TestSubsetSum();

            pause();
        }
    }
}
