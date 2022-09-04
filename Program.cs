using Blog.Data;
using Blog.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().ConfigureApiBehaviorOptions(opt => 
{
    opt.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddDbContext<BlogDataContext>();
builder.Services.AddTransient<TokenService>();

var app = builder.Build();

app.MapControllers();

app.Run();
