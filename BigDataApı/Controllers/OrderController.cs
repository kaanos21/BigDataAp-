using BigDataApi.Repositories.Abstract;
using BigDataApı.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BigDataApı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("OrderListWithPaging")]
        public async Task<IActionResult> OrderListWithPaging(int page, int pageSize)
        {
            var values = await _orderRepository.OrderListWithPaging(page, pageSize);
            return Ok(values);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var values = await _orderRepository.GetAllAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var value = await _orderRepository.GetByIdAsync(id);
            if (value == null) return NotFound();
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            await _orderRepository.AddAsync(order);
            return Ok("Sipariş başarıyla oluşturuldu.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var value = await _orderRepository.GetByIdAsync(id);
            if (value == null) return NotFound();

            await _orderRepository.DeleteAsync(id);
            return Ok("Sipariş silindi.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(Order order)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(order.OrderId);

            if (existingOrder == null)
            {
                return NotFound("Güncellenmek istenen sipariş bulunamadı.");
            }

            existingOrder.ProductId = order.ProductId;
            existingOrder.CustomerId = order.CustomerId;
            existingOrder.Quantity = order.Quantity;
            existingOrder.PaymentMethod = order.PaymentMethod;
            existingOrder.OrderStatus = order.OrderStatus;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.OrderNotes = order.OrderNotes;

            await _orderRepository.UpdateAsync(existingOrder);

            return Ok("Siparişin tüm bilgileri başarıyla güncellendi.");
        }
    }
}