


    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _repository;

        
        public UserController(ILogger<UserController> logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [ProducesResponseType(typeof(UserDetailsDTO), StatusCodes.Status201Created)]
        //[ProducesResponseType(typeof(UserDetailsDTO), StatusCodes.)] - tilføj statuscode såfremt user ikke skabes
        [HttpPost]
        public async Task<UserDetailsDTO> Post(UserCreateDTO user)
        {
            return await _repository.CreateAsync(user);
            
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDetailsDTO), StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public async Task<UserDetailsDTO> Get(int id) 
        {
            return await _repository.FindUserByIdAsync(id);
        }

        
        [AllowAnonymous] //overrides the authorize annotation, used bcuz the other stuff doesn't work 
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IReadOnlyCollection<UserDetailsDTO>),StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IReadOnlyCollection<UserDetailsDTO>> GetSupervisors()
        {
            return await _repository.GetAllSupervisors();
        }
    }
