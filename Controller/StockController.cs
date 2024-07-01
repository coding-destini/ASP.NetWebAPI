using api.Data; // This includes the namespace api.Data so that classes and methods within that namespace can be used in this file.
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // This includes ASP.NET Core MVC components, enabling the use of controllers, actions, and routing.

namespace api.Controller
{
    [Route("api/stock")]
    [ApiController] // This attribute indicates that this class is an API controller and provides automatic model state validation, among other things.
    public class StockController : ControllerBase //StockController that inherits from ControllerBase, which provides basic functionality for handling HTTP requests.
    {
        private readonly ApplicationDBContext dbContext; // Declares a read-only field to hold the database context.
        private readonly IStockRepository _stockRepo;

        // This is a constructor that takes an ApplicationDBContext parameter. The provided context is assigned to the dbContext field.
        // This is an example of dependency injection, where ApplicationDBContext is injected into the controller by the dependency injection framework.
        public StockController(ApplicationDBContext context, IStockRepository stockrepo)
        {
            dbContext = context;
            _stockRepo = stockrepo;
        }

        //Get all stocks
        [HttpGet]
        //In ASP.NET Core, controller actions can return different types of results.When an action method is asynchronous, it returns a Task that represents the ongoing operation.
        //    The Task can be parameterized with a type, indicating the type of result the task will produce when it completes.

        //Task<IActionResult>: Indicates that the asynchronous method will eventually return an IActionResult. 
        //    IActionResult is a common return type for controller actions that can produce various types of HTTP responses, like Ok, NotFound, BadRequest, etc.
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            var stocks = await _stockRepo.GetAllAsync(query);
            var stockDTO = stocks.Select(s => s.ToStockDto()); //This retrieves all stock records from the database as a list.
            return Ok(stockDTO);
        }

        //Get stock by Id
        [HttpGet("{id:int}")] //This attribute specifies that this action responds to HTTP GET requests with a URL parameter (id).
        public async Task<IActionResult> GetById([FromRoute] int id)
        { //This method takes an id parameter from the route and returns an IActionResult. [FromRoute]int id: This is model binding, where the value of id in the URL is automatically bound to the id parameter of the method.
            var stock = await _stockRepo.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(new
            {
                message = "Stock found 😉",
                Stock = stock.ToStockDto()
            });
        }

        [HttpPost]
        //IActionResult, and takes a CreateStockRequestDto object as a parameter.The [FromBody]
        //attribute indicates that the stockDto should be deserialized from the JSON body of the HTTP request. 
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stockModel = stockDto.ToStockfromCreateDto(); //This line converts the CreateStockRequestDto (stockDto) into a Stock entity (stockModel) using the ToStockfromCreateDto mapping method.
         
            await _stockRepo.CreateAsync(stockModel); 

            return CreatedAtAction(nameof(GetById),
                new { id = stockModel.Id }, 
                stockModel.ToStockDto()); // The CreatedAtAction method in ASP.NET Core creates a Location header pointing to the URL of
                                          // the newly created resource. This method requires the action name (GetById in this case) and the route
                                          // values (e.g., id) to build the URL. It uses the route data to generate the URL dynamically.  
                                          //Ex : In response header we will get this : location: https://localhost:7182/api/stock/id , by this we can directly access our stock  
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto UpdateStockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stockModel = await _stockRepo.UpdateByIdAsync(id,UpdateStockDto);
            if (stockModel == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                message = $"Stock model with Id = {stockModel.Id} is Updated successfully 🙌",
                stock = stockModel.ToStockDto()
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var stockModel = await _stockRepo.DeleteByIdAsync(id);
            if (stockModel == null)
            {
                return NotFound();
            }
            return Ok(new
            {
                message = "Stock Deleted Successfully 😊"
            });
        }
    }
}
