#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define FILENAME "sample"

enum type { Large = 0, Small };

struct Node {
	int type;
	char *name;
	size_t n_vertices;
	int n_visited;
	struct Node *vertices[];
};

struct Node *new_node(int tag, char *name, size_t n_vertices) {
	struct Node *node = malloc(sizeof(struct Node) + n_vertices * sizeof(struct Node));
	node->n_vertices = 0;
	node->n_visited = 0;
	node->type = tag;
	node->name = name;
	return node;
}

void delete_tree(struct Node *node) {
	for (size_t i = 0; i < node->n_vertices && node->vertices[i]; ++i)
		delete_tree(node->vertices[i]);
	free(node);
}

bool append(struct Node *node, struct Node *vertex) {
	return vertex ? (node->vertices[node->n_vertices++] = vertex) : false;
};

bool has_node(struct Node *nodes[], size_t length, char *target) {
	for (size_t i = 0; i < length; i++)
		if (strncmp(nodes[i]->name, target, strlen(target)) == 0) return true;
	return false;
}

struct Node *get_node(struct Node *nodes[], size_t length, char *target) {
	for (size_t i = 0; i < length; i++)
		if (strncmp(nodes[i]->name, target, strlen(target)) == 0) return nodes[i];
	return NULL;
}

bool has_vertex(struct Node *node, char *target) {
	for (size_t i = 0; i < node->n_vertices; i++)
		if (strncmp(node->vertices[i]->name, target, strlen(target)) == 0) return true;
	return false;
}

struct Vertex {
	char *a;
	char *b;
};

struct Vertex *new_vec(size_t length) {
	struct Vertex *vec = malloc(length * sizeof(struct Vertex));
	for (size_t i = 0; i < length; i++) {
		vec[i].a = malloc(5);
		vec[i].b = malloc(5);
	}
	return vec;
}

size_t sum_vert(struct Vertex *vertices, size_t length, char *target) {
	size_t count = 0;
	for (size_t i = 0; i < length; i++)
		if (strncmp(vertices[i].a, target, strlen(target)) == 0 || strncmp(vertices[i].b, target, strlen(target)) == 0)
			count++;
	return count;
}

bool is_large_cave(char *name) {
	bool is_large = false;
	for (size_t i = 0; i < strlen(name); i++)
		is_large &= (name[i] >= 'A' && name[i] <= 'Z');
	return is_large;
}

int part1(struct Node *start) {
	struct Node ***paths;

	printf("Part 1: result: %u \n", 0);
	return 1;
}

int part2(struct Node *start) {

	printf("Part 2: result: %u \n", 0);
	return 1;
}

int main(void) {
	FILE *file = fopen(FILENAME, "r");
	if (file == NULL) {
		printf("File \"%s\" does not exist!\n", FILENAME);
		return 0;
	}

	const char split[] = "-";
	size_t length = 10;
	char *line = malloc(length);
	size_t count = 0;
	while (fgets(line, length, file) != NULL)
		count++;
	rewind(file);

	struct Vertex *vertices = new_vec(count);
	size_t vertex_count = 0;

	while (fgets(line, length, file) != NULL) {
		int la = strcspn(line, split);
		strncpy(vertices[vertex_count].a, line, la);
		line += la + 1;
		int lb = strcspn(line, split);
		strncpy(vertices[vertex_count++].b, line, lb);
	}

	struct Node *nodes[count];
	size_t nodes_idx = 0;

	/* clang-format off */
	for (size_t i = 0; i < count; i++) {
		if (!has_node(nodes, nodes_idx, vertices[i].a))
			nodes[nodes_idx++] = new_node(
				is_large_cave(vertices[i].a) ? Large : Small, 
				vertices[i].a,
				sum_vert(vertices, vertex_count, vertices[i].a)
			);

		if (!has_node(nodes, nodes_idx, vertices[i].b))
			nodes[nodes_idx++] = new_node(
				is_large_cave(vertices[i].b) ? Large : Small, 
				vertices[i].b,
				sum_vert(vertices, vertex_count, vertices[i].b)
			);
	}
	/* clang-format on */

	for (size_t i = 0; i < count; i++)
		if (!has_vertex(get_node(nodes, nodes_idx, vertices[i].a), vertices[i].b))
			append(get_node(nodes, nodes_idx, vertices[i].a), get_node(nodes, nodes_idx, vertices[i].b));

	if (part1(get_node(nodes, nodes_idx, "start")) == 0) return 1;
	if (part2(get_node(nodes, nodes_idx, "start")) == 0) return 1;

	fclose(file);

	return 0;
}
