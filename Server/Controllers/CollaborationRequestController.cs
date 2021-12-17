


    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class CollaborationRequestController : ControllerBase
    {
        private readonly ILogger<CollaborationRequestController> _logger;

        private readonly ICollaborationRequestRepository _repository;

        public CollaborationRequestController(ILogger<CollaborationRequestController> logger,
            ICollaborationRequestRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IReadOnlyCollection<CollaborationRequestDetailsDTO>> Get()
        {
            //når der virkelig er data på bordet, så kan det være vi skal tjekke for om listen er tom.
            //dummy data
            var cb1 = new CollaborationRequestDetailsDTO
            {
                StudentId = 1,
                SupervisorId = 2,
                Application = "Science",
                Status = CollaborationRequestStatus.Waiting
            };
            var cb2 = new CollaborationRequestDetailsDTO
            {
                StudentId = 3,
                SupervisorId = 4,
                Application = "Not Science",
                Status = CollaborationRequestStatus.Waiting
            };
            return new List<CollaborationRequestDetailsDTO> { cb1, cb2 };
            //return await _repository.DERSKALLAVESENGETMETODE();
        }

        [Authorize]
        [HttpGet("{ideaid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(CollaborationRequestDetailsDTO), StatusCodes.Status200OK)]
        public async Task<IReadOnlyCollection<CollaborationRequestDetailsDTO>> GetByIdeaId(int id)
        {
            var requests = await _repository.FindRequestsByIdeaAsync(id);
            return requests;
        }

        [Authorize]
        [HttpGet("{userid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IReadOnlyCollection<CollaborationRequestDetailsDTO>> GetRequestsByUser(bool isSupervisor, int userId)
        {
            if (isSupervisor)
            {
                return await _repository.FindRequestsBySupervisorAsync(userId);
            }
            else
            {
                return await _repository.FindRequestsByStudentAsync(userId);
            }
        }


        [Authorize]
        [HttpPost]
        //[ProducesResponseType()]
        public async Task<IActionResult> Post(CollaborationRequestCreateDTO request)
        {
            var created = await _repository.CreateAsync(request);

            //det skal være created.Id og ikke application, men DTO har ikke id pt... 
            return CreatedAtAction(nameof(Get), new { created.Application }, created);
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<int> Delete(int id)
        {
            //hvis returtypen laves om på deleteasync, kan det være at denne metodes
            //returtype skal laves om til IActionResult i stedet og følge rasmus' eksempel
            return await _repository.DeleteAsync(id);
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<CollaborationRequestDetailsDTO> Put(int id,
            [FromBody] CollaborationRequestUpdateDTO request)
        {
            //denne er ligesom delete, den følger ikke 100% rasmus' eksempel ift. IActionResult returtype og returnering af .ToActionResult
            return (await _repository.UpdateAsync(id, request));
        }


    }
