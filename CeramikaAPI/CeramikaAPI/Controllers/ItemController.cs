using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using CeramikaAPI.Models;
using CeramikaAPI.Services;

namespace CeramikaAPI.Controllers
{
    public class ItemCreationForm
    {
        public string name { get; set; } = null!;
        public string description { get; set; } = null!;
        public string type { get; set; } = null!;
        public int amount { get; set; }
        public float price { get; set; }
        public string model { get; set; } = null!;
        public string author { get; set; } = null!;
        public string token { get; set; } = null!;
    }
    [Route("api/items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ItemService itemService;
        public ItemController() { itemService = new ItemService(); }

        [HttpPost("List")]
        [ProducesResponseType<List<ItemListModelDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult List([FromForm] float min, [FromForm] float max, [FromForm] string[] tags, [FromForm] string author = "", [FromForm] string type = "")
        {
            List<ItemListModelDTO> returnable = itemService.ItemsByFilter(min, max, author, type, tags);
            return returnable == null ? BadRequest() : Ok(returnable);

        }
        [HttpGet("TagsList")]
        [ProducesResponseType<List<String>>(StatusCodes.Status200OK)]
        public IActionResult TagsList()
        {
            return Ok(itemService.GetTags());
        }

        [HttpGet("AuthorsList")]
        [ProducesResponseType<List<String>>(StatusCodes.Status200OK)]
        public IActionResult AuthorsList()
        {
            return Ok(itemService.GetAuthors());
        }

        [HttpGet("TypesList")]
        [ProducesResponseType<List<String>>(StatusCodes.Status200OK)]
        public IActionResult TypesList()
        {
            return Ok(itemService.GetTypes());
        }

        [HttpGet("Details")]
        [ProducesResponseType<ItemListModelDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Details(int idItem)
        {
            ItemListModelDTO returnable = itemService.ItemData(idItem);
            return returnable == null ? BadRequest() : Ok(returnable);

        }

        [HttpPost("AddTag")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddTag([FromForm]string tag, [FromForm]string token)
        {
            var hold = itemService.AddTag(tag, token);
            return hold ? Ok(true) : BadRequest();
        }

        [HttpPost("AddPhoto")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddPhoto([FromForm]string photo, [FromForm]string token)
        {
            var hold = itemService.AddPhoto(photo, token);
            return hold ? Ok(true) : BadRequest();
        }

        [HttpPost("DeleteTag")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteTag([FromForm]string tag, [FromForm]string token)
        {
            var hold = itemService.RemoveTag(tag, token);
            return hold ? Ok(true) : BadRequest();
        }

        [HttpPost("DeletePhoto")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeletePhoto([FromForm]string photo, [FromForm]string token)
        {
            var hold = itemService.RemovePhoto(photo, token);
            return hold ? Ok(true) : BadRequest();
        }

        [HttpPost("AddItem")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddItem([FromForm] ItemCreationForm itemCreationForm)
        {
            var hold = itemService.AddItem(itemCreationForm.name, itemCreationForm.description, itemCreationForm.type, itemCreationForm.amount, itemCreationForm.price, itemCreationForm.model, itemCreationForm.author, itemCreationForm.token);
            return hold ? Ok(true) : BadRequest();
        }

        [HttpPost("AddTagsItem")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddTagsItem([FromForm]int idItem, [FromForm]string token, [FromForm]int[] idTags)
        {
            var hold = itemService.AddTagsToItem(idItem, token, idTags);
            return hold ? Ok(true) : BadRequest();
        }

        [HttpPost("AddPhotosItem")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddPhotosItem([FromForm]int idItem, [FromForm]string token, [FromForm]int[] idPhotos)
        {
            var hold = itemService.AddPhotosToItem(idItem, token, idPhotos);
            return hold ? Ok(true) : BadRequest();
        }



        [HttpGet("Tags")]
        [ProducesResponseType<List<TagModel>>(StatusCodes.Status200OK)]
        public IActionResult ShowTags()
        {
            return Ok(itemService.ShowTags());
        }

        [HttpGet("Photos")]
        [ProducesResponseType<List<PhotoModel>>(StatusCodes.Status200OK)]
        public IActionResult ShowPhotos()
        {
            return Ok(itemService.ShowPhotos());
        }
    }
}
