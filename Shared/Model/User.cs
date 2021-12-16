
public abstract class User
    {
    public int Id { get; init; }

    [Required]
    [StringLength(50)]
    public string UserName { get; init; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; init; }
        public ICollection<CollaborationRequest> CollaborationRequests { get; set; }

        public void LogIn()
        {
            //Link to the login form - we are not making it 
            throw new NotImplementedException();
        }

        public bool RespondToCollaborationRequest(CollaborationRequest collabrequest, bool Approve)
        {
            //this method simply calls the wanted collabrequest. 
            //the request checks if it is possible to do it at this time
            return collabrequest.UpdateStatus(Approve, this);
        }

}
