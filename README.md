# Extensions
Some useful code that you need from time to time

ConvertLength:
---------

1. Calculate the length directly into the unit prefix which fits:

```cs
ConvertLength.Item nSize = ConvertLength.Calculate(1024);
// nSize.Length = Your new length
// nSize.Type = Your new unit prefix.
```

2. Calculate e.g. 1024 KB to MB

```cs
ConvertLength.Item nSize = ConvertLength.Calculate(new ConvertLength.Item(1024.0, ConvertLength.Type_.KB), ConvertLength.Type_.MB);
```

FileAssociation
---------

Useful for associate a file extension with your program (with icon)

```cs
// Add
FileAssociation.SetFileAssociation("_NAME_", "_EXTENSION_", "_PATH_TO_ICO", "_PATH_TO_EXE"); 
// Delete
FileAssociation.DeleteFileAssociation("_NAME_", "_EXTENSION_");
```
