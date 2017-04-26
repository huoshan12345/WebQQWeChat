namespace Domain.Entities.Base
{
    public class AggregateRoot : AggregateRoot<int>, IAggregateRoot
    {
    }

    public class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>
    {
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
