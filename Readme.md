# Personnummer

[![GitHub Workflow Status](https://img.shields.io/github/workflow/status/personnummer/csharp/Test)](https://github.com/personnummer/csharp/actions)

Validate Swedish social security numbers.

## Installation

```
dotnet add package Personnummer --version 3.0.0
```

## Examples

### Validation

```csharp
using Personnummer;

class Test 
{
  public void TestValidation() 
  {
    Personnummer.Valid("191212121212");     // => True
    Personnummer.Valid("12121+21212")       // => True
    Personnummer.Valid("2012121-21212")     // => True
  }
}
```

### Format

```csharp
using Personnummer;

// Short format (YYMMDD-XXXX)
(new Personnummer("1212121212")).Format();
//=> 121212-1212

// Short format for 100+ years old
(new Personnummer("191212121212")).Format();
//=> 121212+1212

// Long format (YYYYMMDDXXXX)
(new Personnummer("1212121212")).Format(true);
//=> 201212121212
```

### Age

```csharp
using Personnummer;

(new Personnummer("1212121212")).Age;
//=> 7
```

### Get sex

```csharp


(new Personnummer("1212121212")).IsMale;
//=> true
(new Personnummer("1212121212")).IsFemale;
//=> false
```

See `Personnummer.Test/PersonnummerTest.cs` for more examples.

## License

```
MIT License

Copyright (c) 2017-2019 - Personnummer and Contributors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

```
