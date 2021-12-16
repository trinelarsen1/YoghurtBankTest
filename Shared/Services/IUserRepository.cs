
    public interface IUserRepository
    {
        Task<UserDetailsDTO>? FindUserByIdAsync(int userId);

        Task<UserDetailsDTO> CreateAsync(UserCreateDTO user);

        Task<int> DeleteAsync(int id);

        Task<IReadOnlyCollection<UserDetailsDTO>> GetAllSupervisors(); 

    }
