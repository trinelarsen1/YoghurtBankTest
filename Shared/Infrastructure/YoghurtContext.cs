
    public class YoghurtContext : DbContext, IYoghurtContext
    {
        public YoghurtContext(DbContextOptions<YoghurtContext> options) : base(options) { }
        public DbSet<User> Users {get; set;}
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<CollaborationRequest> CollaborationRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CollaborationRequest>().Property(c => c.Status)
                .HasConversion(new EnumToStringConverter<CollaborationRequestStatus>());
        }
    }
