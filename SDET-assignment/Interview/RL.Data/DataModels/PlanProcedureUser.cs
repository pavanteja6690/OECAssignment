namespace RL.Data.DataModels
{
    public class PlanProcedureUser
    {
        public int PlanProcedureUsersUserId { get; set; }
        public int UserPlanProceduresPlanId { get; set; }
        public int UserPlanProceduresProcedureId { get; set; }

        public User User { get; set; }
        public PlanProcedure PlanProcedure { get; set; }
    }

}
