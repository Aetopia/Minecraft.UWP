namespace Minecraft.UWP;

using System.Runtime.InteropServices;

static class Native
{
    internal const int LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800;

    internal const int MEM_RELEASE = 0x00008000;

    internal const int PROCESS_ALL_ACCESS = 0X1FFFFF;

    internal const int MEM_COMMIT = 0x00001000;

    internal const int MEM_RESERVE = 0x00002000;

    internal const int PAGE_EXECUTE_READWRITE = 0x40;

    internal const int ERROR_INSTALL_PACKAGE_NOT_FOUND = unchecked((int)0x80073CF1);

    internal const int AO_NOERRORUI = 0x00000002;

    [DllImport("Kernel32", SetLastError = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern nint OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("Kernel32", SetLastError = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern int WaitForSingleObject(nint hHandle, int dwMilliseconds);

    [DllImport("Kernel32", SetLastError = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern bool CloseHandle(nint hObject);

    [DllImport("Kernel32", SetLastError = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern nint VirtualAllocEx(nint hProcess, nint lpAddress, int dwSize, int flAllocationType, int flProtect);

    [DllImport("Kernel32", SetLastError = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern bool WriteProcessMemory(nint hProcess, nint lpBaseAddress, nint lpBuffer, int nSize, out int lpNumberOfBytesWritten);

    [DllImport("Kernel32", SetLastError = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern nint CreateRemoteThread(nint hProcess, nint lpThreadAttributes, int dwStackSize, nint lpStartAddress, nint lpParameter, int dwCreationFlags, out int lpThreadId);

    [DllImport("Kernel32", CharSet = CharSet.Auto, SetLastError = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern nint LoadLibraryEx(string lpLibFileName, nint hFile, int dwFlags);

    [DllImport("Kernel32", SetLastError = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern bool FreeLibrary(nint hLibModule);

    [DllImport("Kernel32", SetLastError = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern nint GetProcAddress(nint hModule, string lpProcName);

    [DllImport("Kernel32", SetLastError = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern bool VirtualFreeEx(nint hProcess, nint lpAddress, int dwSize, nint dwFreeType);

    [DllImport("Shlwapi", SetLastError = true, CharSet = CharSet.Auto), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern bool PathIsRelative(string pszPath);
}