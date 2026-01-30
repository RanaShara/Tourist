# ================================
# 1️⃣ مرحلة البناء
# ================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# انسخ كل ملفات المشروع
COPY . ./

# استرجاع كل الحزم
RUN dotnet restore

# بناء المشروع
RUN dotnet publish -c Release -o out

# ================================
# 2️⃣ مرحلة التشغيل
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# نسخ الملفات المبنية من مرحلة البناء
COPY --from=build /app/out .

# إعداد متغير البيئة الافتراضي للـ ConnectionString
# لاحقًا على Render سنضع Environment Variable حقيقي
#ENV ConnectionStrings__DefaultConnection=""

# أمر تشغيل المشروع
ENTRYPOINT ["dotnet", "TouristP.dll"]
