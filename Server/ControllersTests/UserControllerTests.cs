using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;


    public class UserControllerTests
    {

        private readonly UserController _controller;
        private readonly Mock<IUserRepository> _repoMock; 

        public UserControllerTests()
        {
            var logMock = new Mock<ILogger<UserController>>();
            _repoMock = new Mock<IUserRepository>();
            _controller = new UserController(logMock.Object, _repoMock.Object);

        }

        [Fact]
        public async Task POST_given_userCreateDTO_returns_object()
        {
            var user = new UserCreateDTO();
            var userDetails = new UserDetailsDTO {
                Id = 2,
                UserName = "Lars",
                UserType = "Student",
                Email = "Lars@itu.dk"
            };
            _repoMock.Setup(u => u.CreateAsync(user)).ReturnsAsync(userDetails);

            var result = await _controller.Post(user);

            Assert.NotNull(result);
            
        }


        [Fact]
        public async Task FindById_given_non_existing_id_returns_null()
        {
            #region Arrange
            var id = 666;
            UserDetailsDTO? user = null;
            _repoMock.Setup(m => m.FindUserByIdAsync(id)).ReturnsAsync(user);
            #endregion

            #region Act
            var result = await _controller.Get(id);
            #endregion

            #region Assert
            Assert.Null(result);
            #endregion
        }

        [Fact]
        public async Task FindById_given_existing_id_returns_user()
        {
            #region Arrange
            var id = 1;
            UserDetailsDTO user = new UserDetailsDTO{Id = 1, UserName = "SuperDan", UserType = "Supervisor", Email = "dan@mail.dk"};
            _repoMock.Setup(m => m.FindUserByIdAsync(id)).ReturnsAsync(user);
            #endregion

            #region Act
            var result = await _controller.Get(id);
            #endregion

            #region Assert
            Assert.Equal(user, result);
            #endregion
        }

        [Fact]
        public async Task Delete_given_valid_id_returns_that_id()
        {
            var id = 1; 
            _repoMock.Setup(m => m.DeleteAsync(id)).ReturnsAsync(id);
            
            var result = await _controller.Delete(id);

            Assert.Equal(id, result); 
        }


        //RET TIL, DET ER IKKE MENINGEN AT -1 SKAL RETURNERES I TILFÆLDE AF FEJL
        [Fact]
        public async Task Delete_given_invalid_id_returns_minus_one()
        {
            var id = 1; 
            _repoMock.Setup(m => m.DeleteAsync(id)).ReturnsAsync(-1);
            
            var result = await _controller.Delete(id);

            Assert.Equal(-1, result); 
        }

        [Fact]
        public async Task GetAllSupervisors_returns_all_supervisors()
        {
            var Mikki = new UserDetailsDTO
            {
                Id = 1,
                UserName = "Mikki",
                UserType = "Supervisor",
                Email = "mikki@erstor.dk"

            };

            var Sofia = new UserDetailsDTO
            {
                Id = 2,
                UserName = "Sofia",
                UserType = "Supervisor",
                Email = "sofkj@itu.dk"

            };
    
            _repoMock.Setup(m => m.GetAllSupervisors()).ReturnsAsync(new List<UserDetailsDTO>{Mikki, Sofia}.AsReadOnly());

            var result = await _controller.GetSupervisors();
            Assert.Equal(2, result.Count());
        }
    }
