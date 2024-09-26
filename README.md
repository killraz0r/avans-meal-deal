# migrations
## application
### add migration
dotnet ef --project .\AvansMealDeal.Infrastructure.Application.SQLServer\AvansMealDeal.Infrastructure.Application.SQLServer.csproj --startup-project .\AvansMealDeal.UserInterface.WebService\AvansMealDeal.UserInterface.WebService.csproj migrations add NAME --context DbContextApplicationSqlServer
### update database
dotnet ef --project .\AvansMealDeal.Infrastructure.Application.SQLServer\AvansMealDeal.Infrastructure.Application.SQLServer.csproj --startup-project .\AvansMealDeal.UserInterface.WebService\AvansMealDeal.UserInterface.WebService.csproj database update --context DbContextApplicationSqlServer
## identity
### add migration
dotnet ef --project .\AvansMealDeal.Infrastructure.Identity.SQLServer\AvansMealDeal.Infrastructure.Identity.SQLServer.csproj --startup-project .\AvansMealDeal.UserInterface.WebService\AvansMealDeal.UserInterface.WebService.csproj migrations add NAME --context DbContextIdentitySqlServer
### update database
dotnet ef --project .\AvansMealDeal.Infrastructure.Identity.SQLServer\AvansMealDeal.Infrastructure.Identity.SQLServer.csproj --startup-project .\AvansMealDeal.UserInterface.WebService\AvansMealDeal.UserInterface.WebService.csproj database update --context DbContextIdentitySqlServer