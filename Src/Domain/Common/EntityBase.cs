namespace Domain.Common;

public abstract class EntityBase
{
    public Guid Id { get; set; }
	public EntityBase()
	{
		Id = Guid.NewGuid();
	}
}