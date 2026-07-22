using LocumInvoiceTracker.Components;
using LocumInvoiceTracker.Data;
using LocumInvoiceTracker.Services;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

    builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "DefaultConnection was not configured.");

    options.UseSqlite(connectionString);
});

builder.Services.AddScoped<ExcelExportService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

    app.MapGet(
    "/api/reports/shifts.xlsx",
    async (
        int hospitalId,
        DateTime? from,
        DateTime? to,
        IDbContextFactory<AppDbContext> dbFactory,
        ExcelExportService excelExport) =>
    {
        await using var db =
            await dbFactory.CreateDbContextAsync();

        var hospital = await db.Hospitals
            .AsNoTracking()
            .SingleOrDefaultAsync(
                hospital => hospital.Id == hospitalId);

        if (hospital is null)
        {
            return Results.NotFound("Hospital not found.");
        }

        var query = db.WorkShifts
            .AsNoTracking()
            .Include(shift => shift.Hospital)
            .Where(shift => shift.HospitalId == hospitalId);

        if (from.HasValue)
        {
            query = query.Where(
                shift => shift.Date >= from.Value.Date);
        }

        if (to.HasValue)
        {
            query = query.Where(
                shift => shift.Date <= to.Value.Date);
        }

        var shifts = await query
            .OrderBy(shift => shift.Date)
            .ToListAsync();

        var fileBytes = excelExport.CreateShiftReport(
            hospital.Name,
            from,
            to,
            shifts);

        var safeHospitalName = string.Concat(
            hospital.Name.Where(char.IsLetterOrDigit));

        var fileName =
            $"{safeHospitalName}-Shift-Report.xlsx";

        return Results.File(
            fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    });

app.Run();
