namespace JobManagement.Domain.SeedWork
{
    public static class EntityListExtension
    {
        public static T FindById<T, TId>(this IEnumerable<T> entities, TId id)
            where T : Entity<TId>
            where TId : IEquatable<TId>
        {
            var entity = entities.FirstOrDefault(e => e.Id.Equals(id));
            if (entity is null)
                throw new Exception($"Inconsistent state: no {typeof(Entity<TId>).Name} found with ID {id}.");
            return entity;
        }

    }
}
