WinCopies-framework (WinCopies.IO package)
==========================================

The WinCopies® software framework

README
======

Description
-----------

Some utilities to extend the .NET framework I/O operations capabilities, such as:

- Some static methods to interact with file paths,
- Some static methods to interact with the Windows Registry,
- Windows API Code Pack's ShellObject wrapper with advanced features,
- SevenZipSharp's ArchiveFileInfo wrapper with advanced features,
- Items loaders for file system items

Usage notes
-----------

**How to implement IO's interfaces?**

**IBrowsableObjectInfoFactory**

- When you implement an interface that inherits from the IBrowsableObjectInfoFactory interface, the your class or structure should contain methods that can be used to get new IBrowsableObjectInfo objects.
- These methods should all have the following name: GetBrowsableObjectInfo.
- If your class or structure has default methods for the factory, they should call methods that can be used to specify a custom factory. The default-factory methods should pass a deep clone of the current factory to the custom-factory methods if the UseRecursively property is equal to True.
- (Un)RegisterPath implementations:
	- These methods should be implemented explicitly.
	- The RegisterPath method should check if references of the value of the Path property and of the given path are equal, and should throw an exception if they are (an InvalidOperationException is recommended). This allow us to prevent that a path could be registered more than one time.
	- Then, the RegisterPath method should check if references of the given path's factory and of the current factory are equal, and register the given path if they are, and should throw an exception otherwise (an InvalidOperationException is recommended). This behavior simulates an internal assignment between two classes.
	- The UnregisterPath method should do the same as described in the previous line, but should remove the reference to the Path instead of adding one.

Project link
------------

[https://github.com/pierresprim/WinCopies-framework](https://github.com/pierresprim/WinCopies-framework)

License
-------

See [LICENSE](https://github.com/pierresprim/WinCopies-framework/blob/master/LICENSE) for the license of the WinCopies framework.

This framework uses some external dependencies. Each external dependency is integrated to the WinCopies framework under its own license, regardless of the WinCopies framework license.
