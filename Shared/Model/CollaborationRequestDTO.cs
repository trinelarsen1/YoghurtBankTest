
    public record CollaborationRequestCreateDTO
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int SupervisorId { get; set; }
        public int? IdeaId { get; set; }

        [Required]
        [StringLength(500)]
        public string Application { get; set; }
    }

    public record CollaborationRequestUpdateDTO
    {
        public int Id { get; set; }
        public CollaborationRequestStatus Status { get; set; }
    }

    public record CollaborationRequestDetailsDTO
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int SupervisorId { get; set; }

        [StringLength(500)]
        [Required]
        public string Application { get; set; }

        public CollaborationRequestStatus Status { get; set; }
    }

