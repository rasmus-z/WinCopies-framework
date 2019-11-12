WinCopies-framework
===================

The WinCopies® software framework

CHANGELOG
=========

Updates
-------

2.0
===

WinCopies.Data (2.0)
--------------------

First release

WinCopies.Util (2.0)
--------------------

- Existing items behavior updates:
	- WinCopies.Util.BackgroundWorker class:
		- The BackgroundWorker class now resets its properties in background.
		- If a ThreadAbortException is thrown, and is not caught, in the background thread, the BackgroundWorker will consider that a cancellation has occurred.
		- Info can now be passed to the Cancel and CancelAsync methods.
	- The 'If' methods perform a real 'xor' comparison in binary mode and are faster.
	- The 'If' methods now set up the out 'key' parameter with the second value and predicate pair that was checked, if any, instead of the default value for the key type when performing a 'xor' comparison.
	- The ApartmentState, WorkerReportsProgress and WorkerSupportsCancellation properties of the IBackgroundWorker interface are now settable.
	- The IsNullConverter class now uses the 'is' operator instead of '=='.
	- WinCopies.Util.Collections.ObservableCollection class:
		- Now calls base methods for avoinding reentrancy.
		- Now have the Serializable attribute
	- WinCopies.Util.Data.IValueObject generic and non-generic:
		- are now obsoletes and have been replaced by the WinCopies.Util.IValueObject interfaces**;
		- now inherits from the WinCopies.Util.IValueObject interfaces.**
	- The WinCopies.Util.Data.ValueObject now implements the WinCopies.Util.IValueObject generic interface.**
	- The WinCopies.Util.IValueObject interface implements IDisposable, so all classes that implements the WinCopies.Util.IValueObject are also disposable.

- Added:
	- Classes:
		- EnumComparer
		- ArrayAndListBuilder class to build arrays, lists and observable collections like the .Net's StringBuilder does.
		- Comparer classes and interfaces for sorting support.
		- ValueObjectEqualityComparer
		- TreeNode
		- ReadOnlyTreeNode
	- Interfaces:
		- IDeepCloneable
		- IDisposable
		- IObservableCollection
		- IReadOnlyObservableCollection
		- WinCopies.Util.IValueObject
		- ITreeNode
		- IReadOnlyTreeNode
	- Delegates:
		- EqualityComparison
		- FieldValidateValueCallback*
		- FieldValueChangedCallback*
		- PropertyValidateCallback*
		- PropertyValueChangedCallback*
		- ActionParams
		- Func
		- FuncParams
	- Methods:
		- Static methods:
			- 'SetField' static method
			- 'Between' static methods for the other numeric types
			- 'ThrowIfNull' static method
			- 'GetOrThrowIfNotType' static method
			- 'GetIf' methods
			- ThrowOnInvalidCopyToArrayOperation method
		- Extension methods:
			- ToStringWithoutAccents string extension method
			- Extension methods for LinkedLists
			- Extension methods for setting properties in BackgroundWorkers with an is-busy check.
			- Extension method for throwing if an object that implements IDisposable is disposing or disposed.
			- FirstOrDefault ienumerable extension method. This is a generic method that looks for the first item of the given generic type parameter. If any item is found, the method returns the default value for the given generic type parameter.
		- Misc:
			- The view model classes now have an OnAutoPropertyChanged method to automatically set an auto-property and raise the PropertyChanged event.
			- The ReadOnlyObservableCollection has now an OnCollectionChanging protected virtual method.
	- Parameters:
		- The WinCopies.Util.Extensions.SetProperty/Field now have multiple new optional parameters to extend the capabilities of these methods.
	- Misc:
		- ValueObjectEnumerator structure

- Bug fixes:
	- BackgroundWorker class: when aborting, the RunWorkerCompleted event was raised twice.
	- BackgroundWorker class: when finalizing, an invalid operation exception was thrown if the BackgroundWorker was busy; now, the BackgroundWorker aborts the working instead of throwing an exception.
	- Is extension method: error when setting the typeEquality parameter to false.
	- String extension methods: unexpected results.

- Removals:
	- The 'performIntegrityCheck' parameter in 'SetProperty' methods has been replaced by the 'throwIfReadOnly' parameter.

- Misc:
	- ReadOnlyObservableCollection's CollectionChanging event has now the protected access modifier.
	- Some code now uses the 'in' parameter modifier
	- The dependency package System.Windows.Interactivity.WPF has been replaced by the Microsoft.Xaml.Behaviors.Wpf package.
	- Updated doc.

\* See WinCopies.Util.Extensions.SetProperty/Field
\*\* This also applies to the other already existing classes and interfaces, in the previous versions, that inherit from or implement these classes and interfaces.

Project link
------------

[https://github.com/pierresprim/WinCopies-framework](https://github.com/pierresprim/WinCopies-framework)

License
-------

See [LICENSE](https://github.com/pierresprim/WinCopies-framework/blob/master/LICENSE) for the license of the WinCopies framework.

This framework uses some external dependencies. Each external dependency is integrated to the WinCopies framework under its own license, regardless of the WinCopies framework license.
