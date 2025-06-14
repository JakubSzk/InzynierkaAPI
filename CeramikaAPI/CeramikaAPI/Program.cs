var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string AllowAllOrigins = "AllowAllOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowAllOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*");
                      });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(AllowAllOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
