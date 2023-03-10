if (Test-Path Data/Migrations) {
    Remove-Item -Recurse -Force Data/Migrations
}

if (Test-Path Data/CompiledModels) {
    Remove-Item -Recurse -Force Data/CompiledModels
}

dotnet ef migrations add Users -c ApplicationDbContext -o Data/Migrations
dotnet ef dbcontext optimize -c ApplicationDbContext -o Data/CompiledModels
dotnet ef database update
