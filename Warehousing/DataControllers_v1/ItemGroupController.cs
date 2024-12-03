using Microsoft.AspNetCore.Mvc;
using Warehousing.DataServices_v1;


namespace Warehousing.DataControllers_v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ItemGroupsController : ControllerBase
    {
        private readonly ItemGroupService _itemGroupService;

        public ItemGroupsController(ItemGroupService itemGroupService)
        {
            _itemGroupService = itemGroupService;
        }

        [HttpGet]
        public IActionResult GetAllItemGroups() => Ok(_itemGroupService.GetAllItemGroups());

        [HttpGet("{id}")]
        public IActionResult GetItemGroupById(int id)
        {
            var itemGroup = _itemGroupService.GetItemGroupById(id);
            return Ok(itemGroup);
        }

        [HttpPost]
        public IActionResult AddItemGroup([FromBody] ItemGroup itemGroup)
        {
            _itemGroupService.AddItemGroup(itemGroup);
            return CreatedAtAction(nameof(GetItemGroupById), new { id = itemGroup.Id }, itemGroup);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateItemGroup(int id, [FromBody] ItemGroup updatedItemGroup)
        {
            _itemGroupService.UpdateItemGroup(id, updatedItemGroup);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItemGroup(int id)
        {
            _itemGroupService.DeleteItemGroup(id);
            return Ok();
        }
    }
}