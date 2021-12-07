#include <limits.h>
#include <math.h>
#include <stdio.h>
#include <stdlib.h>

#define FILENAME "input"

int *new_vec(size_t length) {
	int *vec = malloc(length * sizeof(int));
	for (size_t i = 0; i < length; i++) {
		vec[i] = 0;
	}
	return vec;
}

int part1() {
	FILE *file = fopen(FILENAME, "r");
	if (file == NULL) {
		printf("File \"%s\" does not exist!\n", FILENAME);
		return 0;
	}

	size_t count = 0;
	int crab = 0;
	while (fscanf(file, "%d", &crab) > 0) {
		getc(file);
		count++;
	}
	rewind(file);

	int *crabs = new_vec(count);

	for (size_t i = 0; i < count; i++) {
		fscanf(file, "%d", &crab);
		getc(file);
		crabs[i] = crab;
	}
	fclose(file);

	int fuel = 0;
	int lest_fuel = INT_MAX;
	for (size_t i = 0; i < count; i++) {
		for (size_t j = 0; j < count; j++) {
			fuel += abs(crabs[j] - (int)i);
		}
		if (lest_fuel > fuel) lest_fuel = fuel;
		fuel = 0;
	}

	printf("Part 1: lest amount of fuel: %u \n", lest_fuel);
	return 1;
}

int part2() {
	FILE *file = fopen(FILENAME, "r");
	if (file == NULL) {
		printf("File \"%s\" does not exist!\n", FILENAME);
		return 0;
	}

	size_t count = 0;
	int crab = 0;
	while (fscanf(file, "%d", &crab) > 0) {
		getc(file);
		count++;
	}
	rewind(file);

	int *crabs = new_vec(count);

	for (size_t i = 0; i < count; i++) {
		fscanf(file, "%d", &crab);
		getc(file);
		crabs[i] = crab;
	}
	fclose(file);

	int fuel = 0;
	int lest_fuel = INT_MAX;
	for (size_t i = 0; i < count; i++) {
		for (size_t j = 0; j < count; j++) {
			int x = abs(crabs[j] - (int)i);
			fuel += (int)(0.5 * pow(x, 2) + 0.5 * x);
		}
		if (lest_fuel > fuel) lest_fuel = fuel;
		fuel = 0;
	}

	printf("Part 2: lest amount of fuel: %u \n", lest_fuel);
	return 1;
}

int main(void) {
	if (part1() == 0) return 1;
	if (part2() == 0) return 1;

	return 0;
}
