using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

class Program {
    // [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate int AssemblyFunction(int a);

    [DllImport("kernel32.dll")]
    static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

    static void Main(string[] args) {
        byte[] assembled_code = {
            0x55,               // push ebp
            0x89, 0xE5,         // mov ebp, esp
            0x8B, 0x45, 0x08,   // mov eax, [ebp + 8] ; eax = param
            0x40,               // inc eax
            0x5D,               // pop ebp
            0xC3                // ret ; eax is return value
        };

        AssemblyFunction mystery_function;
        unsafe {
            fixed (byte *ptr = assembled_code) {
                IntPtr address = (IntPtr)ptr;

                if (!VirtualProtectEx(Process.GetCurrentProcess().Handle, address, (UIntPtr)assembled_code.Length, 0x40 /* EXECUTE_READWRITE */, out uint _)) {
                    throw new Win32Exception();
                }

                mystery_function = Marshal.GetDelegateForFunctionPointer<AssemblyFunction>(address);
            }
        }

        Console.WriteLine(mystery_function(27));

        Console.ReadKey();
    }
}