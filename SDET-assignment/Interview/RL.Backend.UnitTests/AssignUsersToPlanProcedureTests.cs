using FluentAssertions;
using Moq;
using RL.Backend.Commands;
using RL.Backend.Commands.Handlers.Plans;
using RL.Backend.Exceptions;
using RL.Data;

namespace RL.Backend.UnitTests;

[TestClass]
public class AssignUsersToPlanProcedureTests
{
    [TestMethod]
    [DataRow(-1, 1, new[] { 1 })]
    [DataRow(0, 1, new[] { 1 })]
    [DataRow(int.MinValue, 1, new int[0])]
    public async Task AssignUsersToPlanProcedure_InvalidPlanId_ReturnsBadRequest(int planId, int procedureId, int[] userIdsArray)
    {
        // Convert the int[] to List<int>
        var userIds = userIdsArray.ToList();

        // Given
        var context = new Mock<RLContext>();
        var sut = new AssignUsersToPlanProcedureCommandHandler(context.Object);
        var request = new AssignUsersToPlanProcedureCommand
        {
            PlanId = planId,
            ProcedureId = procedureId,
            UserIds = userIds
        };

        // When
        var result = await sut.Handle(request, new CancellationToken());

        // Then
        result.Exception.Should().BeOfType(typeof(BadRequestException));
        result.Succeeded.Should().BeFalse();
    }
}
