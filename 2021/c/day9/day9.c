#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define FILENAME "input"
#define X 100

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

int part1(int **map, size_t count) {

	int risc_level = 0;

	for (size_t i = 0; i < count; i++) {
		for (size_t j = 0; j < X; j++) {
			bool lowest_point = true;
			if (i != 0) lowest_point &= map[i][j] < map[i - 1][j];
			if (i + 1 != count) lowest_point &= map[i][j] < map[i + 1][j];
			if (j != 0) lowest_point &= map[i][j] < map[i][j - 1];
			if (j + 1 != X) lowest_point &= map[i][j] < map[i][j + 1];

			if (lowest_point) risc_level = risc_level + 1 + map[i][j];
		}
	}

	printf("Part 1: risc_level: %u \n", risc_level);
	return 1;
}

bool part_of_basin(size_t i, size_t j, int *captured, size_t *capture_count) {
	bool in_basin = false;
	for (size_t k = 0; k < *capture_count; k++) {
		if (captured[k] == (int)(i * X + j)) in_basin = true;
	}
	return in_basin;
}

int find_basin(size_t i, size_t j, int **map, size_t count, int *captured, size_t *capture_count) {
	size_t basin_count = 0;

	if (!part_of_basin(i, j, captured, capture_count) && 9 != map[i][j]) {
		captured[*capture_count] = i * X + j;
		*capture_count += 1;
		basin_count++;
		/* clang-format off */
		if (i != 0 && 9 != map[i - 1][j])
			basin_count += find_basin(i - 1, j, map, count, captured, capture_count);
		if (i + 1 != count && 9 != map[i + 1][j])
			basin_count += find_basin(i + 1, j, map, count, captured, capture_count);
		if (j != 0 && 9 != map[i][j - 1])
			basin_count += find_basin(i, j - 1, map, count, captured, capture_count);
		if (j + 1 != X && 9 != map[i][j + 1])
			basin_count += find_basin(i, j + 1, map, count, captured, capture_count);
		/* clang-format on */
	}
	return basin_count;
}

int compare(const void* a, const void* b) {
	return *((int*)b)-*((int*)a);
}

int part2(int **map, size_t count) {

	int *captured = new_vec(count * X);
	size_t *capture_count = malloc(sizeof(size_t));
	*capture_count = 0;
	int *basin_sizes = new_vec(count * X);
	size_t basin_sizes_count = 0;

	for (size_t i = 0; i < count; i++) {
		for (size_t j = 0; j < X; j++) {
			if (map[i][j] != 9) {
				int size = find_basin(i, j, map, count, captured, capture_count);
				if (size != 0) basin_sizes[basin_sizes_count++] = size;
			}
		}
	}

	free(capture_count);
	free(captured);

	qsort(basin_sizes, basin_sizes_count, sizeof(int), compare);

	int result = 1;
	for (size_t i = 0; i < 3; i++) {
		result *= basin_sizes[i];
	}

	free(basin_sizes);

	printf("Part 2: result: %u \n", result);
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
