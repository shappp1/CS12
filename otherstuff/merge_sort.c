#include <stdio.h>
#include <stdlib.h>
#include <time.h>

// it is expected that buffer has at least a size of (end - start)
void _merge_sort(int *arr, int *buffer, int start, int end) {
	if (end - start < 2) return;

	int midpoint = (start + end) / 2;

	_merge_sort(arr, buffer, start, midpoint);
	_merge_sort(arr, buffer, midpoint, end);

	int index1 = start, index2 = midpoint;
	for (int i = 0; i < end - start; i++) {
		if (index1 < midpoint && index2 < end) {
			buffer[i] = (arr[index1] <= arr[index2]) ? arr[index1++] : arr[index2++];
		} else if (index1 >= midpoint) {
			buffer[i] = arr[index2++];
		} else {
			buffer[i] = arr[index1++];
		}
	}

	int j = 0;
	for (int i = start; i < end; i++) {
		arr[i] = buffer[j++];
	}
}

void merge_sort(int *arr, int start, int end) {
	int *buffer = malloc((end - start) * sizeof *buffer);
	_merge_sort(arr, buffer, start, end);
	free(buffer);
}

void fill_arr_rand(int *arr, int size) {
	for (int i = 0; i < size; i++) {
		arr[i] = rand();
	}
}

int main() {
    srand(time(NULL));
	printf("RAND_MAX: %d\n", RAND_MAX);
	
	const int SIZE = 5000000;
    int *to_sort = calloc(SIZE, sizeof *to_sort);
	// for (int i = 0; i < SIZE; i++) {
	// 	to_sort[i] = i;
	// }

	double avg = 0;
	double ms;
	for (int i = 0; i < 10; i++) {
		fill_arr_rand(to_sort, SIZE);
		
		clock_t dt = clock();
		merge_sort(to_sort, 0, SIZE);
		dt = clock() - dt;
		
		ms = (double)dt / CLOCKS_PER_SEC * 1000/*ms/s*/;
		printf("Trial %d time (ms): %f\n", i, ms);
		avg += ms / 100.;
	}
	printf("Average for 100 trials (ms): %f\n", avg);
    
    // for (int i = 0; i < SIZE; i++) {
    //    printf("%d ", to_sort[i]);
    // }
    // printf("\n");
    
	free(to_sort);
    return EXIT_SUCCESS;
}