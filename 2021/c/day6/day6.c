#include <stdio.h>
#include <stdlib.h>

#define FILENAME "input"

int64_t *new_vec(size_t length) {
	int64_t *vec = malloc(length * sizeof(int64_t));
	for (size_t i = 0; i < length; i++) {
		vec[i] = 0;
	}
	return vec;
}

int populate(size_t days, int part) {
	FILE *file = fopen(FILENAME, "r");
	if (file == NULL) {
		printf("File \"%s\" does not exist!\n", FILENAME);
		return 0;
	}

	int8_t max_age = 9;
	size_t count = 0;
	int age = 0;
	while (fscanf(file, "%d", &age) > 0) {
		getc(file);
		count++;
	}
	rewind(file);

	int64_t *ages = new_vec(max_age);

	for (size_t i = 0; i < count; i++) {
		fscanf(file, "%d", &age);
		getc(file);
		ages[age] += 1;
	}
	fclose(file);

	for (size_t i = 0; i < days; i++) {
		int64_t buffer = ages[0];
		for (size_t j = 0; j < max_age - 1; j++)
			ages[j] = ages[j + 1];
		ages[8] = buffer;
		ages[6] += buffer;
	}

	count = 0;
	for (size_t i = 0; i < max_age; i++) {
		count += ages[i];
	}

	free(ages);
	printf("Part %u: amount of lanternfish: %lu \n", part, count);

	return 1;
}

int part1() {
	return populate(80,1);
}

int part2() {
	return populate(256,1);
}

int main(void) {
	if (part1() == 0) return 1;
	if (part2() == 0) return 1;

	return 0;
}
