/**
 *   SHANE GOODRICK
 *   
 *   1-D DYNAMIC PROGRAMMING PART 2
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace DP1Dp2 {
    class Program {
        static void pause() {
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }

        static int[] LongestIncreasingSubsequence(int[] seq) {
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

            const int CANT_MAKE_VALUE = -1234;
            const int DONE_VALUE = -1;

            // nums_index[i] is the index into nums
            int[] nums_index = new int[sum + 1];
            for (int i = 0; i <= sum; i++) {
                nums_index[i] = CANT_MAKE_VALUE;
            }

            nums_index[0] = DONE_VALUE;

            for (int i = 0; i < nums.Length; i++) {
                if (nums[i] == 0) continue; // 0 is bad
                
                for (int j = sum - nums[i]; j >= 0; j--) {
                    if (nums_index[j] != CANT_MAKE_VALUE) {
                        nums_index[nums[i] + j] = i;
                    }
                }
            }

            Stack<int> ret = new Stack<int>();
            int index = sum;
            while (nums_index[index] != DONE_VALUE) {
                ret.Push(nums_index[index]);

                index -= nums[nums_index[index]];
            }

            return ret.ToArray();
        }

        static void Main(string[] args) {
            Console.WriteLine("{{ {0} }}", string.Join(", ", SubsetSum(new int[] { 7, 1, 4 }, 11)));

            pause();
        }
    }
}
