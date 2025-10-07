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

            var response = await _graphQLClient.SendQueryAsync<RentalsTienDtGraphQLResponse>(query);
            var result = response?.Data?.rentalsTienDts?.ToList();
            return result;
        }

        public async Task<int> CreateRentalsAsync(RentalsTienDt rentalsTienDt)
        {
            try
            {
                var graphQLRequest = new GraphQLRequest()
                {
                    Query = @"mutation CreateRentalsTienDt ($input: RentalsTienDtInput!) {
    createRentalsTienDt(rentalsTienDt: $input)}",
                    Variables = new {input = rentalsTienDt}
                };
                var response = await _graphQLClient.SendMutationAsync<CreateGraphQLResponse>(graphQLRequest);
                var result = response.Data.createRentalsTienDt;
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


    } 
}
