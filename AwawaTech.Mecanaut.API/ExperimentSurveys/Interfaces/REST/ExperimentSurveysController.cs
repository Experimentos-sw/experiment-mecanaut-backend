using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Services;
using AwawaTech.Mecanaut.API.ExperimentSurveys.Interfaces.REST.Resources;
using AwawaTech.Mecanaut.API.ExperimentSurveys.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Interfaces.REST;

[ApiController]
[Route("api/v1/experiment-surveys")]
public class ExperimentSurveysController : ControllerBase
{
    private readonly IExperimentSurveyCommandService _commandService;
    private readonly IExperimentSurveyQueryService _queryService;

    public ExperimentSurveysController(
        IExperimentSurveyCommandService commandService,
        IExperimentSurveyQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateExperimentSurveyResource resource)
    {
        var command = CreateExperimentSurveyCommandFromResourceAssembler.ToCommand(resource);
        var survey = await _commandService.HandleAsync(command);
        var result = ExperimentSurveyToResourceAssembler.ToResource(survey);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var surveys = await _queryService.ListAsync();
        var resources = surveys.Select(survey => ExperimentSurveyToResourceAssembler.ToResource(survey)).ToList();

        return Ok(resources);
    }
    
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
    {
        var survey = await _queryService.FindByIdAsync(id);

        if (survey is null)
            return NotFound();

        var resource = ExperimentSurveyToResourceAssembler.ToResource(survey);

        return Ok(resource);
    }
}