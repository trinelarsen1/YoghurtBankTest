

    public interface IIdeaRepository
    {
        Task<(HttpStatusCode code, IReadOnlyCollection<IdeaDetailsDTO> list)> FindIdeasBySupervisorIdAsync(int userId);

        Task<IdeaDetailsDTO> FindIdeaDetailsAsync(int IdeaId);

        Task<IdeaDetailsDTO> CreateAsync(IdeaCreateDTO idea);

        Task<int> DeleteAsync(int id);
        Task<IdeaDetailsDTO> UpdateAsync(int id, IdeaUpdateDTO update);

        Task<IReadOnlyCollection<IdeaDetailsDTO>> ReadAllAsync();
    }

