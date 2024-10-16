namespace Minecraft.UWP;

using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Runtime.InteropServices;

static class Library
{
    static readonly SecurityIdentifier identifier = new("S-1-15-2-1");

    static readonly nint lpStartAddress;

    static Library()
    {
        var hModule = Native.LoadLibraryEx("Kernel32.dll", default, Native.LOAD_LIBRARY_SEARCH_SYSTEM32);
        lpStartAddress = Native.GetProcAddress(hModule, "LoadLibraryW");
        Native.FreeLibrary(hModule);
    }

    internal static void Load(int processId, string path)
    {
        if (File.Exists(path))
        {
            var security = File.GetAccessControl(path);
            security.AddAccessRule(new(identifier, FileSystemRights.ReadAndExecute, AccessControlType.Allow));
            File.SetAccessControl(path, security);
        }
        else throw new FileNotFoundException();

        nint hProcess = default, lpBaseAddress = default, hThread = default;
        try
        {
            hProcess = Native.OpenProcess(Native.PROCESS_ALL_ACCESS, false, processId);
            if (hProcess == default) throw new Win32Exception(Marshal.GetLastWin32Error());

            var nSize = sizeof(char) * (path.Length + 1);

            lpBaseAddress = Native.VirtualAllocEx(hProcess, default, nSize, Native.MEM_COMMIT | Native.MEM_RESERVE, Native.PAGE_EXECUTE_READWRITE);
            if (lpBaseAddress == default) throw new Win32Exception(Marshal.GetLastWin32Error());

            if (!Native.WriteProcessMemory(hProcess, lpBaseAddress, Marshal.StringToHGlobalUni(path), nSize, out _)) throw new Win32Exception(Marshal.GetLastWin32Error());

            hThread = Native.CreateRemoteThread(hProcess, default, 0, lpStartAddress, lpBaseAddress, 0, out _);
            if (hThread == default) throw new Win32Exception(Marshal.GetLastWin32Error());
            Native.WaitForSingleObject(hThread, Timeout.Infinite);
        }
        finally
        {
            Native.VirtualFreeEx(hProcess, lpBaseAddress, 0, Native.MEM_RELEASE);
            Native.CloseHandle(hThread);
            Native.CloseHandle(hProcess);
        }
    }
}