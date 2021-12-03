CC = gcc
SRCS = $(wildcard */*.c)
CFLAGS = -g -Wall -Wextra
LIBFLAGS = -lm

TARGETS = $(patsubst %.c,%,$(SRCS))

all: $(TARGETS)

%: %.c
		$(CC) $(CFLAGS) -o $@ $< $(LIBFLAGS)
clean: 
		rm -f $(TARGETS)
