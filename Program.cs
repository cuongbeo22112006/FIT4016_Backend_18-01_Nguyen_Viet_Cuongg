using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlite("Data Source=SchoolManagement.db"));

var app = builder.Build();

// Ensure DB created and seed sample data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
    db.Database.EnsureCreated();
    await EnsureSeedDataAsync(db);
}

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Students}/{action=Index}/{id?}");

app.Run();

// Local helper to seed schools (>=10) and students (>=20)
static async Task EnsureSeedDataAsync(SchoolDbContext db)
{
    if (await db.Schools.CountAsync() >= 10 && await db.Students.CountAsync() >= 20)
        return;

    if (!await db.Schools.AnyAsync())
    {
        var schools = new[]
        {
            new School { Name="Greenwood High", Principal="Alice Johnson", Address="12 Oak Street", CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow },
            new School { Name="Riverside Academy", Principal="Robert Smith", Address="45 River Road", CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow },
            new School { Name="Hillcrest School", Principal="Maria Garcia", Address="99 Hill Ave", CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow },
            new School { Name="Sunnydale College", Principal="David Brown", Address="101 Sunny St", CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow },
            new School { Name="Maple Leaf Institute", Principal="Linda Davis", Address="23 Maple Rd", CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow },
            new School { Name="Lakeside High", Principal="James Wilson", Address="8 Lake View", CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow },
            new School { Name="Oakridge Secondary", Principal="Patricia Martinez", Address="77 Oakridge Blvd", CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow },
            new School { Name="Cedar Grove School", Principal="Michael Anderson", Address="4 Cedar Grove", CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow },
            new School { Name="Pinecrest Academy", Principal="Barbara Thomas", Address="55 Pinecrest Ln", CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow },
            new School { Name="Westview High", Principal="William Jackson", Address="200 Westview Dr", CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow },
        };
        db.Schools.AddRange(schools);
        await db.SaveChangesAsync();
    }

    if (await db.Students.CountAsync() < 20)
    {
        var schoolsIds = await db.Schools.Select(s => s.Id).ToArrayAsync();
        var sampleStudents = new List<Student>
        {
            new Student{FullName="Oliver Brown", StudentId="S10001", Email="oliver.brown@example.com", Phone="0123456789", SchoolId=schoolsIds[0], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="Emma Wilson", StudentId="S10002", Email="emma.wilson@example.com", Phone="0123456790", SchoolId=schoolsIds[0], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="Liam Johnson", StudentId="S10003", Email="liam.johnson@example.com", Phone="0987654321", SchoolId=schoolsIds[1], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="Ava Martinez", StudentId="S10004", Email="ava.martinez@example.com", Phone="0987654320", SchoolId=schoolsIds[1], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="Noah Garcia", StudentId="S10005", Email="noah.garcia@example.com", Phone="0112233445", SchoolId=schoolsIds[2], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="Isabella Rodriguez", StudentId="S10006", Email="isabella.rodriguez@example.com", Phone="0112233446", SchoolId=schoolsIds[2], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="William Lee", StudentId="S10007", Email="william.lee@example.com", Phone="0223344556", SchoolId=schoolsIds[3], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="Sophia Walker", StudentId="S10008", Email="sophia.walker@example.com", Phone="0223344557", SchoolId=schoolsIds[3], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="James Hall", StudentId="S10009", Email="james.hall@example.com", Phone="0334455667", SchoolId=schoolsIds[4], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="Mia Allen", StudentId="S10010", Email="mia.allen@example.com", Phone="0334455668", SchoolId=schoolsIds[4], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="Benjamin Young", StudentId="S10011", Email="benjamin.young@example.com", Phone="0445566778", SchoolId=schoolsIds[5], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="Charlotte Hernandez", StudentId="S10012", Email="charlotte.hernandez@example.com", Phone="0445566779", SchoolId=schoolsIds[5], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="Elijah King", StudentId="S10013", Email="elijah.king@example.com", Phone="0556677889", SchoolId=schoolsIds[6], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="Amelia Wright", StudentId="S10014", Email="amelia.wright@example.com", Phone="0556677880", SchoolId=schoolsIds[6], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="Lucas Lopez", StudentId="S10015", Email="lucas.lopez@example.com", Phone="0667788990", SchoolId=schoolsIds[7], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="Harper Hill", StudentId="S10016", Email="harper.hill@example.com", Phone="0667788991", SchoolId=schoolsIds[7], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="Henry Scott", StudentId="S10017", Email="henry.scott@example.com", Phone="0778899001", SchoolId=schoolsIds[8], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="Evelyn Green", StudentId="S10018", Email="evelyn.green@example.com", Phone="0778899002", SchoolId=schoolsIds[8], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="Alexander Adams", StudentId="S10019", Email="alexander.adams@example.com", Phone="0889900112", SchoolId=schoolsIds[9], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
            new Student{FullName="Abigail Baker", StudentId="S10020", Email="abigail.baker@example.com", Phone="0889900113", SchoolId=schoolsIds[9], CreatedAt=DateTime.UtcNow, UpdatedAt=DateTime.UtcNow},
        };

        foreach (var s in sampleStudents)
        {
            if (!await db.Students.AnyAsync(x => x.StudentId == s.StudentId || x.Email == s.Email))
            {
                db.Students.Add(s);
            }
        }
        await db.SaveChangesAsync();
    }
}