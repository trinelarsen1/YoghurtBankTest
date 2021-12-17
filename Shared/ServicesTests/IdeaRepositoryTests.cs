using Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace YoghurtBank.ServicesTests
{

    public class IdeaRepositoryTests : IDisposable
    {
        private readonly YoghurtContext _context;
        private readonly IdeaRepository _repo;
        private DateTime _now;
        private DateTime _today;

        public IdeaRepositoryTests()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<YoghurtContext>();
            builder.UseSqlite(connection);
            var context = new YoghurtContext(builder.Options);
            context.Database.EnsureCreated();
            _now = DateTime.Now;
            _today = DateTime.Today;

            var supervisor1 = new Supervisor { Id = 1, UserName = "Torben", CollaborationRequests = new List<CollaborationRequest>(), Ideas = new List<Idea>(), Email = "test@test.dk" };
            var supervisor2 = new Supervisor { Id = 2, UserName = "Preben", CollaborationRequests = new List<CollaborationRequest>(), Ideas = new List<Idea>(), Email = "test@test.dk" };

            var idea1 = new Idea { Id = 1, Creator = supervisor2, Posted = _now, Subject = "Big Data", Title = "Big data is good", Description = "Big data gives value", AmountOfCollaborators = 3, Open = true, TimeToComplete = _today - _today, StartDate = _now, Type = IdeaType.Bachelor };
            var idea2 = new Idea { Id = 2, Creator = supervisor1, Posted = _now, Subject = "Data Intelligence", Title = "Data Intelligence is good", Description = "Data Intelligence gives value", AmountOfCollaborators = 1, Open = true, TimeToComplete = _today - _today, StartDate = _now, Type = IdeaType.PhD };
            var idea3 = new Idea { Id = 3, Creator = supervisor2, Posted = _now, Subject = "DevOps", Title = "DevOps is good", Description = "DevOps gives value", AmountOfCollaborators = 2, Open = true, TimeToComplete = _today - _today, StartDate = _now, Type = IdeaType.Project };
            var idea4 = new Idea { Id = 4, Creator = supervisor1, Posted = _now, Subject = "Requirements Elicitation", Title = "Requirements Elicitation is good", Description = "Requirements Elicitation gives value", AmountOfCollaborators = 5, Open = true, TimeToComplete = _today - _today, StartDate = _now, Type = IdeaType.Masters };
            //DateTime.Now-DateTime.Today

            supervisor1.Ideas.Add(idea2);
            supervisor1.Ideas.Add(idea4);
            supervisor2.Ideas.Add(idea1);
            supervisor2.Ideas.Add(idea3);

            context.Ideas.AddRange(idea1, idea2, idea3, idea4);
            context.Users.AddRange(supervisor1, supervisor2);

            context.SaveChanges();
            _context = context;
            _repo = new IdeaRepository(_context);

        }

        [Fact]
        public async Task FindIdeaDetailsAsync_given_valid_id_returns_details()
        {
            #region Arrange
            var id = 1;
            #endregion

            #region Act
            var result = await _repo.FindIdeaDetailsAsync(id);
            #endregion

            #region Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Big Data", result.Subject);
            Assert.Equal("Big data is good", result.Title);
            Assert.Equal("Big data gives value", result.Description);
            Assert.Equal(3, result.AmountOfCollaborators);
            Assert.Equal(2, result.CreatorId);
            Assert.True(result.Open);
            //datetime properties also?
            #endregion
        }

        [Fact]
        public async Task FindIdeaDetailsAsync_given_invalid_id_returns_null()
        {
            #region Arrange
            var id = 500;
            #endregion

            #region Act
            var result = await _repo.FindIdeaDetailsAsync(id);
            #endregion

            #region Assert
            //needs to be changed when return value of method is changed
            Assert.Null(result);
            #endregion

        }

        [Fact]
        public async Task DeleteAsync_given_valid_id_deletes_it()
        {
            #region Arrange
            var id = 1;
            #endregion

            #region Act
            var result = await _repo.DeleteAsync(id);
            var entity = await _context.Ideas.FindAsync(id);
            #endregion

            #region Assert
            Assert.Equal(1, result);
            Assert.Null(entity);
            #endregion
        }

        //TODO change this when return value of delete is changed
        [Fact]
        public async Task DeleteAsync_given_invalid_id_returns_minusone()
        {
            var id = 500;
            //make sure it doesnt exists initally
            var entity = await _context.Ideas.FindAsync(id);
            Assert.Null(entity);

            var result = await _repo.DeleteAsync(id);
            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task UpdateAsync_given_existing_entity_updates_it()
        {
            #region Arrange
            var id = 1;
            //get intital 
            var entity = await _context.Ideas.FindAsync(id);

            var update = new IdeaUpdateDTO
            {
                Title = "NewTitle",
                Subject = "NewSubject",
                Description = "NewDescription",
                AmountOfCollaborators = 400,
                Open = false
            };
            #endregion

            #region Act
            var result = await _repo.UpdateAsync(id, update);
            //get the actual entity to verify the updates

            var updatedEntity = await _context.Ideas.FindAsync(id);

            #endregion
            #region Assert
            Assert.NotNull(result);
            Assert.Equal("NewTitle", result.Title);
            Assert.Equal("NewSubject", result.Subject);
            Assert.Equal("NewDescription", result.Description);
            Assert.Equal(400, result.AmountOfCollaborators);
            Assert.False(result.Open);


            Assert.NotNull(updatedEntity);
            Assert.Equal("NewTitle", updatedEntity.Title);
            Assert.Equal("NewSubject", updatedEntity.Subject);
            Assert.Equal("NewDescription", updatedEntity.Description);
            Assert.Equal(400, updatedEntity.AmountOfCollaborators);
            Assert.False(updatedEntity.Open);
            Assert.Equal(entity.TimeToComplete, updatedEntity.TimeToComplete);
            Assert.Equal(entity.StartDate, updatedEntity.StartDate);
            Assert.Equal(entity.Type, updatedEntity.Type);
            #endregion
        }

        [Fact]
        public async Task UpdateAsync_given_nonexisting_entity_returns_null()
        {
            var id = 666;
            
            var update = new IdeaUpdateDTO
            {
                Title = "NewTitle",
                Subject = "NewSubject",
                Description = "NewDescription",
                AmountOfCollaborators = 400,
                Open = false
            };

            var result = await _repo.UpdateAsync(id, update);
            
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_given_idea_returns_it()
        {
            #region Arrange
            var idea = new IdeaCreateDTO
            {
                CreatorId = 2,
                Title = "GADDAG's and their use in ScrabbleBots",
                Subject = "Scrabble",
                Description = "Heya Sweden",
                AmountOfCollaborators = 2,
                Open = true,
                TimeToComplete = _today - _today,
                StartDate = _now,
                Type = IdeaType.Bachelor
            };
            #endregion

            #region Act
            var result = await _repo.CreateAsync(idea);
            #endregion

            #region Assert
            Assert.NotNull(result);
            //needs more assertions
            #endregion
        }

        [Fact]
        public async Task CreateAsync_given_something_something()
        {
            //get 100% coverage when createasync is not a mess
        }

        [Fact]
        public async Task FIndIdeaBySupervisor_given_invalid_SupervisorId_returns_null()
        {
            int supervisorid = 5;
            
            var ideasFromSupervisorId = await _repo.FindIdeasBySupervisorIdAsync(supervisorid);

            Assert.Equal((HttpStatusCode.NotFound, null), ideasFromSupervisorId);

        }

        [Fact]
        public async Task FIndIdeaBySupervisor_given_valid_UserId_returns_list_with_two_elements()
        {
            int supervisorid = 2;
            var ideasFromSupervisorId = await _repo.FindIdeasBySupervisorIdAsync(supervisorid);

            var ideaDTO1 = new IdeaDetailsDTO
            {
                Id = 1,
                Title = "Big data is good",
                Subject = "Big Data",
                Type = IdeaType.Bachelor
            };

            var ideaDTO2 = new IdeaDetailsDTO
            {
                Id = 3,
                Title = "DevOps is good",
                Subject = "DevOps",
                Type = IdeaType.Project
            };

            Assert.Equal(HttpStatusCode.Accepted, ideasFromSupervisorId.code);
            Assert.Collection(ideasFromSupervisorId.list,
            idea => Assert.Equal(ideaDTO1, idea),
            idea => Assert.Equal(ideaDTO2, idea)
            );
        }

        //Todo fix broken test - Timespan is off (precision)
        [Fact]
        public async Task ReadAllAsync_returns_all_elements_in_context()
        {
            var ideas = await _repo.ReadAllAsync();


            var supervisor1 = _context.Users.Find(1);
            var supervisor2 = _context.Users.Find(2);

            var IdeaDetailsDTO1 = new IdeaDetailsDTO
            {
                CreatorId = supervisor1.Id,
                Id = 2,
                Title = "Data Intelligence is good",
                Subject = "Data Intelligence",
                Posted = _now,
                Description = "Data Intelligence gives value",
                AmountOfCollaborators = 1,
                Open = true,
                TimeToComplete = _today - _today,
                StartDate = _now,
                Type = IdeaType.PhD
            };

            var IdeaDetailsDTO2 = new IdeaDetailsDTO
            {
                CreatorId = supervisor1.Id,
                Id = 4,
                Title = "Requirements Elicitation is good",
                Subject = "Requirements Elicitation",
                Posted = _now,
                Description = "Requirements Elicitation gives value",
                AmountOfCollaborators = 5,
                Open = true,
                TimeToComplete = _today - _today,
                StartDate = _now,
                Type = IdeaType.Masters
            };
            var IdeaDetailsDTO3 = new IdeaDetailsDTO
            {
                CreatorId = supervisor2.Id,
                Id = 1,
                Title = "Big data is good",
                Subject = "Big Data",
                Posted = _now,
                Description = "Big data gives value",
                AmountOfCollaborators = 3,
                Open = true,
                TimeToComplete = _today - _today,
                StartDate = _now,
                Type = IdeaType.Bachelor
            };
            var IdeaDetailsDTO4 = new IdeaDetailsDTO
            {
                CreatorId = supervisor2.Id,
                Id = 3,
                Title = "DevOps is good",
                Subject = "DevOps",
                Posted = _now,
                Description = "DevOps gives value",
                AmountOfCollaborators = 2,
                Open = true,
                TimeToComplete = _today - _today,
                StartDate = _now,
                Type = IdeaType.Project
            };

            Assert.Collection(ideas,
            idea => Assert.Equal(IdeaDetailsDTO1.ToString(), idea.ToString()),
            idea => Assert.Equal(IdeaDetailsDTO2.ToString(), idea.ToString()),
            idea => Assert.Equal(IdeaDetailsDTO3.ToString(), idea.ToString()),
            idea => Assert.Equal(IdeaDetailsDTO4.ToString(), idea.ToString())
            );
        }

        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}