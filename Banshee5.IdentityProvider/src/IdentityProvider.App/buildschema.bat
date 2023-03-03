rmdir /S /Q "Data/Migrations"
rmdir /S /Q "Data/CompiledModels"

dotnet ef migrations add Users -c ApplicationDbContext -o Data/Migrations
dotnet ef dbcontext optimize -c ApplicationDbContext -o Data/CompiledModels
