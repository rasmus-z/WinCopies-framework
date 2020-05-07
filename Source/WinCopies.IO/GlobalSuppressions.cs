// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "These constructors are implemented with more parameters.", Scope = "type", Target = "~T:WinCopies.IO.IOException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "These constructors are implemented with more parameters.", Scope = "type", Target = "~T:WinCopies.IO.DirectoryNotFoundException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "These constructors are implemented with more parameters.", Scope = "type", Target = "~T:WinCopies.IO.DriveNotFoundException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "These constructors are implemented with more parameters.", Scope = "type", Target = "~T:WinCopies.IO.EndOfStreamException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "These constructors are implemented with more parameters.", Scope = "type", Target = "~T:WinCopies.IO.FileLoadException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "These constructors are implemented with more parameters.", Scope = "type", Target = "~T:WinCopies.IO.FileNotFoundException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "These constructors are implemented with more parameters.", Scope = "type", Target = "~T:WinCopies.IO.PathTooLongException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "These constructors are implemented with more parameters.", Scope = "type", Target = "~T:WinCopies.IO.PipeException")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Validation made by the ThrowIfNull method.", Scope = "member", Target = "~M:WinCopies.IO.ArchiveItemInfo.From(WinCopies.IO.IShellObjectInfo,SevenZip.ArchiveFileInfo)~WinCopies.IO.ArchiveItemInfo")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "The Dispose method is implemented only for the IEnumerator interface implementation and does not dispose unmanaged code.", Scope = "member", Target = "~M:WinCopies.IO.ShellObjectInfo.GetArchiveItemInfoItems~System.Collections.Generic.IEnumerable{WinCopies.IO.IBrowsableObjectInfo}")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "The Dispose method is implemented only for the IEnumerator interface implementation and does not dispose unmanaged code.", Scope = "member", Target = "~M:WinCopies.IO.ArchiveItemInfo.GetItems~System.Collections.Generic.IEnumerable{WinCopies.IO.IBrowsableObjectInfo}")]
