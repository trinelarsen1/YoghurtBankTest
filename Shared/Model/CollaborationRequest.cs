
    public class CollaborationRequest
    {
        public int Id { get; set; }

        [Required]
        public Student Requester { get; init; }

        [Required]
        public Supervisor Requestee { get; init; }
        public Idea? Idea { get; init; }

        [Required]
        [StringLength(500)]
        public string Application { get; set; }
        public CollaborationRequestStatus Status { get; set; } = CollaborationRequestStatus.Waiting;

        public bool UpdateStatus(bool Approved, User Updater)
        {
            //check if the user updating can make the update that is being attempted
            //check if the attempted update is allowed

            throw new NotImplementedException();
        }
    }

