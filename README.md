# OSEP-Breaking-Chains

## Journey

At the time of writing this I am about halfway through the PEN-300 course. Although I say this about every offsec course I take, this course has been very motivational. Granted I've taken longer on the course materials than I wanted to, this is mostly because I tend to beyond what is expected from an exercise or extra mile because I like to tinker. It helps in the long run, but hurts the lab timer, oh well. 

## Code Snippets

As I progress through the course, I am suiting up some code snippets that I will share here. The intent here is to give you a taste of some of the techniques you'll get exposed to with the hopes of motivating you to join your peers by enrolling in the course. 

### Shellcode Crypters

| File Name | Cipher | Language
| :-- | :--| :--|
| 3DES-Shellcode.cs | 3DES 128/192-bit | C# |
| 3DES-Shellcode.ps1 | 3DES 128/192-bit | PowerShell |
| AES-Shellcode.cs | AES 128/192/256-bit | C# |
| AES-Shellcode.ps1 | AES 128/192/256-bit | PowerShell |
| Caesar-Shellcode.cs | Caesar/ROT | C# |
| Caesar-Shellcode.ps1 | Caesar/ROT | PowerShell |
| Caesar-Xor-Shellcode.cs | Caesar/ROT then XOR | C# |
| Caesar-Xor-Shellcode.ps1 | Caesar/ROT then XOR | PowerShell |
| Caesar-Xor-VBA-Shellcode.cs | Caesar/ROT then XOR for VBA | C# |
| Caesar-Xor-VBA-Shellcode.ps1 | Caesar/ROT then XOR for VBA | PowerShell |
| DES-Shellcode.cs | DES 64-bit | C# |
| DES-Shellcode.ps1 | DES 64-bit | PowerShell |
| RC2-Shellcode.cs | RC2 40/128-bit | C# |
| RC2-Shellcode.ps1 | RC2 40/128-bit | PowerShell |
| XOR-Shellcode.cs | XOR | C# |
| XOR-Shellcode.ps1 | XOR | PowerShell |

### Shellcode Runners

| File Name | Type | Language
| :-- | :--| :--|
| AES-Shellcode-Runner.aspx | AES 128/192/256-bit encrypted shellcode runner | ASPX/C# |
| AES-Shellcode-Runner.cs | AES 128/192/256-bit encrypted shellcode runner | C# |
| AES-Shellcode-Runner.ps1 | AES 128/192/256-bit encrypted shellcode runner | PowerShell |
| Caesar-XOR-Process-Injection.cs | Process injection with Caesar+XOR encrypted shellcode | C# |
| Caesar-XOR-Shellcode-Runner.vb | Caesar+XOR encrypted shellcode runner for word macros | VB |
| Caesar-XOR-Staged-Shellcode-Runner.vb | Staged Caesar+XOR encrypted shellcode runner for word macros | VB |
| DLL-Shellcode-Runner.cs | AES 128/192/256-bit encrypted shellcode class library (DLL) runner | C# |
| DLL-Shellcode-Loader.ps1 | Reflective loading technques for a class library (DLL) runner | PowerShell |
