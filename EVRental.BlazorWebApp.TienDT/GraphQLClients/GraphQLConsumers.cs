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
                Query = @"mutation UpdateRentalsTienDt($input: RentalsTienDtInput!) {
                    updateRentalsTienDt(rentalsTienDt: $input)
                }",
                Variables = new { input = rental }
            };

            var response = await _graphQLClient.SendMutationAsync<UpdateGraphQLResponse>(request);
            return response.Data.updateRentalsTienDt;
        }



        public async Task<int> CreateRentalsAsync(RentalsTienDt rentalsTienDt)
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation CreateRentalsTienDt($input: RentalsTienDtInput!) {
                    createRentalsTienDt(rentalsTienDt: $input)
                }",
                Variables = new { input = rentalsTienDt }
            };

            var response = await _graphQLClient.SendMutationAsync<CreateGraphQLResponse>(request);
            return response.Data.createRentalsTienDt;
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

    public class CreateGraphQLResponse
    {
        public int createRentalsTienDt { get; set; }
    }

    public class RentalStatusesTienDtGraphQLResponse
    {
        public List<RentalStatusesTienDt> rentalStatusesTienDts { get; set; }
    }

}
