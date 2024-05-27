namespace FreelancerProfile.Domain.SeedWork
{
    public abstract class Entity<TId>
    {
        private TId _id;

        public virtual TId Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Entity<TId>))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            var entity = obj as Entity<TId>;

            return entity.Id.Equals(this.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(Entity<TId> entity1, Entity<TId> entity2)
        {
            if (entity1 is null && entity2 is null)
                return true;
            if (entity1 is null || entity2 is null)
                return false;

            return entity1.Equals(entity2);
        }

        public static bool operator !=(Entity<TId> entity1, Entity<TId> entity2)
        {
            return !(entity1 == entity2);
        }

    }
}
