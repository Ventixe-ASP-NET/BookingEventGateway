using BookingEventGateway.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<BookingBusinessLogic>();
builder.Services.AddHttpClient<BookingServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://bookingserviceventixe-fbb7amdzfsh4b4d6.swedencentral-01.azurewebsites.net/");
});

builder.Services.AddHttpClient<EventServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://ventixe-event-rest-api-cxeqehfrcqcvdkck.swedencentral-01.azurewebsites.net/");
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});
app.Run();
