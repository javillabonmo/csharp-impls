namespace PersonalBlog.Models.Common;

public abstract class AuditableEntityBase
{
    
        
        public DateTime CreatedAt { get; set; }

        
        public Guid CreatedBy { get; set; }

        
        public DateTime LastUpdatedAt { get; set; }

       
        public Guid LastUpdatedBy { get; set; }
    

}