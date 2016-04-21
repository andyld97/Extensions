# Extensions
Some useful code that you need from time to time

ConvertLength:
---------

1. Calculate the length directly into the unit prefix which fits:

```cs
ConvertLength.Item nSize = VFS.Language.ConvertLength.Calculate(1024);
// nSize.Length = Your new length
// nSize.Type = Your new unit prefix.
```

2. Calculate e.g. 1024 KB to MB

```cs
ConvertLength.Item nSize = ConvertLength.Calculate(new ConvertLength.Item(1024.0, ConvertLength.Type_.KB), ConvertLength.Type_.MB);
```
