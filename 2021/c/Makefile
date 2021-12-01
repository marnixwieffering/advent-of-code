CC = gcc
SRCS = $(wildcard */*.c)
CFLAGS = -g -Wall -Wextra

TARGETS = $(patsubst %.c,%,$(SRCS))

all: $(TARGETS)

%: %.c
		$(CC) $(CFLAGS) -o $@ $<
clean: 
		rm -f $(TARGETS)
