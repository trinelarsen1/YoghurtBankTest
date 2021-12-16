
    public class Idea
    {
        [Key]
        public int Id { get; set; }
        public Supervisor Creator { get; set; }
        public DateTime Posted { get; set; }

        [Required]
        [StringLength(100)]
        public string Subject { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        public int AmountOfCollaborators { get; set; }

        [Required]
        public bool Open { get; set; }
        public TimeSpan TimeToComplete { get; set; }
        public DateTime StartDate { get; set; }
        public IdeaType Type { get; set; }
    }
