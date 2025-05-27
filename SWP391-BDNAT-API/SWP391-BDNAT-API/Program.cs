using BDNAT_Helper;
using BDNAT_Service.Implementation;
using BDNAT_Service.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IBlogsTypeService, BlogsTypeService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IKitOrderService, KitOrderService>();
builder.Services.AddScoped<IParameterService, ParameterService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IResultService, ResultService>();
builder.Services.AddScoped<ISampleService, SampleService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IServiceTypeService, ServiceTypeService>();
builder.Services.AddScoped<IShippingOrderService, ShippingOrderService>();
builder.Services.AddScoped<ITestKitService, TestKitService>();
builder.Services.AddScoped<ITestParameterService, TestParameterService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // Important for SignalR
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
