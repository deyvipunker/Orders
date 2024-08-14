﻿using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.UnitsOfWork.Interfaces
{
    public interface IStateUnitOfWork
    {
        Task<ActionResponse<State>> GetAsync(int id);
        Task<ActionResponse<IEnumerable<State>>> GetAsync();
        Task<ActionResponse<IEnumerable<State>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}
