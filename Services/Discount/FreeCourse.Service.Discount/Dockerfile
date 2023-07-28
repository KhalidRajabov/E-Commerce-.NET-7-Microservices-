FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim-amd64 as build
WORKDIR /app
EXPOSE 80
COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Services/Discount/FreeCourse.Service.Discount/*.csproj Services/Discount/FreeCourse.Service.Discount/
RUN dotnet restore Services/Discount/FreeCourse.Service.Discount/*.csproj
COPY . .
RUN dotnet publish Services/Discount/FreeCourse.Service.Discount/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim-amd64 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","FreeCourse.Service.Discount.dll" ]