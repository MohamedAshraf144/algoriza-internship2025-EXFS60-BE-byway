# 🚀 SmarterASP.NET Deployment Guide

## 📋 **خطوات النشر على SmarterASP.NET**

### **1. إعداد قاعدة البيانات:**
```sql
-- استخدم Connection String التالي:
Data Source=SQL5106.site4now.net;Initial Catalog=db_abf20a_mohamedexfs60;User Id=db_abf20a_mohamedexfs60_admin;Password=YOUR_DB_PASSWORD;TrustServerCertificate=true;
```

### **2. تحديث كلمة المرور:**
- استبدل `YOUR_DB_PASSWORD` بكلمة المرور الفعلية
- في ملف `appsettings.Production.json`
- في ملف `appsettings.SmarterASP.json`

### **3. إعدادات البيئة:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=SQL5106.site4now.net;Initial Catalog=db_abf20a_mohamedexfs60;User Id=db_abf20a_mohamedexfs60_admin;Password=YOUR_ACTUAL_PASSWORD;TrustServerCertificate=true;"
  }
}
```

### **4. ملفات النشر المطلوبة:**
- ✅ `appsettings.Production.json` - للإعدادات الإنتاجية
- ✅ `appsettings.SmarterASP.json` - لإعدادات SmarterASP
- ✅ `web.config` - لإعدادات IIS
- ✅ `build.bat` - لبناء المشروع

### **5. خطوات النشر:**

#### **أ) بناء المشروع:**
```bash
# تشغيل ملف البناء
.\build.bat
```

#### **ب) رفع الملفات:**
1. ارفع محتويات مجلد `bin/Release/net8.0/publish/`
2. ارفع ملف `web.config`
3. ارفع ملف `appsettings.Production.json`

#### **ج) إعدادات SmarterASP:**
1. في لوحة التحكم، اختر "Application Settings"
2. اضبط Environment Variable:
   - `ASPNETCORE_ENVIRONMENT` = `Production`
3. تأكد من أن Connection String صحيح

### **6. اختبار النشر:**
```bash
# اختبار API
curl https://your-domain.smarterasp.net/api/courses
```

## ⚠️ **ملاحظات مهمة:**

### **أمان:**
- ✅ لا ترفع ملفات `appsettings.Development.json`
- ✅ استخدم كلمات مرور قوية
- ✅ تأكد من إعدادات CORS للفرونت إند

### **أداء:**
- ✅ استخدم `appsettings.Production.json` للإنتاج
- ✅ تأكد من إعدادات Logging
- ✅ راجع إعدادات JWT

## 🔧 **استكشاف الأخطاء:**

### **مشاكل شائعة:**
1. **Connection String خاطئ** → راجع كلمة المرور
2. **CORS Error** → أضف domain الفرونت إند
3. **JWT Error** → تأكد من Secret Key

### **ملفات المساعدة:**
- `DEPLOYMENT-GUIDE.md` - دليل شامل
- `web.config` - إعدادات IIS
- `build.bat` - بناء المشروع

## 📞 **الدعم:**
إذا واجهت مشاكل، راجع:
1. SmarterASP.NET Documentation
2. ASP.NET Core Deployment Guide
3. ملف `DEPLOYMENT-GUIDE.md`
