using System.ComponentModel.DataAnnotations;

namespace SharedKernel;

public abstract class EntityBase<T>: IAuditable where T : struct {

    [Key]
    public T Id { get; set; }
    public bool IsActive { get; set; } = true;

    public Guid CreatedById { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now.ToUniversalTime();
    public DateTime? UpdatedOn { get; set; }
    public DateTime? DeletedOn { get; set; }

    public Guid UpdatedById { get; set; }
    
}

public abstract class EntityBase: IAuditable  {

    [Key]
    public Guid Id { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid CreatedById { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now.ToUniversalTime();
    public DateTime? UpdatedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    public Guid UpdatedById { get; set; }
}