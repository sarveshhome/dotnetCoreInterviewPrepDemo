// public class GuidService{

//     public Guid Id { get; set; }
//     public GuidService(){
//         Id = Guid.NewGuid();
//     }

// }


// Using primary constructor
public class GuidService(Guid id = default)
{
    public Guid Id { get; set; } = id == default ? Guid.NewGuid() : id;
}

public class ParentService(GuidService guid)
{
    public string Id => $"parent_{guid.Id}";
}