# dotnetCoreInterviewPrepDemo
Dotnet Core interview Prepration 


## Middleware



## Swagger 

`dotnet add package Swashbuckle.AspNetCore`


`dotnet new list`

### command

`dotnet watch --no-hot-reload`

 - With --no-hot-reload: App fully restarts when changes are detected


### 

```bash
// Current implementation
public class GuidService
{
    public Guid Id { get; set; }
    public GuidService()
    {
        Id = Guid.NewGuid();
    }
}

// Changes needed for primary constructor:
// 1. Remove traditional constructor
// 2. Add primary constructor parameter in class declaration
// 3. Initialize property with expression

// New implementation with primary constructor
public class GuidService(/* primary constructor here */)
{
    // Property initialization using expression body
    public Guid Id { get; set; } = Guid.NewGuid();
}

// Alternative implementation with parameter
public class GuidService(Guid id = default)
{
    public Guid Id { get; set; } = id == default ? Guid.NewGuid() : id;
}

/* Key Changes:
   - Removed: public GuidService() { ... }
   - Added: Constructor parameters after class name
   - Changed: Property initialization style
   - Requirements: .NET 8 and C# 12
*/


