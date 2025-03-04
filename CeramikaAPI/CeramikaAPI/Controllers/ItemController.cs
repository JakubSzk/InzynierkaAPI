using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using CeramikaAPI.Models;
using CeramikaAPI.Services;

namespace CeramikaAPI.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ItemService itemService;
        public ItemController() { itemService = new ItemService(); }

        [HttpPost("List")]
        [ProducesResponseType<List<ItemListModelDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult List(float min, float max, string author, string type, string[] tags)
        {
            List<ItemListModelDTO> returnable = itemService.ItemsByFilter(min, max, author, type, tags);
            return returnable == null ? BadRequest() : Ok(returnable);

        }

        [HttpPost("Details")]
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
        public IActionResult AddTag(string tag)
        {
            var hold = itemService.AddTag(tag);
            return hold ? Ok(true) : BadRequest();
        }

        [HttpPost("AddPhoto")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddPhoto(string photo)
        {
            var hold = itemService.AddPhoto(photo);
            return hold ? Ok(true) : BadRequest();
        }

        [HttpPost("DeleteTag")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteTag(string tag)
        {
            var hold = itemService.RemoveTag(tag);
            return hold ? Ok(true) : BadRequest();
        }

        [HttpPost("DeletePhoto")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeletePhoto(string photo)
        {
            var hold = itemService.RemovePhoto(photo);
            return hold ? Ok(true) : BadRequest();
        }

        [HttpPost("AddItem")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddItem(string name, string description, string type, int amount, float price, string model, string author)
        {
            var hold = itemService.AddItem(name, description, type, amount, price, model, author);
            return hold ? Ok(true) : BadRequest();
        }

        [HttpPost("AddTagsItem")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddTagsItem(int idItem, List<int> idTags)
        {
            var hold = itemService.AddTagsToItem(idItem, idTags);
            return hold ? Ok(true) : BadRequest();
        }

        [HttpPost("AddPhotosItem")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddPhotosItem(int idItem, List<int> idPhotos)
        {
            var hold = itemService.AddPhotosToItem(idItem, idPhotos);
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
