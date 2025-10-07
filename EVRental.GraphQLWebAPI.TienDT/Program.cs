using EVRental.GraphQLWebAPI.TienDT.GraphQLs;
using EVRental.Services.TienDT;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Add very permissive CORS for local development so the Blazor WASM client can call this API.
// NOTE: For production, tighten this to specific origins.
var allowAllCorsPolicy = "AllowAllCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowAllCorsPolicy, policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IServiceProviders, ServiceProviders>();
builder.Services.AddGraphQLServer()
    .AddQueryType<Queries>()
    .AddMutationType<Mutations>()
    .BindRuntimeType<DateTime, HotChocolate.Types.DateTimeType>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS before routing so it applies to the GraphQL endpoint and controllers.
app.UseCors(allowAllCorsPolicy);

app.UseAuthorization();

app.MapControllers();
app.UseRouting().UseEndpoints(endpoint => { endpoint.MapGraphQL(); });

app.Run();
