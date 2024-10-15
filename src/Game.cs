namespace Minecraft.UWP;

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Management.Core;
using Windows.Management.Deployment;
using System.Runtime.InteropServices;

public sealed class Game
{
    static readonly ApplicationActivationManager applicationActivationManager = new();

    static readonly PackageManager packageManager = new();

    static readonly PackageDebugSettings packageDebugSettings = new();

    const string packageFamilyName = "Microsoft.MinecraftUWP_8wekyb3d8bbwe";

    const string appUserModelId = "Microsoft.MinecraftUWP_8wekyb3d8bbwe!App";

    public readonly int ProcessId;

    internal Game(int processId) => ProcessId = processId;

    public static int Launch()
    {
        var package = packageManager.FindPackagesForUser(string.Empty, packageFamilyName).FirstOrDefault() ?? throw new Win32Exception(Native.ERROR_INSTALL_PACKAGE_NOT_FOUND);
        Marshal.ThrowExceptionForHR(packageDebugSettings.GetPackageExecutionState(package.Id.FullName, out var packageExecutionState));
        Marshal.ThrowExceptionForHR(packageDebugSettings.EnableDebugging(package.Id.FullName, default, default));
        
        using ManualResetEventSlim @event = new(packageExecutionState is not PackageExecutionState.Unknown or PackageExecutionState.Terminated);
        using FileSystemWatcher watcher = new(ApplicationDataManager.CreateForPackageFamily(packageFamilyName).LocalFolder.Path)
        {
            NotifyFilter = NotifyFilters.FileName,
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };
        watcher.Deleted += (_, e) => { if (e.Name.Equals(@"games\com.mojang\minecraftpe\resource_init_lock", StringComparison.OrdinalIgnoreCase)) @event.Set(); };

        Marshal.ThrowExceptionForHR(applicationActivationManager.ActivateApplication(appUserModelId, default, Native.AO_NOERRORUI, out var processId)); @event.Wait(); return processId;
    }

    public static async Task<int> LaunchAsync() => await Task.Run(Launch).ConfigureAwait(false);

    public void Load(string path) => Library.Load(ProcessId, Path.GetFullPath(path));

    public async Task LoadAsync(string path) => await Task.Run(() => Load(path)).ConfigureAwait(false);
}