# Pangea.Domain

A project containing multiple different value objects with extended behaviour, for example CreditCard, Percentage, Iban, etc. 
The repository is created because I like to have more explicit logic for the objects I am using and do not want to spread the logic around.

For example in most projects I worked on, I have created a DateRange object that is able to validate whether a given date was within a range. 

The repository is not following the Single Responsibility Principle for the whole repository, since the value objects are a grab bag of all different unrelated concepts. I am aware of that, but still I hope there is someone who is helped by this. I can imagine that in certain cases a developer is tempted to only use a single concept / value object that is applicable for his/her project. Feel free to copy the source code for one file, although that will therefore (obviously) will not be updated with new versions.
