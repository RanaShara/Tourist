# مرحلة البناء
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# انسخ ملفات المشروع
COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish TouristP.csproj -c Release -o out

# مرحلة التشغيل
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "TouristP.dll"]
