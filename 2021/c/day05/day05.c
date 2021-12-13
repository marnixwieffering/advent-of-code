#include <math.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>

#define FILENAME "sample"

struct vec2 {
	int x;
	int y;
};

struct line {
	struct vec2 start;
	struct vec2 end;
};

struct line *new_vec(size_t length) {
	struct line *vec = malloc(length * sizeof(struct line));
	for (size_t i = 0; i < length; i++) {
		vec[i].start.x = 0;
		vec[i].start.y = 0;
		vec[i].end.y = 0;
		vec[i].end.y = 0;
	}
	return vec;
}

int **new_mat2(size_t n) {
	int *row = malloc(n * n * sizeof(int));
	int **col = malloc(n * sizeof(int *));
	for (size_t i = 0; i < n; i++) {
		col[i] = row + i * n;
	}
	return col;
}
void free_mat2(int **mat) {
	free(*mat);
	free(mat);
}

int part1() {
	FILE *file = fopen(FILENAME, "r");
	if (file == NULL) {
		printf("File \"%s\" does not exist!\n", FILENAME);
		return 0;
	}

	size_t length = 64;
	char *card_line = malloc(length);
	size_t count = 0;
	while (fgets(card_line, length, file) != NULL)
		count++;
	rewind(file);

	struct line *lines = new_vec(count);
	size_t max = 0;
	for (size_t i = 0; i < count; i++) {
		int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
		/* clang-format off */
		fscanf(file, "%d", &x1); getc(file); fscanf(file, "%d", &y1);
		getc(file);	getc(file);	getc(file);	getc(file);
		fscanf(file, "%d", &x2); getc(file); fscanf(file, "%d", &y2);
		/* clang-format on */
		if ((size_t)x1 > max) max = x1;
		if ((size_t)x2 > max) max = x2;
		if ((size_t)y1 > max) max = y1;
		if ((size_t)y2 > max) max = y2;
		lines[i].start.x = x1;
		lines[i].start.y = y1;
		lines[i].end.x = x2;
		lines[i].end.y = y2;
	}
	max++;
	fclose(file);

	int **grid = new_mat2(max);

	for (size_t i = 0; i < count; i++) {
		if (lines[i].start.x != lines[i].end.x && lines[i].start.y != lines[i].end.y) continue;
		if (lines[i].start.x != lines[i].end.x) {
			bool asc = lines[i].start.x - lines[i].end.x < 0;
			for (size_t j = lines[i].start.x; asc ? j <= (size_t)lines[i].end.x : j >= (size_t)lines[i].end.x;
				 asc ? j++ : j--)
				grid[lines[i].start.y][j] += 1;
		}
		if (lines[i].start.y != lines[i].end.y) {
			bool asc = lines[i].start.y - lines[i].end.y < 0;
			for (size_t j = lines[i].start.y; asc ? j <= (size_t)lines[i].end.y : j >= (size_t)lines[i].end.y;
				 asc ? j++ : j--)
				grid[j][lines[i].start.x] += 1;
		}
	}

	int result = 0;

	for (size_t i = 0; i < max; i++)
		for (size_t j = 0; j < max; j++)
			if (grid[i][j] >= 2) result += 1;

	free(lines);
	free_mat2(grid);
	printf("Part 1: result: %u \n", result);

	return 1;
}

int part2() {
	FILE *file = fopen(FILENAME, "r");
	if (file == NULL) {
		printf("File \"%s\" does not exist!\n", FILENAME);
		return 0;
	}

	size_t length = 64;
	char *card_line = malloc(length);
	size_t count = 0;
	while (fgets(card_line, length, file) != NULL)
		count++;
	rewind(file);

	struct line *lines = new_vec(count);
	size_t max = 0;
	for (size_t i = 0; i < count; i++) {
		int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
		/* clang-format off */
		fscanf(file, "%d", &x1); getc(file); fscanf(file, "%d", &y1);
		getc(file);	getc(file);	getc(file);	getc(file);
		fscanf(file, "%d", &x2); getc(file); fscanf(file, "%d", &y2);
		/* clang-format on */
		if ((size_t)x1 > max) max = x1;
		if ((size_t)x2 > max) max = x2;
		if ((size_t)y1 > max) max = y1;
		if ((size_t)y2 > max) max = y2;
		lines[i].start.x = x1;
		lines[i].start.y = y1;
		lines[i].end.x = x2;
		lines[i].end.y = y2;
	}
	max++;
	fclose(file);

	int **grid = new_mat2(max);

	for (size_t i = 0; i < count; i++) {
		if (lines[i].start.x != lines[i].end.x && lines[i].start.y != lines[i].end.y) {
			bool asc_x = lines[i].start.x - lines[i].end.x < 0;
			bool asc_y = lines[i].start.y - lines[i].end.y < 0;
			size_t dif = abs(lines[i].start.y - lines[i].end.y);
			size_t x = lines[i].start.x;
			size_t y = lines[i].start.y;
			for (int j = 0; j <= dif; j++) {
				grid[y][x] += 1;
				asc_x ? x++ : x--;
				asc_y ? y++ : y--;
			}
		} else if (lines[i].start.x != lines[i].end.x) {
			bool asc = lines[i].start.x - lines[i].end.x < 0;
			for (size_t j = lines[i].start.x; asc ? j <= (size_t)lines[i].end.x : j >= (size_t)lines[i].end.x;
				 asc ? j++ : j--)
				grid[lines[i].start.y][j] += 1;
		} else if (lines[i].start.y != lines[i].end.y) {
			bool asc = lines[i].start.y - lines[i].end.y < 0;
			for (size_t j = lines[i].start.y; asc ? j <= (size_t)lines[i].end.y : j >= (size_t)lines[i].end.y;
				 asc ? j++ : j--)
				grid[j][lines[i].start.x] += 1;
		}
	}

	int result = 0;

	for (size_t i = 0; i < max; i++)
		for (size_t j = 0; j < max; j++)
			if (grid[i][j] >= 2) result += 1;

	free(lines);
	free_mat2(grid);
	printf("Part 2: result: %u \n", result);

	return 1;
}

int main(void) {
	if (part1() == 0) return 1;
	if (part2() == 0) return 1;

	return 0;
}
