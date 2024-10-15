using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

[ComImport, Guid("2E941141-7F97-4756-BA1D-9DECDE894A3D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
interface IApplicationActivationManager { int ActivateApplication(string appUserModelId, string arguments, int options, out int processId); }

[ComImport, Guid("45BA127D-10A8-46EA-8AB7-56EA9078943C")]
sealed class ApplicationActivationManager : IApplicationActivationManager
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern int ActivateApplication(string appUserModelId, string arguments, int options, out int processId);
}