#include <stdio.h>
#include <stdlib.h>
#include <time.h>

void merge_sort(int *arr, int size, int start, int end) {
	if (start < 0 || start >= size || end < 0 || end > size) return; // invalid params
	if (end == 0) end = size;
	
	if (end - start < 2) return;
	
	int m = (start + end) / 2;
	
	merge_sort(arr, size, start, m);
	merge_sort(arr, size, m, end);
	
	int *builder = malloc((end - start) * sizeof *builder);

	int index1 = start, index2 = m;
	for (int i = 0; i < end - start; i++) {

		if (index1 < m && index2 < end) {
			builder[i] = (arr[index1] <= arr[index2]) ? arr[index1++] : arr[index2++];
		} else if (index1 >= m) {
			builder[i] = arr[index2++];
		} else {
			builder[i] = arr[index1++];
		}

	}
	
	int j = 0;
	for (int i = start; i < end; i++) {
		arr[i] = builder[j++];
	}
	
	free(builder);

	return;
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
	for (int i = 0; i < SIZE; i++) {
		to_sort[i] = i;
	}

	double avg = 0;
	double ms;
	for (int i = 0; i < 100; i++) {
		// fill_arr_rand(to_sort, SIZE);
		
		clock_t dt = clock();
		merge_sort(to_sort, SIZE, 0, 0);
		dt = clock() - dt;
		
		ms = (double)dt / CLOCKS_PER_SEC * 1000/*ms/s*/;
		printf("Trial %d time (ms): %f\n", i, ms);
		avg += ms / 100.;
		// PRINT AVG
	}
	printf("Average for 100 trials (ms): %f\n", avg);
	free(to_sort);
    
    //for (int i = 0; i < 8; i++) {
    //    printf("%d ", to_sort[i]);
    //}
    //printf("\n");
    
    return EXIT_SUCCESS;
}