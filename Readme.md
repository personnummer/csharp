# Personnummer

[![Build status](https://ci.appveyor.com/api/projects/status/4gg2pecaek9garpt/branch/master?svg=true)](https://ci.appveyor.com/project/Johannestegner/csharp/branch/master)

Validate Swedish social security numbers.

## Example

```csharp
use Personnummer;

class Test 
{
  public void TestValidation() 
  {
    Personnummer.Valid(6403273813); 	// => True
    Personnummer.Valid('19130401+2931') // => True
  }
}
```

See `Personnummer.Test/PersonnummerTest.cs` for more examples.

## License

MIT
