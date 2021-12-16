    public class IdeaRepository : IIdeaRepository
    {
        private readonly IYoghurtContext _context;

        public IdeaRepository(IYoghurtContext context) {
            _context = context;
        }
        public async Task<IdeaDetailsDTO> CreateAsync(IdeaCreateDTO idea)
        {
            var sup = (Supervisor) await _context.Users.FindAsync(idea.CreatorId);
            //var sup = (Supervisor) await _context.Users.Where(u => u.Id == idea.CreatorId).FirstOrDefault();
            //List<Idea> ideas = await _context.Ideas.Where(i => i.Creator.Id == sup.Id).Select(i => i).ToListAsync();
            if(sup == null)
            {
                Environment.Exit(1);
                //hvad fanden skal der ske brødre??? -> på en eller anden måde skal vi indikere at der skete en fejl
            }
            if(sup.Ideas == null)
            {
                Console.WriteLine("NEJ!!!");
                //Environment.Exit(1);
            }

            //husk noget null-checking 
            var entity = new Idea
            {
                Creator = sup,
                Title = idea.Title,
                Subject = idea.Subject,
                Description = idea.Description,
                AmountOfCollaborators = idea.AmountOfCollaborators,
                Open = idea.Open,
                TimeToComplete = idea.TimeToComplete,
                StartDate = idea.StartDate, 
                Type = idea.Type
            };

            sup.Ideas.Add(entity);
            _context.Ideas.Add(entity);
            await _context.SaveChangesAsync();

            return new IdeaDetailsDTO
            {
                Id = entity.Id,
                CreatorId = entity.Creator.Id,
                Title = entity.Title,
                Subject = entity.Subject,
                Posted = entity.Posted,
                Description = entity.Description,
                AmountOfCollaborators = entity.AmountOfCollaborators,
                Open = entity.Open,
                TimeToComplete = entity.TimeToComplete,
                StartDate = entity.StartDate
            };
        }

        public async Task<IdeaDetailsDTO> UpdateAsync(int id, IdeaUpdateDTO update)
        {
            var entity = await _context.Ideas.FindAsync(id);
            if(entity == null)
            {
                return null; //RETURN A STATUS INSTEAD
            }

            
            entity.Title = update.Title != null ? update.Title : entity.Title;
            entity.Subject = update.Subject != null ? update.Subject : entity.Subject;
            entity.Description = update.Description != null ? update.Description : entity.Description;
            entity.AmountOfCollaborators = update.AmountOfCollaborators != null ? update.AmountOfCollaborators : entity.AmountOfCollaborators;
            entity.Open = update.Open != null ? update.Open : entity.Open;
            entity.TimeToComplete = update.TimeToComplete != null ? update.TimeToComplete : entity.TimeToComplete;
            entity.StartDate = update.StartDate != null ? update.StartDate : entity.StartDate;
            entity.Type = update.Type != null ? update.Type : entity.Type;

            await _context.SaveChangesAsync();
            return new IdeaDetailsDTO
            {
                Id = entity.Id,
                CreatorId = entity.Creator.Id,
                Title = entity.Title,
                Subject = entity.Subject,
                Posted = entity.Posted,
                Description = entity.Description,
                AmountOfCollaborators = entity.AmountOfCollaborators,
                Open = entity.Open,
                TimeToComplete = entity.TimeToComplete,
                StartDate = entity.StartDate
            };
        }
        
        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _context.Ideas.FindAsync(id);
            if(entity == null)
            {
                return -1; //needs to be changed! 
            }
            _context.Ideas.Remove(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
        
        
        public async Task<IdeaDetailsDTO> FindIdeaDetailsAsync(int IdeaId)
        {
            var idea = _context.Ideas.Where(i => i.Id == IdeaId).Include(i => i.Creator).FirstOrDefault();
            //eager loading according to: https://docs.microsoft.com/en-us/ef/ef6/querying/related-data 
            //for some reason, creators can be null here - lazy loading error or something else? https://entityframeworkcore.com/knowledge-base/39434878/how-to-include-related-tables-in-dbset-find--- 
           //var idea = await _context.Ideas.FindAsync(IdeaId);
           
           //improve this -> status codes? 
            
            if(idea == null)
            {
                return null;
            }

            return new IdeaDetailsDTO
            {
                Id = idea.Id,
                Title = idea.Title,
                Subject = idea.Subject,
                Posted = idea.Posted,
                Description = idea.Description,
                AmountOfCollaborators = idea.AmountOfCollaborators,
                Open = idea.Open,
                TimeToComplete = idea.TimeToComplete,
                StartDate = idea.StartDate,
                CreatorId = idea.Creator.Id,
                Type = idea.Type
            };
        }


        public async Task<(HttpStatusCode code, IReadOnlyCollection<IdeaDetailsDTO> list)> FindIdeasBySupervisorIdAsync(int userId)
        {
            var supervisor = (Supervisor) _context.Users.Find(userId);

            if (supervisor == null) 
            {
                return (HttpStatusCode.NotFound, null);
            } else 
            {
            var ideas = await _context.Ideas.Where(i => i.Creator.Id == userId).Select(i =>
            new IdeaDetailsDTO {
                Id = i.Id,
                Title = i.Title,
                Subject = i.Subject,
                Type = i.Type
            }).ToListAsync();
            
            return (HttpStatusCode.Accepted, ideas.AsReadOnly());
            }
            
        }

        public async Task<IReadOnlyCollection<IdeaDetailsDTO>> ReadAllAsync()
        {
            var AllIdeas =  await _context.Ideas
            .Select(i => 
            new IdeaDetailsDTO {
                CreatorId = i.Creator.Id,
                Id = i.Id,
                Title = i.Title,
                Subject = i.Subject,
                Posted = i.Posted,
                Description = i.Description,
                AmountOfCollaborators = i.AmountOfCollaborators,
                Open = i.Open,
                TimeToComplete = i.TimeToComplete,
                StartDate = i.StartDate,
                Type = i.Type
            }).ToListAsync();

            return AllIdeas.AsReadOnly();
        }
    }
