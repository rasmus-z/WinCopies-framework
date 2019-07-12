WinCopies-framework
===================

The WinCopies� software framework

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

WinCopies.Util (1.4)
--------------------

- The BackgroundWorker class now resets its properties in background.
- The 'If' methods perform a real 'xor' comparison in binary mode and are now faster.
- The redundant 'performIntegrityCheck' parameter in one of the 'SetProperty' methods has been removed.
- 'SetField' static method added.
- The view model classes now have the OnAutoPropertyChanged method to automatically set an auto-property and raise the PropertyChanged event.
- The ApartmentState, WorkerReportsProgress and WorkerSupportsCancellation properties of the IBackgroundWorker interface are now settable.
- EqualityComparison delegate added.
- ToStringWithoutAccents string extension method added.
- Bug fixed in the BackgroundWorker class: when aborting, the RunWorkerCompleted event was raised twice.
- Bug fiwed in the BackgroundWorker class: when finalizing, an invalid operation exception was thrown if the BackgroundWorker was busy; now, the BackgroundWorker aborts the working instead of throwing an exception.
- The IsNullConverter class now uses the 'is' operator instead of '=='.
- ArrayAndListBuilder class added to build arrays, lists and observable collections like the .Net's StringBuilder does.
- Added extension methods for setting properties in BackgroundWorkers with an is-busy check.
- Extension methods added for LinkedLists.

Project link
------------

[https://github.com/pierresprim/WinCopies-framework](https://github.com/pierresprim/WinCopies-framework)

License
-------

See [LICENSE](https://github.com/pierresprim/WinCopies-framework/blob/master/LICENSE) for the license of the WinCopies framework.

This framework uses some external dependencies. Each external dependency is integrated to the WinCopies framework under its own license, regardless of the WinCopies framework license.
