﻿using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Implementations
{
    public class StatesRepository : GenericRepository<State>, IStatesRepository
    {
        private readonly DataContext _context;

        public StatesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ActionResponse<State>> GetAsync(int id)
        {
            var state = await _context.States
                .Include(c => c.Cities!)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (state == null)
            {
                return new ActionResponse<State>
                {
                    WasSucceess = false,
                    Message = "Departamento no existe"
                };
            }

            return new ActionResponse<State>
            {
                WasSucceess = true,
                Result = state
            };
        }

        public override async Task<ActionResponse<IEnumerable<State>>> GetAsync()
        {
            var states = await _context.States
               .OrderBy(x=> x.Name)
               .Include(c => c.Cities)
               .ToListAsync();
            return new ActionResponse<IEnumerable<State>>
            {
                WasSucceess = true,
                Result = states
            };
        }
        public override async Task<ActionResponse<IEnumerable<State>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.States
                .Include(c => c.Cities)
                .Where(x=> x.Country!.Id == pagination.Id)
                .AsQueryable();

            return new ActionResponse<IEnumerable<State>>
            {
                WasSucceess = true,
                Result = await queryable
                .OrderBy(c => c.Name)
                .Paginate(pagination)
                .ToListAsync()
            };
        }
        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.States
                .Where(x => x.Country!.Id == pagination.Id)
                .AsQueryable();
           
            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecorsNumber);
            return new ActionResponse<int>
            {
                WasSucceess = true,
                Result = totalPages
            };
        }
    }
}
