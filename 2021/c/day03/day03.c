#include <math.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>

#define CHECK_BIT(var, pos) ((var) & (1 << (pos)))
#define FILENAME "input"
#define N 12

bool bit_count(size_t length, size_t index, int *input_vec) {
	int tg = 0, ntg = 0;
	for (size_t i = 0; i < length; i++) {
		if (input_vec[i] == -1) continue;
		CHECK_BIT(input_vec[i], index) == (int)pow(2, index) ? tg++ : ntg++;
	}
	return tg >= ntg;
}

int *new_vec(size_t length) {
	int *vec = malloc(length * sizeof(int));
	for (size_t i = 0; i < length; i++)
		vec[i] = 0;
	return vec;
}

int *vec_copy(size_t length, int *input_vec) {
	int *output_vec = malloc(length * sizeof(int));
	for (size_t i = 0; i < length; i++) {
		output_vec[i] = input_vec[i];
	}
	return output_vec;
}

int vec_where(size_t length, bool flip, int *input_vec) {
	size_t count = 0;
	for (int index = N - 1; index >= 0; index--) {
		count = 0;
		int target = (int)bit_count(length, index, input_vec) * (int)pow(2, index);
		for (size_t i = 0; i < length; i++) {
			if (input_vec[i] == -1) continue;
			int mask = CHECK_BIT(input_vec[i], index);
			if (flip ? mask == target : mask != target) {
				count++;
			} else
				input_vec[i] = -1;
		}
		if (count == 1) {
			for (size_t i = 0; i < length; i++) {
				if (input_vec[i] == -1) continue;
				return input_vec[i];
			}
		}
	}
	return -1;
}

int part1() {
	FILE *file = fopen(FILENAME, "r");
	if (file == NULL) {
		printf("File \"%s\" does not exist!\n", FILENAME);
		return 0;
	}

	size_t total = 0;
	size_t length = N + 2;
	char *line = malloc(N);
	while (fgets(line, length, file) != NULL)
		total++;
	rewind(file);

	int *values = new_vec(total);
	int index = 0;
	while (fgets(line, length, file) != NULL) {
		values[index++] = strtol(line, NULL, 2);
	}

	free(line);
	fclose(file);

	int gamma = 0;
	for (size_t i = 0; i < N; i++)
		if (bit_count(total, i, values)) gamma += pow(2, i);
	int epsilon = pow(2, N - 1) * 2 - 1 - gamma;
	printf("Part 1: gamma: %u, epsilon: %u, power consumption: %u\n", gamma, epsilon, gamma * epsilon);
	free(values);
	return 1;
}

int part2() {
	FILE *file = fopen(FILENAME, "r");
	if (file == NULL) {
		printf("File \"%s\" does not exist!\n", FILENAME);
		return 0;
	}
	size_t total = 0;
	size_t length = N + 2;
	char *line = malloc(N);
	while (fgets(line, length, file) != NULL)
		total++;
	rewind(file);

	int *oxygen = new_vec(total);
	int index = 0;
	while (fgets(line, length, file) != NULL) {
		oxygen[index++] = strtol(line, NULL, 2);
	}
	free(line);
	fclose(file);

	int *co2 = vec_copy(total, oxygen);

	int oxygen_rating = vec_where(total, true, oxygen);
	int co2_rating = vec_where(total, false, co2);

	free(oxygen);
	free(co2);

	printf("Part 2: Oxygen rating: %u, CO2 rating: %u, mul: %u\n", oxygen_rating, co2_rating,
		   oxygen_rating * co2_rating);
	return 1;
}

int main(void) {
	if (part1() == 0) return 1;
	if (part2() == 0) return 1;

	return 0;
}
