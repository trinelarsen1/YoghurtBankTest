
    public record IdeaCreateDTO
    {
        public int CreatorId { get; init; }
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(100)]
        public string Subject { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public int AmountOfCollaborators { get; set; }
        public bool Open { get; set; }
        public TimeSpan TimeToComplete { get; set; }
        public DateTime StartDate { get; set; }
        public IdeaType Type { get; set; }
    }

    public record IdeaUpdateDTO
    {
        public int Id { get; init; }
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(100)]
        public string Subject { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public int AmountOfCollaborators { get; set; }
        public bool Open { get; set; }
        public TimeSpan TimeToComplete { get; set; }
        public DateTime StartDate { get; set; }
        public IdeaType Type { get; set; }
    }


    public record IdeaDTO
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(100)]
        public string Subject { get; set; }
        public IdeaType Type { get; set; }
    }

    public record IdeaDetailsDTO
    {
        public int CreatorId { get; init; }
        public int Id { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(100)]
        public string Subject { get; set; }
        public DateTime Posted { get; init; }
        [StringLength(500)]
        public string Description { get; set; }
        public int AmountOfCollaborators { get; set; }
        public bool Open { get; set; }
        public TimeSpan TimeToComplete { get; set; }
        public DateTime StartDate { get; set; }
        public IdeaType Type {get; set;}
    }


