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

        public async Task<SearchRentalsResponse> SearchRentalsAsync(string? note, decimal? securityDeposit, string? statusName, int currentPage = 1, int pageSize = 10)
        {
            var query = @"query SearchWithPaging($searchRequest: RentalsTienDtSearchRequestInput!) {
                searchWithPaging(searchRequest: $searchRequest) {
                    totalItems
                    totalPages
                    currentPage
                    pageSize
                    items {
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
                }
            }";

            var searchRequest = new
            {
                note = note,
                securityDeposit = securityDeposit,
                statusName = statusName,
                currentPage = currentPage,
                pageSize = pageSize
            };

            var request = new GraphQLRequest { Query = query, Variables = new { searchRequest } };
            var response = await _graphQLClient.SendQueryAsync<SearchRentalsGraphQLResponse>(request);
            return response?.Data?.searchWithPaging ?? new SearchRentalsResponse();
        }

        public async Task<SystemUserAccount?> LoginAsync(string username, string password)
        {
            var query = @"query Login($username: String!, $password: String!) {
                login(username: $username, password: $password) {
                    userAccountId
                    userName
                    password
                    fullName
                    email
                    phone
                    roleId
                    isActive
                }
            }";

            var request = new GraphQLRequest 
            { 
                Query = query, 
                Variables = new { username, password } 
            };
            
            var response = await _graphQLClient.SendQueryAsync<LoginGraphQLResponse>(request);
            return response?.Data?.login;
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

    public class SearchRentalsGraphQLResponse
    {
        public SearchRentalsResponse searchWithPaging { get; set; }
    }

    public class SearchRentalsResponse
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<RentalsTienDt> Items { get; set; } = new List<RentalsTienDt>();
    }

    public class LoginGraphQLResponse
    {
        public SystemUserAccount? login { get; set; }
    }

}
