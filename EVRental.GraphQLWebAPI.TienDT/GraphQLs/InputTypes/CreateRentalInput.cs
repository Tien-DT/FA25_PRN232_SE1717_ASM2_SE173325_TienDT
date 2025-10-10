namespace EVRental.GraphQLWebAPI.TienDT.GraphQLs.InputTypes
{
    public class CreateRentalInput
    {
        public int UserAccountId { get; set; }
        public int VehicleId { get; set; }
        public int StationId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? PlannedEndTime { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal SecurityDeposit { get; set; }
        public string Note { get; set; } = string.Empty;
        public int? RentalStatusTienDtid { get; set; }
        public bool? IsCompleted { get; set; }
        public bool? IsActive { get; set; }
    }
}
