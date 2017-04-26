namespace Domain.Entities.Base
{
    public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey>, IPassivable, ISoftDelete
    {

    }

    public interface IAggregateRoot : IAggregateRoot<int>, IEntity
    {

    }
}
