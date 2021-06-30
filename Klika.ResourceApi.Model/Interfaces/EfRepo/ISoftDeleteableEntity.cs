namespace Klika.ResourceApi.Model.Interfaces.EfRepo
{
    public interface ISoftDeleteableEntity
    {
        public bool IsDeleted { get; set; }
    }
}
