#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>

#define FILENAME "input"

int part1() {
	FILE *file = fopen(FILENAME, "r");
	if (file == NULL) {
		printf("File \"%s\" does not exist!\n", FILENAME);
		return 0;
	}
	const size_t len = 10;
	char *line = malloc(len);
	int x = 0, y = 0;
	while (fgets(line, len, file) != NULL) {
		if (strncmp(line, "forward", 7) == 0) {
			x += atoi(&line[8]);
		} else if (strncmp(line, "down", 4) == 0) {
			y += atoi(&line[5]);
		} else if (strncmp(line, "up", 2) == 0) {
			y -= atoi(&line[3]);
		}
	}
	free(line);
	fclose(file);
	printf("Part 1: x: %u, y: %u, sum: %u\n", x, y, x * y);
	return 1;
}

int part2() {
	FILE *file = fopen(FILENAME, "r");
	if (file == NULL) {
		printf("File \"%s\" does not exist!\n", FILENAME);
		return 0;
	}
	const size_t len = 10;
	char *line = malloc(len);
	int x = 0, y = 0, aim = 0;
	while (fgets(line, len, file) != NULL) {
		if (strncmp(line, "forward", 7) == 0) {
			int a = atoi(&line[8]);
			x += a;
			y += a * aim;
		} else if (strncmp(line, "down", 4) == 0) {
			aim += atoi(&line[5]);
		} else if (strncmp(line, "up", 2) == 0) {
			aim -= atoi(&line[3]);
		}
	}
	free(line);
	fclose(file);
	printf("Part 2: x: %u, y: %u, sum: %u\n", x, y, x * y);
	return 1;
}

int main(void) {
	if (part1() == 0) return 1;
	if (part2() == 0) return 1;

	return 0;
}
