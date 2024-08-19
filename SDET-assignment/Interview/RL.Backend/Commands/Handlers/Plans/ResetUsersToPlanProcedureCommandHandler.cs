using MediatR;
using Microsoft.EntityFrameworkCore;
using RL.Backend.Exceptions;
using RL.Backend.Models;
using RL.Data;

namespace RL.Backend.Commands.Handlers.Plans;

public class ResetUsersToPlanProcedureCommandHandler : IRequestHandler<ResetUsersToPlanProcedureCommand, ApiResponse<Unit>>
{
    private readonly RLContext _context;

    public ResetUsersToPlanProcedureCommandHandler(RLContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Unit>> Handle(ResetUsersToPlanProcedureCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate request
            if (request.PlanId < 1)
                return ApiResponse<Unit>.Fail(new BadRequestException("Invalid PlanId"));
            if (request.ProcedureId < 1)
                return ApiResponse<Unit>.Fail(new BadRequestException("Invalid ProcedureId"));

            var plan = await _context.Plans
                .Include(p => p.PlanProcedures)
                    .ThenInclude(pp => pp.PlanProcedureUsers)
                .FirstOrDefaultAsync(p => p.PlanId == request.PlanId, cancellationToken);

            if (plan == null)
                return ApiResponse<Unit>.Fail(new NotFoundException($"PlanId: {request.PlanId} not found"));

            var planProcedure = plan.PlanProcedures
                .FirstOrDefault(pp => pp.ProcedureId == request.ProcedureId);

            if (planProcedure == null)
                return ApiResponse<Unit>.Fail(new NotFoundException($"ProcedureId: {request.ProcedureId} not found"));

            // Clear existing users and add new ones
            planProcedure.PlanProcedureUsers.Clear();
            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponse<Unit>.Succeed(Unit.Value);
        }
        catch (Exception e)
        {
            return ApiResponse<Unit>.Fail(e);
        }
    }
}
