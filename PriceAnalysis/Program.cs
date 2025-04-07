using Microsoft.EntityFrameworkCore;
using ModuleProjects.Application;
using ModuleProjects.Infrastructure;
using PriceAnalysis.BLL.Services;
using PriceAnalysis.BLL.Services.Support;
using PriceAnalysis.BLL.Transfers;
using PriceAnalysis.DAL;
using PriceAnalysis.DAL.Repository;
using PriceAnalysis.DAL.Repository.FileStorage;
using PriceAnalysis.DAL.Repository.Support;
using PriceAnalysis.Transfers;
using Radzen;


var builder = WebApplication.CreateBuilder(args);
{
    //db
    builder.Services
        .AddDbContext<DBContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("AppDb")));

    //blazor
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    
    //mapping
    builder.Services
        .AddAutoMapper(typeof(BLLMappingProfile))
        .AddAutoMapper(typeof(AppMappingProfile));

    //radzen
    builder.Services
        .AddScoped<DialogService>()
        .AddScoped<NotificationService>()
        .AddScoped<TooltipService>();

    //DI
    builder.Services
        .AddTransient<ITestRepository, TestRepository>()
        .AddTransient<ITestService, TestService>()
        .AddTransient<ISectionRepository, SectionRepository>()
        .AddTransient<ISectionService, SectionService>()
        .AddScoped<IAlertService, AlertService>()
        .AddScoped<IAlertRepository, AlertRepository>()
        .AddScoped<ILoadDataFileService, LoadDataFileService>()
        .AddScoped<IFileStorageRepository, FileStorageRepository>()
        .AddScoped<IFileStorageService, FileStorageService>()
        .AddScoped<ISpecificationRepository, SpecificationRepository>()
        .AddScoped<ISpecificationService, SpecificationService>()
        .AddScoped<IDocFilesRepository, DocFilesRepository>()
        .AddScoped<IReferencesRepository, ReferencesRepository>()
        .AddScoped<ISupplierRepository, SupplierRepository>()
        .AddScoped<ISupplierService, SupplierService>()
        .AddScoped<IKPFilesRepository, KPFilesRepository>()
        .AddScoped<IKPFilesService, KPFilesService>()
        .AddScoped<ISynchPriceRepository, SynchPriceRepository>()
        .AddScoped<ISynchSpecificationRepository, SynchSpecificationRepository>()
        .AddScoped<ISynchSpecificationAndKPPrepRepository, SynchSpecificationAndKPPrepRepository>()
        .AddScoped<ISynchSpecificationAndKPService, SynchSpecificationAndKPService>()
        .AddScoped<IChapterForSpecificationRepository, ChapterForSpecificationRepository>();

    //модуль проекты
    builder.Services
        .AddInfrastructure(builder.Configuration)
        .AddApplication();
}


var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseRouting();

    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");
    app.Run();

    //Clear FileStorage temp dir
    (new FileStorageService()).ClearTempDir();
}