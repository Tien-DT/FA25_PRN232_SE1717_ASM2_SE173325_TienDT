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
                }
            }";

            var request = new GraphQLRequest { Query = query };
            var response = await _graphQLClient.SendQueryAsync<RentalsTienDtGraphQLResponse>(request);
            var result = response?.Data?.rentalsTienDts ?? new List<RentalsTienDt>();
            return result;
        }

        public async Task<int> CreateRentalsAsync(RentalsTienDt rentalsTienDt)
        {
            try
            {
                var graphQLRequest = new GraphQLRequest()
                {
                    Query = @"mutation CreateRentalsTienDt($input: RentalsTienDtInput!) {
    createRentalsTienDt(rentalsTienDt: $input)
}",
                    Variables = new { input = rentalsTienDt }
                };
                var response = await _graphQLClient.SendMutationAsync<CreateGraphQLResponse>(graphQLRequest);
                var result = response?.Data?.createRentalsTienDt ?? 0;
                return result;

            }
            catch (Exception)
            {
                // Preserve stack trace; caller can handle/log as needed
                throw;
            }
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
            var mutation = @"mutation UpdateRentalsTienDt($input: RentalsTienDtInput!) {
                updateRentalsTienDt(rentalsTienDt: $input)
            }";

            var request = new GraphQLRequest { Query = mutation, Variables = new { input = rental } };
            var response = await _graphQLClient.SendMutationAsync<UpdateGraphQLResponse>(request);
            return response?.Data?.updateRentalsTienDt ?? 0;
        }

        public async Task<List<RentalStatusesTienDt>> GetRentalStatusesAsync()
        {
            var query = @"query RentalStatusesTienDts {
    rentalStatusesTienDts {
        statusName
    }
}";

            var request = new GraphQLRequest { Query = query };
            var response = await _graphQLClient.SendQueryAsync<RentalStatusesTienDtGraphQLResponse>(request);
            var result = response?.Data?.rentalStatusesTienDts ?? new List<RentalStatusesTienDt>();
            return result;
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

}
