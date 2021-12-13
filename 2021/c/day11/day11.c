#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define FILENAME "sample"
#define X 10
#define MAXSTEPS 500
#define PART1STEPS 100

int *new_vec(size_t length) {
	int *vec = malloc(length * sizeof(int));
	for (size_t i = 0; i < length; i++) {
		vec[i] = -1;
	}
	return vec;
}
int **new_mat2(size_t m, size_t n) {
	int *row = malloc(m * n * sizeof(int));
	int **col = malloc(m * sizeof(int *));
	for (size_t i = 0; i < m; i++) {
		col[i] = row + i * n;
	}
	return col;
}
void free_mat2(int **mat) {
	free(*mat);
	free(mat);
}

void flash(size_t i, size_t j, int **map, size_t count, size_t *score, bool flashed) {
	bool flash_n = false;
	if (map[i][j] != 0 && flashed) map[i][j] += 1;
	if (map[i][j] > 9) {
		*score = *score + 1;
		flash_n = true;
		map[i][j] = 0;
	}

	/* clang-format off */
	// * up down left right
	if (i != 0)			if (flash_n) flash(i - 1, j, map, count, score, true);
	if (i + 1 != count)	if (flash_n) flash(i + 1, j, map, count, score, true);
	if (j != 0)			if (flash_n) flash(i, j - 1, map, count, score, true);
	if (j + 1 != X)		if (flash_n) flash(i, j + 1, map, count, score, true);

	// * diagonals
	if (i != 0) {
		if (j != 0)		if (flash_n) flash(i - 1, j - 1, map, count, score, true);
		if (j + 1 != X)	if (flash_n) flash(i - 1, j + 1, map, count, score, true);
	}
	if (i + 1 != count) {
		if (j != 0)		if (flash_n) flash(i + 1, j - 1, map, count, score, true);
		if (j + 1 != X)	if (flash_n) flash(i + 1, j + 1, map, count, score, true);
	}
	/* clang-format on */
}

int part1(int **map, size_t count) {
	size_t *score = malloc(sizeof(size_t));
	*score = 0;

	for (size_t c = 0; c < PART1STEPS; c++) {
		for (size_t i = 0; i < count; i++)
			for (size_t j = 0; j < X; j++)
				map[i][j] += 1;
		for (size_t i = 0; i < count; i++)
			for (size_t j = 0; j < X; j++)
				flash(i, j, map, count, score, false);
	}

	printf("Part 1: score: %lu \n", *score);
	return 1;
}

int part2(int **map, size_t count) {
	size_t *score = malloc(sizeof(size_t));
	*score = 0;
	int step = 0;

	for (size_t c = PART1STEPS; c < MAXSTEPS; c++) {
		for (size_t i = 0; i < count; i++)
			for (size_t j = 0; j < X; j++)
				map[i][j] += 1;
		for (size_t i = 0; i < count; i++)
			for (size_t j = 0; j < X; j++)
				flash(i, j, map, count, score, false);

		size_t zero_count = 0;
		for (size_t i = 0; i < count; i++)
			for (size_t j = 0; j < X; j++)
				if (map[i][j] == 0) zero_count++;

		if (zero_count == count * X) {
			step = c + 1;
			break;
		}
	}

	printf("Part 2: step: %u \n", step);
	return 1;
}

int main(void) {
	FILE *file = fopen(FILENAME, "r");
	if (file == NULL) {
		printf("File \"%s\" does not exist!\n", FILENAME);
		return 0;
	}

	size_t length = X + 2;
	char *line = malloc(length);
	size_t count = 0;
	while (fgets(line, length, file) != NULL)
		count++;
	rewind(file);

	int **map = new_mat2(count, X);

	char height = 0;
	for (size_t i = 0; i < count; i++) {
		for (size_t j = 0; j < X; j++) {
			height = getc(file);
			map[i][j] = strtol(&height, NULL, 10);
		}
		getc(file); // captures '\n'
	}
	fclose(file);

	if (part1(map, count) == 0) return 1;
	if (part2(map, count) == 0) return 1;

	free_mat2(map);

	return 0;
}
