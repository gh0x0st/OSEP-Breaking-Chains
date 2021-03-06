# OSEP-Breaking-Chains

## Journey

At the time of writing this I am about halfway through the PEN-300 course. Although I say this about every offsec course I take, this course has been very motivational. Granted I've taken longer on the course materials than I wanted to, this is mostly because I tend to beyond what is expected from an exercise or extra mile because I like to tinker. It helps in the long run, but hurts the lab timer, oh well. 

## Code Snippets

As I progress through the course, I am suiting up some code snippets that I will share here. The intent here is to give you a taste of some of the techniques you'll get exposed to with the hopes of motivating you to join your peers by enrolling in the course.

### Shellcode Crypters

| Snippet | Description | Language
| :-- | :--| :--|
| 3DES-Shellcode.cs | Encrypting shellcode with a 3DES 128/192-bit cipher | C# |
| 3DES-Shellcode.ps1 | Encrypting shellcode with a 3DES 128/192-bit cipher | PowerShell |
| AES-Shellcode.cs | Encrypting shellcode with an AES 128/192/256-bit cipher | C# |
| AES-Shellcode.ps1 | Encrypting shellcode with an AES 128/192/256-bit cipher | PowerShell |
| Caesar-Shellcode.cs | Encrypting shellcode with a Caesar/ROT cipher | C# |
| Caesar-Shellcode.ps1 | Encrypting shellcode with a Caesar/ROT cipher | PowerShell |
| Caesar-Xor-Shellcode.cs | Encrypting shellcode with a Caesar/ROT cipher then XOR | C# |
| Caesar-Xor-Shellcode.ps1 | Encrypting shellcode with a Caesar/ROT cipher then XOR | PowerShell |
| Caesar-Xor-VBA-Shellcode.cs | Encrypting shellcode with a Caesar/ROT cipher then XOR for VBA | C# |
| Caesar-Xor-VBA-Shellcode.ps1 | Encrypting shellcode with a Caesar/ROT cipher then XOR for VBA | PowerShell |
| DES-Shellcode.cs | Encrypting shellcode with a DES 64-bit cipher | C# |
| DES-Shellcode.ps1 | Encrypting shellcode with a DES 64-bit cipher | PowerShell |
| RC2-Shellcode.cs | Encrypting shellcode with a RC2 40/128-bit cipher | C# |
| RC2-Shellcode.ps1 | Encrypting shellcode with a RC2 40/128-bit cipher | PowerShell |
| XOR-Shellcode.cs | Encrypting shellcode with a XOR cipher | C# |
| XOR-Shellcode.ps1 | Encrypting shellcode with a XOR cipher | PowerShell |

### Shellcode Runners

| Snippet | Description | Language
| :-- | :--| :--|
| AES-Shellcode-Runner.cs | AES 128/192/256-bit encrypted shellcode runner | C# |
| AES-Shellcode-Runner.ps1 | AES 128/192/256-bit encrypted shellcode runner | PowerShell |
| DLL-Shellcode-Runner.cs | AES 128/192/256-bit encrypted shellcode class library (DLL) runner | C# |
| DLL-Shellcode-Loader.ps1 | Reflective loading technques for the class library (DLL) runner | PowerShell |
