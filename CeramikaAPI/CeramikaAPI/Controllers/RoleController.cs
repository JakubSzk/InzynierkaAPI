using CeramikaAPI.Models;
using CeramikaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CeramikaAPI.Controllers
{
    public class RoleForm
    {
        public string RoleName { get; set; }
    }

    public class RoleSetForm
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
    }

    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleService roleService;
        public RoleController() { roleService = new RoleService(); }

        [HttpGet]
        [ProducesResponseType<List<RoleModel>>(StatusCodes.Status200OK)]
        public IActionResult GetRoles()
        {
            return Ok(roleService.GetRoles());
        }

        [HttpPost("createRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateRole([FromForm] RoleForm roleForm)
        {
            RoleModel model = new RoleModel { Name = roleForm.RoleName };
            RoleModel role = roleService.AddRole(model);
            return role == null ? BadRequest() : Ok(role);
        }

        [HttpGet("getUserRoles")]
        [ProducesResponseType<List<UserRolesModelDTO>>(StatusCodes.Status200OK)]
        public IActionResult GetUserRoles()
        {
            return Ok(roleService.GetUserRoles());
        }

        [HttpPost("setRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateRole([FromForm] RoleSetForm roleSetForm)
        {
            UserRolesModelDTO model = roleService.SetRole(roleSetForm.RoleId, roleSetForm.UserId);
            return model == null ? BadRequest() : Ok(model);
        }
    }
}
