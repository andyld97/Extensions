# C# Extensions
Some useful code that you need from time to time

ByteUnit
---
Now available as a nuget package:   
`dotnet add package ByteUnit --version 1.0.2`

1. Calculate the length directly into the unit prefix which is appropriate:

```cs
var value = ByteUnit.FindUnit(1024);
// value.Length = Your new length
// value.Type = Your new unit prefix.
```

2. Calculate e.g. 1024 KB to MB

```cs
var value = ByteUnit.From(new ByteUnit(1024.0, Unit.KB), Unit.MB);
// or alternative
var value = ByteUnit.From(ByteUnit.FromKB(1024), Unit.MB);
```

3. Compare two `ByteUnit`s
```cs
var fileSize = ByteUnit.FromFileInfo(new System.IO.FileInfo("PATH"));

if (fileSize < ByteUnit.FromMB(10))
{
    // e.g. only download this file if it's less than 10MB big.
}
```

4. Easy conversation
```cs
var value = ByteUnit.Parse("999.5 MB/s");
var gb = value.To(Unit.GB);
```

5. Parse from string 
```cs
var value = ByteUnit.Parse("100 MB");
// var value = ByteUnit.Parse("999.5 MB/s");
// you can also use TryParse 
```

ByteUnit overrides the default `ToString()`-Method, so you can use it to format your values in strings. There is also an overload for "bytes per seconds", this will add `/s` to end of the formatted string!

FileAssociation
---

Useful for associate a file extension with your program (with icon)

```cs
// Add
FileAssociation.SetFileAssociation("_NAME_", "_EXTENSION_", "_PATH_TO_ICO", "_PATH_TO_EXE"); 

// Delete
FileAssociation.DeleteFileAssociation("_NAME_", "_EXTENSION_");
```

ZipHelper
---
```cs
// Create a zip archive
await ZipHelper.CreateZipFileFromDirectoryAsync(@"F:\Data", @"C:\Users\test\Desktop\test.zip");

// Extract a zip archive
await ZipHelper.ExtractZipFileAsync(@"C:\Users\test\Desktop\test.zip", @"F:\Data");
```
- You can also pass a `byte[]` to `ExtractZipFileAsync(...)`!
- All methods are available synchronously (e.g. `ExtractZipFiles(...)`) and asynchronously (using `async` and `await`)!
- If you are not using .NET Core there is no `System.IO.Path.GetRelative` Method. But you can use this [port](https://gist.github.com/antonKrouglov/07ccc117cb8c30ad8994446a86c062e5) instead!

Hash
---
```cs
// Create hash from file
string hash = await Hash.CreateHashFromFileAsync(@"F:\Data\test.iso");
```
