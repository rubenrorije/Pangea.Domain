# Pangea.Domain

A project containing multiple different value objects with behaviour to have a rich domain model and have logic about these value objects in one place.
Examples are CreditCard, Percentage, Iban, etc. 
When using this library the domain entities should have these rich value objects as properties. 
E.g.

```csharp
public class Customer
{
	public Percentage Discount { get; set; }
	public DateRange DiscountPeriod { get; set; }
	public GpsLocation Location { get; set; }
	public Currency Currency { get; set; }
	public Iban BankAccount { get; set; }
}
````

The example above makes it much easier to work with the domain entities and encapsulates much more information than standard .Net types. 

## Commonalities between value types
All value types are `[Serializable]` and implement `IXmlSerializable`.
### TryParse
All the types for which it makes sense have a `TryParse` method that works similar to the default .Net implementations
### Unsafe
Some types have quite some validation to make sure the given constructor argument is a valid value for the value type. For performance reasons it might be useful to bypass these validations when it is already known that the validations will pass. For instance a credit card from the database might already be validated to be correct, so redoing this validation every time the data is loaded from the database results in unnecessary overhead. Note that it might be better to only use this when the performance is indeed negatively impacted.
### Deconstruct
When applicable it is easy for a type to be deconstructed into its parts in C# 7.0. E.g.
```csharp
var (lat, lon) = new GpsLocation(5.454,7.244).Deconstruct();
var (start, end) = DateRange.Today().Deconstruct();
```

----
### Note
The repository is not following the Single Responsibility Principle for the whole repository, since the value objects are a grab bag of all different unrelated concepts. I am aware of that, but still I hope there is someone who is helped by this. I can imagine that in certain cases a developer is tempted to only use a single concept / value object that is applicable for his/her project. Feel free to copy the source code for one file, although that will therefore (obviously) will not be updated with new versions.
