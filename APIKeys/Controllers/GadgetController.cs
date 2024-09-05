using APIKeys.Data;
using APIKeys.Models;
using APIKeys.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace APIKeys.Controllers
{
    /// <summary>
    /// Controller to view Gagdets, all actions in the controller are protected by an API Key
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [APIKeyFilter]
    public class GadgetController : ControllerBase
    {
        private readonly ILogger<GadgetController> _logger;

        public GadgetController(ILogger<GadgetController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetGadgets")]
        public IEnumerable<Gadget> Get()
        {
            return new List<Gadget>(){new Gadget()
                {
                    Id = 1,
                    GadgetType = "Type A",
                    UsageInstructions = "Use Gadget A to assemble Widgets."
                },
                new Gadget()
                {
                    Id = 2,
                    GadgetType = "Type B",
                    UsageInstructions = "Use Gadget B to test Widgets."
                },
                new Gadget()
                {
                    Id = 3,
                    GadgetType = "Type C",
                    UsageInstructions = "Use Gadget C to use Widgets."
                }
            };
        }
    }
}