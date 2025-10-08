# 🚀 Byway Platform Deployment Guide

This guide will help you deploy the Byway online learning platform to production.

## 📋 Prerequisites

- GitHub account for frontend deployment
- SmarterASP.NET account for backend deployment
- Domain name (optional but recommended)
- SSL certificate (recommended)

## 🎯 Deployment Overview

- **Frontend**: Deploy to GitHub Pages, Netlify, or Vercel
- **Backend**: Deploy to SmarterASP.NET
- **Database**: SQL Server on SmarterASP.NET

## 🔧 Backend Deployment (SmarterASP.NET)

### Step 1: Prepare Backend for Production

1. **Update Connection String**
   ```json
   // appsettings.Production.json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=your-server;Database=BywayDB;User Id=your-username;Password=your-password;TrustServerCertificate=true;"
     }
   }
   ```

2. **Update JWT Secret**
   ```json
   {
     "JwtSettings": {
       "SecretKey": "your-production-secret-key-here-make-it-long-and-secure"
     }
   }
   ```

3. **Update CORS Settings**
   ```json
   {
     "Cors": {
       "AllowedOrigins": [
         "https://your-frontend-domain.com"
       ]
     }
   }
   ```

### Step 2: Build and Deploy

1. **Build the project**
   ```bash
   cd Byway.API
   dotnet build --configuration Release
   dotnet publish --configuration Release --output ./publish
   ```

2. **Upload to SmarterASP.NET**
   - Upload the contents of the `publish` folder
   - Ensure `web.config` is in the root directory
   - Create `wwwroot` folder with write permissions

### Step 3: Database Setup

1. **Create SQL Server Database**
   - Log into SmarterASP.NET control panel
   - Create a new SQL Server database
   - Note the connection details

2. **Run Database Migrations**
   - The application will create tables automatically on first run
   - Ensure the connection string is correct

## 🌐 Frontend Deployment (GitHub Pages/Netlify)

### Step 1: Prepare Frontend for Production

1. **Update API URL**
   ```env
   # .env.production
   REACT_APP_API_URL=https://your-api-domain.com
   ```

2. **Update Google OAuth Settings**
   ```env
   REACT_APP_GOOGLE_CLIENT_ID=your-production-google-client-id
   ```

### Step 2: Build and Deploy

#### Option A: GitHub Pages

1. **Create GitHub Repository**
   ```bash
   git init
   git add .
   git commit -m "Initial commit"
   git remote add origin https://github.com/yourusername/byway-frontend.git
   git push -u origin main
   ```

2. **Enable GitHub Pages**
   - Go to repository Settings
   - Scroll to Pages section
   - Select source branch (main)
   - Set folder to `/ (root)`

3. **Build and Deploy**
   ```bash
   npm run build
   # Upload build folder contents to GitHub Pages
   ```

#### Option B: Netlify

1. **Connect Repository**
   - Connect your GitHub repository to Netlify
   - Set build command: `npm run build`
   - Set publish directory: `build`

2. **Environment Variables**
   - Add environment variables in Netlify dashboard
   - `REACT_APP_API_URL`: Your API URL
   - `REACT_APP_GOOGLE_CLIENT_ID`: Your Google Client ID

#### Option C: Vercel

1. **Connect Repository**
   - Import your GitHub repository to Vercel
   - Set framework preset to React

2. **Environment Variables**
   - Add environment variables in Vercel dashboard

## 🔐 Security Configuration

### 1. SSL Certificates
- Enable SSL for both frontend and backend
- Update API URLs to use HTTPS

### 2. CORS Configuration
- Update CORS settings in backend
- Only allow your frontend domain

### 3. Environment Variables
- Never commit sensitive data to version control
- Use environment variables for all secrets

## 📁 File Structure for Deployment

### Backend (SmarterASP.NET)
```
/
├── Byway.API.dll
├── web.config
├── appsettings.Production.json
├── wwwroot/
│   ├── images/
│   │   ├── courses/
│   │   └── instructors/
└── (other DLL files)
```

### Frontend (GitHub Pages/Netlify)
```
/
├── index.html
├── static/
│   ├── css/
│   ├── js/
│   └── media/
└── (other build files)
```

## 🔧 Post-Deployment Configuration

### 1. Database Setup
- Ensure database tables are created
- Add initial data if needed
- Test database connections

### 2. File Upload Configuration
- Ensure `wwwroot/images/` folders exist
- Set proper permissions for file uploads

### 3. API Testing
- Test all API endpoints
- Verify authentication works
- Check file upload functionality

## 🚨 Troubleshooting

### Common Issues

1. **CORS Errors**
   - Update CORS settings in backend
   - Ensure frontend URL is in allowed origins

2. **Database Connection Issues**
   - Verify connection string
   - Check database server status

3. **File Upload Issues**
   - Check folder permissions
   - Verify file size limits

4. **Authentication Issues**
   - Verify JWT secret key
   - Check token expiration settings

## 📊 Monitoring and Maintenance

### 1. Logs
- Monitor application logs
- Set up error tracking

### 2. Performance
- Monitor API response times
- Optimize database queries

### 3. Security
- Regular security updates
- Monitor for vulnerabilities

## 🔄 Updates and Maintenance

### 1. Code Updates
- Use version control (Git)
- Test changes in staging environment
- Deploy to production

### 2. Database Updates
- Backup database before changes
- Test migrations in staging
- Apply changes to production

## 📞 Support

For deployment issues:
- Check SmarterASP.NET documentation
- Review GitHub Pages/Netlify documentation
- Contact hosting provider support

## ✅ Deployment Checklist

- [ ] Backend deployed to SmarterASP.NET
- [ ] Database created and configured
- [ ] Frontend deployed to hosting service
- [ ] SSL certificates configured
- [ ] CORS settings updated
- [ ] Environment variables set
- [ ] File upload functionality tested
- [ ] Authentication working
- [ ] All API endpoints tested
- [ ] Performance optimized
- [ ] Security measures in place

## 🎉 Success!

Your Byway platform should now be live and accessible to users!
