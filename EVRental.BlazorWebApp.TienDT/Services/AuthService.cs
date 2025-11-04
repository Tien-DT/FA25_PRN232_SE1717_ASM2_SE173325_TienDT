using Microsoft.JSInterop;
using System.Text.Json;
using EVRental.BlazorWebApp.TienDT.Models.Auth;
using EVRental.BlazorWebApp.TienDT.GraphQLClients;

namespace EVRental.BlazorWebApp.TienDT.Services
{
    public class AuthService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _httpClient;
        private readonly GraphQLConsumers _graphQLConsumers;
        private const string TOKEN_KEY = "authToken";
        private const string USER_KEY = "userInfo";

        public AuthService(IJSRuntime jsRuntime, HttpClient httpClient, GraphQLConsumers graphQLConsumers)
        {
            _jsRuntime = jsRuntime;
            _httpClient = httpClient;
            _graphQLConsumers = graphQLConsumers;
        }

        public async Task<LoginResponse> LoginAsync(LoginModel loginModel)
        {
            try
            {
                // Call GraphQL API to authenticate user from database
                var userAccount = await _graphQLConsumers.LoginAsync(loginModel.Username, loginModel.Password);

                if (userAccount != null && userAccount.IsActive)
                {
                    var token = GenerateMockToken();
                    var user = new UserInfo
                    {
                        UserId = userAccount.UserAccountId,
                        Username = userAccount.UserName,
                        FullName = userAccount.FullName ?? userAccount.UserName,
                        Role = userAccount.RoleId == 1 ? "Admin" : "User"
                    };

                    // Store token and user info in session storage
                    await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", TOKEN_KEY, token);
                    await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", USER_KEY, JsonSerializer.Serialize(user));

                    return new LoginResponse
                    {
                        Success = true,
                        Token = token,
                        Message = "Login successful",
                        User = user
                    };
                }

                return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = $"Login failed: {ex.Message}"
                };
            }
        }

        public async Task<string?> GetTokenAsync()
        {
            try
            {
                return await _jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", TOKEN_KEY);
            }
            catch
            {
                return null;
            }
        }

        public async Task<UserInfo?> GetUserInfoAsync()
        {
            try
            {
                var userJson = await _jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", USER_KEY);
                if (!string.IsNullOrEmpty(userJson))
                {
                    return JsonSerializer.Deserialize<UserInfo>(userJson);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }

        public async Task LogoutAsync()
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", TOKEN_KEY);
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", USER_KEY);
        }

        private string GenerateMockToken()
        {
            // Mock JWT token - in production, this would come from your API
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
