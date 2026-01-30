# مرحلة البناء
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# مرحلة التشغيل
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# لا تضع قيمة Connection String هنا
# Render سيضع المتغير في Environment Variables
ENTRYPOINT ["dotnet", "TouristP.dll"]
