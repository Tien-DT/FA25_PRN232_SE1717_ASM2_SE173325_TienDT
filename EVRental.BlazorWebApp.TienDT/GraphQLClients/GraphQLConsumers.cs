using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVRental.BlazorWebApp.TienDT.Models;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace EVRental.BlazorWebApp.TienDT.GraphQLClients
{
    public class GraphQLConsumers
    {
        private readonly IGraphQLClient _graphQLClient;

        public GraphQLConsumers(IGraphQLClient graphQLClient)
        {
            _graphQLClient = graphQLClient;
        }

        public async Task<List<RentalsTienDt>> GetRentalsAsync()
        {
            var query = @"query RentalsTienDts {
                rentalsTienDts {
                    rentalTienDtid
                    userAccountId
                    vehicleId
                    totalAmount
                    securityDeposit
                    note
                    createdDate
                    isActive
                    rentalStatusTienDtid
                }
            }";

            var request = new GraphQLRequest { Query = query };
            var response = await _graphQLClient.SendQueryAsync<RentalsTienDtGraphQLResponse>(request);
            var result = response?.Data?.rentalsTienDts ?? new List<RentalsTienDt>();
            return result;
        }



        public async Task<RentalsTienDt?> GetRentalByIdAsync(int id)
        {
            var query = @"query RentalById($id: Int!) {
                rentalById(id: $id) {
                    rentalTienDtid
                    userAccountId
                    vehicleId
                    stationId
                    startTime
                    endTime
                    plannedEndTime
                    totalAmount
                    securityDeposit
                    note
                    rentalStatusTienDtid
                    isCompleted
                    isActive
                    createdDate
                    updatedDate
                }
            }";

            var request = new GraphQLRequest { Query = query, Variables = new { id } };
            var response = await _graphQLClient.SendQueryAsync<RentalByIdResponse>(request);
            return response?.Data?.rentalById;
        }

        public async Task<int> UpdateRentalAsync(RentalsTienDt rental)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation UpdateRentalsTienDt($rentalTienDtid: Int!, $userAccountId: Int!, $vehicleId: Int!, $stationId: Int!, 
                        $startTime: DateTime!, $endTime: DateTime, $plannedEndTime: DateTime, $totalAmount: Decimal, 
                        $securityDeposit: Decimal!, $note: String!, $rentalStatusTienDtid: Int, $isCompleted: Boolean, $isActive: Boolean) {
                    updateRentalsTienDt(input: {
                        rentalTienDtid: $rentalTienDtid
                        userAccountId: $userAccountId
                        vehicleId: $vehicleId
                        stationId: $stationId
                        startTime: $startTime
                        endTime: $endTime
                        plannedEndTime: $plannedEndTime
                        totalAmount: $totalAmount
                        securityDeposit: $securityDeposit
                        note: $note
                        rentalStatusTienDtid: $rentalStatusTienDtid
                        isCompleted: $isCompleted
                        isActive: $isActive
                    })
                }",
                Variables = new { 
                    rentalTienDtid = rental.RentalTienDtid,
                    userAccountId = rental.UserAccountId,
                    vehicleId = rental.VehicleId,
                    stationId = rental.StationId,
                    startTime = DateTime.SpecifyKind(rental.StartTime, DateTimeKind.Utc),
                    endTime = rental.EndTime.HasValue ? DateTime.SpecifyKind(rental.EndTime.Value, DateTimeKind.Utc) : (DateTime?)null,
                    plannedEndTime = rental.PlannedEndTime.HasValue ? DateTime.SpecifyKind(rental.PlannedEndTime.Value, DateTimeKind.Utc) : (DateTime?)null,
                    totalAmount = rental.TotalAmount,
                    securityDeposit = rental.SecurityDeposit,
                    note = string.IsNullOrWhiteSpace(rental.Note) ? "No note provided" : rental.Note,
                    rentalStatusTienDtid = rental.RentalStatusTienDtid,
                    isCompleted = rental.IsCompleted,
                    isActive = rental.IsActive
                }
            };

            var response = await _graphQLClient.SendMutationAsync<UpdateGraphQLResponse>(request);
            return response.Data.updateRentalsTienDt;
        }



        public async Task<int> CreateRentalsAsync(RentalsTienDt rentalsTienDt)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation CreateRentalsTienDt($userAccountId: Int!, $vehicleId: Int!, $stationId: Int!, $startTime: DateTime!, 
                        $endTime: DateTime, $plannedEndTime: DateTime, $totalAmount: Decimal, $securityDeposit: Decimal!, 
                        $note: String!, $rentalStatusTienDtid: Int, $isCompleted: Boolean, $isActive: Boolean) {
                    createRentalsTienDt(input: {
                        userAccountId: $userAccountId
                        vehicleId: $vehicleId
                        stationId: $stationId
                        startTime: $startTime
                        endTime: $endTime
                        plannedEndTime: $plannedEndTime
                        totalAmount: $totalAmount
                        securityDeposit: $securityDeposit
                        note: $note
                        rentalStatusTienDtid: $rentalStatusTienDtid
                        isCompleted: $isCompleted
                        isActive: $isActive
                    })
                }",
                Variables = new { 
                    userAccountId = rentalsTienDt.UserAccountId,
                    vehicleId = rentalsTienDt.VehicleId,
                    stationId = rentalsTienDt.StationId,
                    startTime = DateTime.SpecifyKind(rentalsTienDt.StartTime, DateTimeKind.Utc),
                    endTime = rentalsTienDt.EndTime.HasValue ? DateTime.SpecifyKind(rentalsTienDt.EndTime.Value, DateTimeKind.Utc) : (DateTime?)null,
                    plannedEndTime = rentalsTienDt.PlannedEndTime.HasValue ? DateTime.SpecifyKind(rentalsTienDt.PlannedEndTime.Value, DateTimeKind.Utc) : (DateTime?)null,
                    totalAmount = rentalsTienDt.TotalAmount,
                    securityDeposit = rentalsTienDt.SecurityDeposit,
                    note = string.IsNullOrWhiteSpace(rentalsTienDt.Note) ? "No note provided" : rentalsTienDt.Note,
                    rentalStatusTienDtid = rentalsTienDt.RentalStatusTienDtid,
                    isCompleted = rentalsTienDt.IsCompleted,
                    isActive = rentalsTienDt.IsActive
                }
            };

            var response = await _graphQLClient.SendMutationAsync<CreateGraphQLResponse>(request);
            return response.Data.createRentalsTienDt;
        }

        public async Task<bool> DeleteRentalAsync(int rentalId)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation DeleteRentalsTienDt($rentalTienDtid: Int!) {
                    deleteRentalsTienDt(rentalTienDtid: $rentalTienDtid)
                }",
                Variables = new { rentalTienDtid = rentalId }
            };

            var response = await _graphQLClient.SendMutationAsync<DeleteGraphQLResponse>(request);
            return response.Data.deleteRentalsTienDt;
        }

        public async Task<List<RentalStatusesTienDt>> GetRentalStatusesTienDtsAsync()
        {
            var query = @"query GetRentalStatusesTienDts {
                rentalStatusesTienDts {
                    rentalStatusTienDtid
                    statusName
                }
            }";

            var response = await _graphQLClient.SendQueryAsync<RentalStatusesTienDtGraphQLResponse>(query);
            return response?.Data?.rentalStatusesTienDts ?? new List<RentalStatusesTienDt>();
        }

    }

    public class RentalByIdResponse
    {
        public RentalsTienDt? rentalById { get; set; }
    }

    public class UpdateGraphQLResponse
    {
        public int updateRentalsTienDt { get; set; }
    }
    public class DeleteGraphQLResponse
    {
        public bool deleteRentalsTienDt { get; set; }
    }

    public class CreateGraphQLResponse
    {
        public int createRentalsTienDt { get; set; }
    }

    public class RentalStatusesTienDtGraphQLResponse
    {
        public List<RentalStatusesTienDt> rentalStatusesTienDts { get; set; }
    }

}
