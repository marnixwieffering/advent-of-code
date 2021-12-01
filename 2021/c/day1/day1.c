#include <stdio.h>
#define FILENAME "input"
#define N 5000

int dephts[N];
size_t length = 0;

int parse() {
	FILE *file = fopen(FILENAME, "r");
	if (file == NULL) {
		printf("File \"%s\" does not exist!\n", FILENAME);
		return 0;
	}
	int num = 0;
	while (fscanf(file, "%d", &num) > 0)
		dephts[length++] = num;
	fclose(file);
	return 1;
}

void part1() {
	int count = 0;
	for (size_t i = 1; i < length; i++)
		if (dephts[i - 1] < dephts[i]) count++;
	printf("Part 1: Amount of increases: %u\n", count);
}

int sum(size_t i, size_t window) {
	int sum = 0;
	for (size_t j = 0; j < window; j++)
		sum += dephts[i + j];
	return sum;
}

void part2() {
	int count = 0;
	size_t window = 3;
	for (size_t i = 0; i + window < length; i++) {
		int sum_a = sum(i, window);
		int sum_b = sum(i + 1, window);
		if (sum_a < sum_b) count++;
	}
	printf("Part 2: Amount of increases: %u\n", count);
}

int main(void) {
	if (parse() == 0) return 1;

	part1();
	part2();

	return 0;
}
