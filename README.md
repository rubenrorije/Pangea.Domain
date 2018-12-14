# Pangea.Domain

A project containing multiple different value objects with behaviour to have a rich domain model and have logic about these value objects in one place.
Examples are CreditCard, Percentage, Iban, etc. 
When using this library the domain entities should have these rich value objects as properties. 
E.g.

```csharp
public class Customer
{
	public Percentage Discount { get; set; }
	public DateRange Valid { get; set; }
	public GpsLocation Location { get; set; }
	public Currency Currency { get; set; }
	public Iban BankAccount { get; set; }
}
````

The example above makes it much easier to work with the domain entities and encapsulates much more information than standard .Net types. 

## Note

The repository is not following the Single Responsibility Principle for the whole repository, since the value objects are a grab bag of all different unrelated concepts. I am aware of that, but still I hope there is someone who is helped by this. I can imagine that in certain cases a developer is tempted to only use a single concept / value object that is applicable for his/her project. Feel free to copy the source code for one file, although that will therefore (obviously) will not be updated with new versions.
