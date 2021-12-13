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
int64_t *new_int_vec(size_t length) {
	int64_t *vec = malloc(length * sizeof(int64_t));
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
		case '{': return true;
		case '<': return true;
	}
	/* clang-format on */
	return false;
}

char matching(char match) {
	/* clang-format off */
	switch (match) {
		case '(': return ')';
		case '[': return ']';
		case '{': return '}';
		case '<': return '>';
	}
	/* clang-format on */
	return 0;
}

int64_t penalty(char match) {
	/* clang-format off */
	switch (match) {
		case ')': return 3;
		case ']': return 57;
		case '}': return 1197;
		case '>': return 25137;
	}
	/* clang-format on */
	return 0;
}

int64_t complete_score(char match) {
	/* clang-format off */
	switch (match) {
		case ')': return 1;
		case ']': return 2;
		case '}': return 3;
		case '>': return 4;
	}
	/* clang-format on */
	return 0;
}

int part1(FILE *file) {
	size_t length = 256;
	char *line = malloc(length);
	int64_t score = 0;
	char *stack = new_vec(length);
	size_t stack_idx = 0;
	while (fgets(line, length, file) != NULL) {
		size_t l = strlen(line);
		for (size_t i = 0; i < l; i++) {
			if (open(line[i])) {
				stack[stack_idx++] = line[i];
			} else if (matching(stack[stack_idx - 1]) != line[i]) {
				score += penalty(line[i]);
				break;
			} else {
				stack_idx--;
			}
		}
	}
	printf("Part 1: result: %lu \n", score);

	free(line);
	free(stack);

	return 1;
}

int compare(const void *a, const void *b) { return *((int *)b) - *((int *)a); }

int part2(FILE *file) {
	size_t length = 256;
	char *line = malloc(length);
	int64_t line_score = 0;
	int64_t *scores = new_int_vec(length);
	size_t score_idx = 0;
	char *stack = new_vec(length);
	size_t stack_idx = 0;
	bool corrupt = false;
	while (fgets(line, length, file) != NULL) {
		size_t l = strlen(line);
		corrupt = false;
		line_score = 0;
		stack_idx = 0;
		for (size_t i = 0; i < l; i++) {
			if (open(line[i]))
				stack[stack_idx++] = line[i];
			else if (matching(stack[stack_idx - 1]) != line[i] && penalty(line[i]) != 0) {
				corrupt = true;
				break;
			} else
				stack_idx--;
		}
		if (!corrupt && stack_idx != 0) {
			for (; stack_idx > 0; stack_idx--)
				line_score = line_score * 5 + complete_score(matching(stack[stack_idx]));
			scores[score_idx++] = line_score;
		}
	}

	qsort(scores, score_idx, sizeof(int64_t), compare);

	printf("Part 2: result: %lu \n", scores[score_idx / 2]);

	free(line);
	free(stack);
	return 1;
}

int main(void) {
	FILE *file = fopen(FILENAME, "r");
	if (file == NULL) {
		printf("File \"%s\" does not exist!\n", FILENAME);
		return 0;
	}

	if (part1(file) == 0) return 1;
	rewind(file);
	if (part2(file) == 0) return 1;

	fclose(file);

	return 0;
}
