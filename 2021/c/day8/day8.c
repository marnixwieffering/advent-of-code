#include <limits.h>
#include <math.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define FILENAME "input"
#define PATTERNCOUNT 10
#define OUTPUTCOUNT 4
#define SEGMENTCOUNT 7

int *new_vec(size_t length) {
	int *vec = malloc(length * sizeof(int));
	for (size_t i = 0; i < length; i++) {
		vec[i] = 0;
	}
	return vec;
}
int sum(int *vec, size_t length) {
	int sum = 0;
	for (size_t i = 0; i < length; i++)
		sum += vec[i];
	return sum;
}
int **new_mat2(size_t m, size_t n) {
	int *row = malloc(m * n * sizeof(int));
	int **col = malloc(m * sizeof(int *));
	for (size_t i = 0; i < m; i++) {
		col[i] = row + i * n;
	}
	return col;
}
int ***new_vec_mat2(size_t length, int m, int n) {
	int ***vec = malloc(length * sizeof(int **));
	for (size_t i = 0; i < length; i++)
		vec[i] = new_mat2(m, n);
	return vec;
}
void free_mat2(int **mat) {
	free(*mat);
	free(mat);
}
void free_vec_mat2(size_t length, int ***mat2) {
	for (size_t i = 0; i < length; i++)
		free_mat2(mat2[i]);
}

void set_order_buffer(int **patterns, int *order_buffer) {
	// Set order buffer
	int j_six = 0;
	int j_five = 0;
	for (size_t j = 0; j < PATTERNCOUNT; j++) {
		/* clang-format off */
		if (sum(patterns[j], SEGMENTCOUNT) == 2) order_buffer[0] = j; // 1
		else if (sum(patterns[j], SEGMENTCOUNT) == 3) order_buffer[1] = j; // 7
		else if (sum(patterns[j], SEGMENTCOUNT) == 4) order_buffer[2] = j; // 4
		else if (sum(patterns[j], SEGMENTCOUNT) == 7) order_buffer[3] = j; // 8
		else if (sum(patterns[j], SEGMENTCOUNT) == 6) order_buffer[4+j_six++] = j; // 0 6 9
		else order_buffer[7+j_five++] = j; // 2 3 5
		/* clang-format on */
	}
}

bool bit_pressent_in_four(int index, int *four) { return four[index] == 1; }

bool bit_pressent_in_one(int index, int *one) { return one[index] == 1; }

bool bits_pressent_in_four(int one, int two, int *four) { return (four[one] == 1 && four[two] == 1); }

bool bits_pressent_in_six(int one, int two, int *six) { return (six[one] == 1 && six[two] == 1); }

int get_off_8_bit_index(int *pattern, int *eight) {
	int off_bit_index = 0;
	for (size_t i = 0; i < SEGMENTCOUNT; i++) {
		if (pattern[i] != eight[i]) off_bit_index = i;
	}
	return off_bit_index;
}

int *get_off_8_bits_indicies(int *pattern, int *eight) {
	int *off_bit_indicies = new_vec(2);
	int c = 0;
	for (size_t i = 0; i < SEGMENTCOUNT; i++) {
		if (pattern[i] != eight[i]) off_bit_indicies[c++] = i;
	}
	return off_bit_indicies;
}

void set_connection_buffer(int **patterns, int **connection_buffer, int *order_buffer) {
	connection_buffer[1] = patterns[order_buffer[0]]; // First number is always 1
	connection_buffer[7] = patterns[order_buffer[1]]; // second number is always 7
	connection_buffer[4] = patterns[order_buffer[2]]; // third number is always 4
	connection_buffer[8] = patterns[order_buffer[3]]; // fourth number is always 8
	for (size_t j = 4; j < 7; j++) {
		/* clang-format off */
		int off_8_bit_index = get_off_8_bit_index(patterns[order_buffer[j]], connection_buffer[8]);
		bool in_1 = bit_pressent_in_one(off_8_bit_index, connection_buffer[1]);
		bool in_4 = bit_pressent_in_four(off_8_bit_index, connection_buffer[4]);
		if (in_1) connection_buffer[6] = patterns[order_buffer[j]];
		else if (!in_1 && in_4) connection_buffer[0] = patterns[order_buffer[j]];
		else connection_buffer[9] = patterns[order_buffer[j]];
		/* clang-format on */
	}
	for (size_t j = 7; j < PATTERNCOUNT; j++) {
		/* clang-format off */
		int* off_8_bit_indicies = get_off_8_bits_indicies(patterns[order_buffer[j]], connection_buffer[8]);
		bool in_4 = bits_pressent_in_four(off_8_bit_indicies[0], off_8_bit_indicies[1], connection_buffer[4]);
		bool in_6 = bits_pressent_in_six(off_8_bit_indicies[0], off_8_bit_indicies[1], connection_buffer[6]);
		if (in_4) connection_buffer[2] = patterns[order_buffer[j]];
		else if (in_6) connection_buffer[3] = patterns[order_buffer[j]];
		else connection_buffer[5] = patterns[order_buffer[j]];

		free(off_8_bit_indicies);
		/* clang-format on */
	}
}

int get_decoded_sum(int **patterns, int **connection_buffer) {
	int sum = 0;
	int power = 1000;
	for (size_t i = PATTERNCOUNT; i < OUTPUTCOUNT + PATTERNCOUNT; i++) {
		for (size_t j = 0; j < PATTERNCOUNT; j++) {
			bool hit = true;
			for (size_t k = 0; k < SEGMENTCOUNT; k++)
			{
				hit &= connection_buffer[j][k] == patterns[i][k];
			}
			
			if (hit) {
				sum += j * power;
				power = power / 10;
			}
		}
	}
	return sum;
}

int part1(int ***patterns, size_t count) {
	int result = 0;
	for (size_t i = 0; i < count; i++) {
		for (size_t j = PATTERNCOUNT; j < PATTERNCOUNT + OUTPUTCOUNT; j++) {
			int sum = 0;
			for (size_t k = 0; k < SEGMENTCOUNT; k++) {
				sum += patterns[i][j][k];
			}
			if (sum == 2 || sum == 3 || sum == 4 || sum == 7) result++;
		}
	}

	printf("Part 1: result: %u \n", result);
	return 1;
}

int part2(int ***patterns, size_t count) {
	int *order_buffer = new_vec(PATTERNCOUNT);
	int **connection_buffer = new_mat2(PATTERNCOUNT, SEGMENTCOUNT);

	int sum = 0;

	for (size_t i = 0; i < count; i++) {
		// Set order buffer for deducing numbers
		set_order_buffer(patterns[i], order_buffer);
		// Deduce numbers and store in connection buffer
		set_connection_buffer(patterns[i], connection_buffer, order_buffer);
		// Calculate the sum value
		sum += get_decoded_sum(patterns[i], connection_buffer);
	}

	free(order_buffer);

	printf("Part 2: result: %u \n", sum);
	return 1;
}

int main(void) {
	FILE *file = fopen(FILENAME, "r");
	if (file == NULL) {
		printf("File \"%s\" does not exist!\n", FILENAME);
		return 0;
	}

	size_t length = 128;
	char *line = malloc(length);
	size_t count = 0;
	while (fgets(line, length, file) != NULL)
		count++;
	rewind(file);

	int ***patterns = new_vec_mat2(count, PATTERNCOUNT + OUTPUTCOUNT, SEGMENTCOUNT);
	char *current = malloc(length);

	for (size_t i = 0; i < count; i++) {
		for (size_t j = 0; j < PATTERNCOUNT + OUTPUTCOUNT; j++) {
			if (j == PATTERNCOUNT) fscanf(file, "%s", current); // Captures |
			fscanf(file, "%s", current);
			size_t l = strlen(current);
			for (size_t k = 0; k < l; k++) {
				/* clang-format off */
				if (current[k] == 'a') patterns[i][j][0] = 1;
				else if (current[k] == 'b')	patterns[i][j][1] = 1;
				else if (current[k] == 'c')	patterns[i][j][2] = 1;
				else if (current[k] == 'd')	patterns[i][j][3] = 1;
				else if (current[k] == 'e')	patterns[i][j][4] = 1;
				else if (current[k] == 'f')	patterns[i][j][5] = 1;
				else if (current[k] == 'g')	patterns[i][j][6] = 1;
				/* clang-format on */
			}
		}
	}
	free(current);
	fclose(file);

	if (part1(patterns, count) == 0) return 1;
	if (part2(patterns, count) == 0) return 1;

	free_vec_mat2(count, patterns);

	return 0;
}
