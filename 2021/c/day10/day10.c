#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define FILENAME "input"

char *new_vec(size_t length) {
	char *vec = malloc(length * sizeof(char));
	for (size_t i = 0; i < length; i++) {
		vec[i] = 0;
	}
	return vec;
}

bool open(char match) {
	/* clang-format off */
	switch (match) {
		case '(': return true;
		case '[': return true;
		case '<': return true;
		case '{': return true;
	}
	/* clang-format on */
	return false;
}

char matching(char match) {
	/* clang-format off */
	switch (match) {
		case '(': return ')';
		case '[': return ']';
		case '<': return '>';
		case '{': return '}';
	}
	/* clang-format on */
	return 0;
}

int64_t penalty(char match) {
	/* clang-format off */
	switch (match) {
		case ')': return 3;
		case ']': return 57;
		case '>': return 1107;
		case '}': return 25137;
	}
	/* clang-format on */
	return 0;
}

bool TOKEN(char *line, size_t *idx, char target) {
	if (line[*idx] == target) {
		*idx = *idx + 1;
		return true;
	}
	return false;
}

int part1() {
	FILE *file = fopen(FILENAME, "r");
	if (file == NULL) {
		printf("File \"%s\" does not exist!\n", FILENAME);
		return 0;
	}

	size_t length = 128;
	char *line = malloc(length);
	int64_t score = 0;
	char *stack = new_vec(length);
	size_t stack_idx = 0;
	while (fgets(line, length, file) != NULL) {
		size_t l = strlen(line);
		for (size_t i = 0; i < l; i++) {
			if (open(line[l])) {
				stack[stack_idx++] = line[i];
			} else if (matching(stack[stack_idx - 1]) != line[l]) {
				score += penalty(stack[stack_idx]);
				break;
			} else {
				stack_idx--;
			}
		}
	}
	fclose(file);

	printf("Part 1: result: %lu \n", score);
	free(stack);

	return 1;
}

int part2() {

	printf("Part 2: result: %u \n", 0);
	return 1;
}

int main(void) {
	if (part1() == 0) return 1;
	if (part2() == 0) return 1;

	return 0;
}
