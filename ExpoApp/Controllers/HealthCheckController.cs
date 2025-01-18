using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;

[Route("")]
[ApiController]
public class HealthCheckController() : ControllerBase
{
	[HttpGet]
	[AllowAnonymous]
	public ActionResult HealthCheck()
	{
		return Ok();
	}
}