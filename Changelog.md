WinCopies-framework
===================

The WinCopies® software framework

CHANGELOG
=========

Updates
-------

2.0
===

WinCopies.Data (1.0)
--------------------

First release

WinCopies.IO (1.0)
------------------

First release

WinCopies.Util (2.0)
--------------------

- Major updates:
	- The BackgroundWorker class now resets its properties in background.
	- The 'If' methods perform a real 'xor' comparison in binary mode and are faster.
	- The 'If' methods now set up the out 'key' parameter with the second value and predicate pair that was checked, if any, instead of the default value for the key type when performing a 'xor' comparison.
	- The ApartmentState, WorkerReportsProgress and WorkerSupportsCancellation properties of the IBackgroundWorker interface are now settable.
	- The IsNullConverter class now uses the 'is' operator instead of '=='.
	- The ObservableCollection now calls base methods for avoinding reentrancy.

- Added:
	- Classes:
		- EnumComparer
		- ArrayAndListBuilder class to build arrays, lists and observable collections like the .Net's StringBuilder does.
		- Comparer classes and interfaces for sorting support.
	- Methods:
		- Static methods:
			- 'SetField' static method.
			- 'Between' static methods for the other numeric types.
			- 'ThrowIfNull' static method.
			- 'GetOrThrowIfNotType' static method.
			- 'GetIf' methods.
		- Extension methods:
			- ToStringWithoutAccents string extension method
			- Extension methods for LinkedLists
			- Extension methods for setting properties in BackgroundWorkers with an is-busy check.
		- Delegates:
			- EqualityComparison
			- FieldValidateValueCallback*
			- FieldValueChangedCallback*
			- PropertyValidateCallback*
			- PropertyValueChangedCallback*
			- ActionParams
			- Func
			- FuncParams
		- Misc:
			- The view model classes now have an OnAutoPropertyChanged method to automatically set an auto-property and raise the PropertyChanged event.
			- The ReadOnlyObservableCollection has now an OnCollectionChanging protected virtual method.
	- Parameters:
		- The WinCopies.Util.Extensions.SetProperty/Field now have multiple new optional parameters to extend the capabilities of these methods.

- Bug fixes:
	- Bug fixed in the BackgroundWorker class: when aborting, the RunWorkerCompleted event was raised twice.
	- Bug fixed in the BackgroundWorker class: when finalizing, an invalid operation exception was thrown if the BackgroundWorker was busy; now, the BackgroundWorker aborts the working instead of throwing an exception.
	- Bug fixed in the Is extension method when setting the typeEquality parameter to false.

- Removals:
	- The 'performIntegrityCheck' parameter in 'SetProperty' methods has been replaced by the 'throwIfReadOnly' parameter.

- Misc:
	- ReadOnlyObservableCollection's CollectionChanging event has now the protected access modifier.
	- Updated doc.

\* See WinCopies.Util.Extensions.SetProperty/Field

Project link
------------

[https://github.com/pierresprim/WinCopies-framework](https://github.com/pierresprim/WinCopies-framework)

License
-------

See [LICENSE](https://github.com/pierresprim/WinCopies-framework/blob/master/LICENSE) for the license of the WinCopies framework.

This framework uses some external dependencies. Each external dependency is integrated to the WinCopies framework under its own license, regardless of the WinCopies framework license.
