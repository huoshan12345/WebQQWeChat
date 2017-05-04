namespace Domain.Entities.Base
{
    public abstract class AggregateRoot : AggregateRoot<int>, IAggregateRoot
    {
    }

    public abstract class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>
    {
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
