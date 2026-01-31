# مرحلة البناء
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# انسخ ملفات المشروع فقط أولاً لتسريع restore
COPY TouristP.csproj ./
RUN dotnet restore TouristP.csproj

# انسخ باقي الملفات
COPY . ./

# بناء المشروع
RUN dotnet publish TouristP.csproj -c Release -o out

# مرحلة التشغيل
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# لا تضع قيمة Connection String هنا
# Render سيضع المتغير في Environment Variables
ENTRYPOINT ["dotnet", "TouristP.dll"]
