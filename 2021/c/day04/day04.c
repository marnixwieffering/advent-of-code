#include <math.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>

#define FILENAME "input"
#define N 5

int *new_vec(size_t length) {
	int *vec = malloc(length * sizeof(int));
	for (size_t i = 0; i < length; i++)
		vec[i] = 0;
	return vec;
}
int **new_mat2() {
	int *row = malloc(N * N * sizeof(int));
	int **col = malloc(N * sizeof(int *));
	for (size_t i = 0; i < N; i++) {
		col[i] = row + i * N;
	}
	return col;
}
int ***bingo_cards(size_t length) {
	int ***vec = malloc(length * sizeof(int **));
	for (size_t i = 0; i < length; i++)
		vec[i] = new_mat2();
	return vec;
}
void free_mat2(int **mat) {
	free(*mat);
	free(mat);
}
void free_bingo_cards(size_t length, int ***bingo_cards) {
	for (size_t i = 0; i < length; i++)
		free_mat2(bingo_cards[i]);
}

void call_number(int number, size_t card_count, int ***bingo_cards) {
	for (size_t card = 0; card < card_count; card++) {
		for (size_t col = 0; col < N; col++) {
			for (size_t row = 0; row < N; row++) {
				if (bingo_cards[card][col][row] == number) {
					bingo_cards[card][col][row] = 101;
				}
			}
		}
	}
}

int check_bingo(size_t card_count, int ***bingo_cards) {
	int col_count = 0, row_count = 0;
	size_t col = 0, row = 0;
	for (size_t card = 0; card < card_count; card++) {
		for (col = 0; col < N; col++) {
			row_count = 0;
			col_count = 0;
			for (row = 0; row < N; row++) {
				if (bingo_cards[card][col][row] == 101)
					row_count += 1;
				else
					row_count = 0;

				if (bingo_cards[card][row][col] == 101)
					col_count += 1;
				else
					col_count = 0;

				if (row_count == N || col_count == N) {
					return card;
				}
			}
		}
	}
	return 101;
}

int last_bingo(size_t card_count, int ***bingo_cards) {
	int col_count = 0, row_count = 0, roof = 0, fac = 0;
	size_t col = 0, row = 0, count = 0;
	for (size_t i = 0; i < card_count; roof += i++) {
	}
	for (size_t card = 0; card < card_count; card++) {
		if (count + 1 == card_count)
		{
			return roof - fac;
		}
		
		for (col = 0; col < N; col++) {
			row_count = 0;
			col_count = 0;
			for (row = 0; row < N; row++) {
				if (bingo_cards[card][col][row] == 101)
					row_count += 1;
				else
					row_count = 0;

				if (bingo_cards[card][row][col] == 101)
					col_count += 1;
				else
					col_count = 0;
			}
			if (row_count == N || col_count == N) {
				count++;
				fac += card;
				break;
			}
		}
	}
	return 101;
}

int get_score(int card, int ***bingo_cards) {
	int score = 0;
	for (size_t col = 0; col < N; col++) {
		for (size_t row = 0; row < N; row++) {
			if (bingo_cards[card][col][row] != 101) score += bingo_cards[card][col][row];
		}
	}
	return score;
}

int play_bingo(int part, int (*rule)(size_t, int***)){
	FILE *file = fopen(FILENAME, "r");
	if (file == NULL) {
		printf("File \"%s\" does not exist!\n", FILENAME);
		return 0;
	}

	int *calls;

	const char tok[] = ",";
	size_t call_count = 0;
	size_t call_length = 290;
	char *call_line = malloc(call_length);
	char *call = malloc(5);
	if (fgets(call_line, call_length, file) != NULL) {
		calls = new_vec(strlen(call_line));
		do {
			int l = strcspn(call_line, tok);
			strncpy(call, call_line, l + 1);
			calls[call_count++] = strtol(call, NULL, 10);
			call_line += l + 1;
		} while (call_line[-1]);
	}
	free(call);
	rewind(file);

	int ***cards;

	size_t card_count = 0;
	size_t card_line_length = N * N * 2;
	char *card_line = malloc(card_line_length);
	while (fgets(card_line, card_line_length, file) != NULL)
		card_count++;
	card_count = (card_count - 2) / 6;
	rewind(file);

	cards = bingo_cards(card_count);

	size_t count = 0;
	int col = 0, row = 0;
	int card_cell;
	int card = 0;
	fgets(card_line, card_line_length, file);
	fgets(card_line, card_line_length, file);
	while (fscanf(file, "%d", &card_cell) > 0) {
		if (col == N) {
			col = 0;
			row = 0;
			card++;
		}
		cards[card][col][row] = card_cell;
		row++;
		if (row == N) {
			col++;
			row = 0;
		}
		count++;
	}
	fclose(file);

	int score = 0;
	for (size_t i = 0; i < call_count; i++) {
		call_number(calls[i], card_count, cards);
		int winning_card = rule(card_count, cards);
		if (winning_card != 101) {
			score = get_score(winning_card, cards) * calls[i];
			break;
		}
	}

	printf("Part %u: score: %u \n", part, score);

	free(calls);
	free(cards);
	return 1;
}

int part1() {
	return play_bingo(1, check_bingo);
}

int part2() {
	return play_bingo(2, last_bingo);
}

int main(void) {
	if (part1() == 0) return 1;
	if (part2() == 0) return 1;

	return 0;
}
