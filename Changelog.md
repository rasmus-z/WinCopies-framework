WinCopies-framework
===================

The WinCopies® software framework

CHANGELOG
=========

Updates
-------

1.4
---

WinCopies.Data (1.0)
------------------

First release

WinCopies.IO (1.0)
------------------

First release

WinCopies.Util (2.0)
--------------------

- Added:
	- Classes:
		- EnumComparer
		- ArrayAndListBuilder class to build arrays, lists and observable collections like the .Net's StringBuilder does.
	- Methods:
		- EqualityComparison delegate
		- ToStringWithoutAccents string extension method
		- Extension methods for LinkedLists
		- Extension methods for setting properties in BackgroundWorkers with an is-busy check.
		- 'SetField' static method.
		- The view model classes now have the OnAutoPropertyChanged method to automatically set an auto-property and raise the PropertyChanged event.

- Bug fixes:
	- Bug fixed in the BackgroundWorker class: when aborting, the RunWorkerCompleted event was raised twice.
	- Bug fixed in the BackgroundWorker class: when finalizing, an invalid operation exception was thrown if the BackgroundWorker was busy; now, the BackgroundWorker aborts the working instead of throwing an exception.
	- Bug fixed in the Is extension method when setting the typeEquality parameter to false.

- Major updates:
	- The BackgroundWorker class now resets its properties in background.
	- The 'If' methods perform a real 'xor' comparison in binary mode and are now faster.
	- The ApartmentState, WorkerReportsProgress and WorkerSupportsCancellation properties of the IBackgroundWorker interface are now settable.
	- The IsNullConverter class now uses the 'is' operator instead of '=='.

- Removals:
	- The redundant 'performIntegrityCheck' parameter in one of the 'SetProperty' methods has been removed.

- Updated doc.

Project link
------------

[https://github.com/pierresprim/WinCopies-framework](https://github.com/pierresprim/WinCopies-framework)

License
-------

See [LICENSE](https://github.com/pierresprim/WinCopies-framework/blob/master/LICENSE) for the license of the WinCopies framework.

This framework uses some external dependencies. Each external dependency is integrated to the WinCopies framework under its own license, regardless of the WinCopies framework license.
