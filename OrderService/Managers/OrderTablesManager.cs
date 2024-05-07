﻿using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Interfaces;
using OrderService.Models;

namespace OrderService.Managers;

public class OrderTablesManager : IOrderTables
{
    private readonly MyDbContext _context;

    public OrderTablesManager(MyDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrderTable>> GetAllOrdersAsync()
    {
        return await _context.OrderTables.ToListAsync();
    }

    public async Task<OrderTable> GetOrderByIdAsync(int orderId)
    {
        return await _context.OrderTables.FindAsync(orderId);
    }

    public async Task AddOrderAsync(OrderTable order)
    {
        _context.OrderTables.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOrderAsync(OrderTable order)
    {
        _context.Entry(order).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteOrderAsync(int orderId)
    {
        var order = await _context.OrderTables.FindAsync(orderId);
        if (order != null)
        {
            _context.OrderTables.Remove(order);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<OrderTable>> GetOrdersByCustomerAndDateAsync(int customerId, DateTime start, DateTime end)
    {
        return await _context.OrderTables
            .Where(o => o.CustomerId == customerId && o.OrderDate >= start && o.OrderDate <= end)
            .ToListAsync();
    }

    public async Task DeleteOrderAsync(OrderTable order)
    {
        _context.OrderTables.Remove(order);
        await _context.SaveChangesAsync();
    }
}