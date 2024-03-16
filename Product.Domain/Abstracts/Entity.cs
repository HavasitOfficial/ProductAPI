namespace Product.Domain.Abstracts
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        protected Entity()
        {
            Id = Id == Guid.Empty ? Guid.NewGuid() : Id;
        }

    }
}
