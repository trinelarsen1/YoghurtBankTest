

    public interface IYoghurtContext
    {
        public DbSet<User> Users {get;}
        public DbSet<Idea> Ideas { get; }
        public DbSet<CollaborationRequest> CollaborationRequests { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();

    }
