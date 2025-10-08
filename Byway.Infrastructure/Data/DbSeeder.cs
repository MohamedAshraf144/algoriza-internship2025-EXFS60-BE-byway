using Microsoft.EntityFrameworkCore;
using Byway.Domain.Entities;
using Byway.Domain.Enums;
using BCrypt.Net;

namespace Byway.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Seed Admin User
            if (!await context.Users.AnyAsync(u => u.Role == UserRole.Admin))
            {
                var adminUser = new User
                {
                    Email = "admin@byway.com",
                    FirstName = "Admin",
                    LastName = "User",
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.UtcNow
                };

                // Hash password
                adminUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!");

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();
            }

            // Seed Categories
            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Web Development",
                        ImagePath = "https://images.unsplash.com/photo-1461749280684-dccba630e2f6?w=500",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        Name = "Mobile Development",
                        ImagePath = "https://images.unsplash.com/photo-1512941937669-90a1b58e7e9c?w=500",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        Name = "Data Science",
                        ImagePath = "https://images.unsplash.com/photo-1551288049-bebda4e38f71?w=500",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        Name = "UI/UX Design",
                        ImagePath = "https://images.unsplash.com/photo-1558655146-d09347e92766?w=500",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        Name = "DevOps",
                        ImagePath = "https://images.unsplash.com/photo-1667372393120-327ac5cc5468?w=500",
                        CreatedAt = DateTime.UtcNow
                    }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // Seed Instructors
            if (!await context.Instructors.AnyAsync())
            {
                var instructors = new List<Instructor>
                {
                    new Instructor
                    {
                        Name = "أحمد محمد",
                        Bio = "مطور ويب محترف مع أكثر من 8 سنوات من الخبرة في تطوير التطبيقات الحديثة باستخدام React و Node.js",
                        JobTitle = JobTitle.FullstackDeveloper,
                        ImagePath = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=500",
                        Rating = 4.8m,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Instructor
                    {
                        Name = "فاطمة علي",
                        Bio = "مصممة UI/UX متخصصة في إنشاء تجارب مستخدم متميزة وتصميم واجهات تفاعلية",
                        JobTitle = JobTitle.UXUIDesigner,
                        ImagePath = "https://images.unsplash.com/photo-1494790108755-2616b612b786?w=500",
                        Rating = 4.9m,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Instructor
                    {
                        Name = "محمد حسن",
                        Bio = "مطور Backend متخصص في ASP.NET Core و Entity Framework مع خبرة في قواعد البيانات",
                        JobTitle = JobTitle.BackendDeveloper,
                        ImagePath = "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=500",
                        Rating = 4.7m,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Instructor
                    {
                        Name = "نور الدين",
                        Bio = "مطور Frontend متخصص في React و TypeScript مع خبرة في تطوير تطبيقات SPA",
                        JobTitle = JobTitle.FrontendDeveloper,
                        ImagePath = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=500",
                        Rating = 4.6m,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                await context.Instructors.AddRangeAsync(instructors);
                await context.SaveChangesAsync();
            }

            // Seed Courses
            if (!await context.Courses.AnyAsync())
            {
                var categories = await context.Categories.ToListAsync();
                var instructors = await context.Instructors.ToListAsync();

                var courses = new List<Course>
                {
                    new Course
                    {
                        Title = "تعلم React من الصفر إلى الاحتراف",
                        Description = "دورة شاملة لتعلم React.js من الأساسيات إلى المستوى المتقدم، تشمل Hooks و Context API و Redux",
                        Price = 299.99m,
                        Rating = 4.8m,
                        Level = Level.Beginner,
                        Duration = 40, // hours
                        Requirements = "معرفة أساسية بـ JavaScript و HTML/CSS",
                        LearningOutcomes = "بناء تطبيقات React كاملة، استخدام Hooks، إدارة الحالة، ربط APIs",
                        ImagePath = "https://images.unsplash.com/photo-1633356122544-f134324a6cee?w=500",
                        CategoryId = categories.First(c => c.Name == "Web Development").Id,
                        InstructorId = instructors.First(i => i.Name == "نور الدين").Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Course
                    {
                        Title = "ASP.NET Core Web API",
                        Description = "تعلم بناء APIs قوية وآمنة باستخدام ASP.NET Core و Entity Framework",
                        Price = 399.99m,
                        Rating = 4.7m,
                        Level = Level.Intermediate,
                        Duration = 35,
                        Requirements = "معرفة بـ C# و أساسيات البرمجة",
                        LearningOutcomes = "بناء RESTful APIs، استخدام Entity Framework، تطبيق الأمان، اختبار APIs",
                        ImagePath = "https://images.unsplash.com/photo-1516321318423-f06f85e504b3?w=500",
                        CategoryId = categories.First(c => c.Name == "Web Development").Id,
                        InstructorId = instructors.First(i => i.Name == "محمد حسن").Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Course
                    {
                        Title = "تصميم UI/UX للمبتدئين",
                        Description = "دورة شاملة لتعلم أساسيات التصميم وواجهات المستخدم باستخدام Figma",
                        Price = 199.99m,
                        Rating = 4.9m,
                        Level = Level.Beginner,
                        Duration = 25,
                        Requirements = "لا توجد متطلبات مسبقة",
                        LearningOutcomes = "مبادئ التصميم، استخدام Figma، إنشاء Prototypes، تصميم Responsive",
                        ImagePath = "https://images.unsplash.com/photo-1561070791-2526d30994b5?w=500",
                        CategoryId = categories.First(c => c.Name == "UI/UX Design").Id,
                        InstructorId = instructors.First(i => i.Name == "فاطمة علي").Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Course
                    {
                        Title = "Full Stack Development",
                        Description = "دورة متكاملة لتطوير تطبيقات ويب كاملة من Frontend إلى Backend",
                        Price = 599.99m,
                        Rating = 4.8m,
                        Level = Level.Expert,
                        Duration = 60,
                        Requirements = "معرفة بـ JavaScript و أساسيات البرمجة",
                        LearningOutcomes = "تطوير تطبيقات كاملة، استخدام React و Node.js، قواعد البيانات، النشر",
                        ImagePath = "https://images.unsplash.com/photo-1555066931-4365d14bab8c?w=500",
                        CategoryId = categories.First(c => c.Name == "Web Development").Id,
                        InstructorId = instructors.First(i => i.Name == "أحمد محمد").Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Course
                    {
                        Title = "React Native للمبتدئين",
                        Description = "تعلم تطوير تطبيقات الهاتف المحمول باستخدام React Native",
                        Price = 349.99m,
                        Rating = 4.6m,
                        Level = Level.Beginner,
                        Duration = 30,
                        Requirements = "معرفة أساسية بـ React",
                        LearningOutcomes = "تطوير تطبيقات iOS و Android، استخدام Native APIs، النشر على المتاجر",
                        ImagePath = "https://images.unsplash.com/photo-1512941937669-90a1b58e7e9c?w=500",
                        CategoryId = categories.First(c => c.Name == "Mobile Development").Id,
                        InstructorId = instructors.First(i => i.Name == "نور الدين").Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Course
                    {
                        Title = "Data Science مع Python",
                        Description = "تعلم تحليل البيانات والتعلم الآلي باستخدام Python و Pandas",
                        Price = 449.99m,
                        Rating = 4.7m,
                        Level = Level.Intermediate,
                        Duration = 45,
                        Requirements = "معرفة أساسية بـ Python",
                        LearningOutcomes = "تحليل البيانات، Machine Learning، Visualization، Statistical Analysis",
                        ImagePath = "https://images.unsplash.com/photo-1551288049-bebda4e38f71?w=500",
                        CategoryId = categories.First(c => c.Name == "Data Science").Id,
                        InstructorId = instructors.First(i => i.Name == "أحمد محمد").Id,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                await context.Courses.AddRangeAsync(courses);
                await context.SaveChangesAsync();
            }

            // Seed Cart Items for existing users
            var users = await context.Users.Where(u => u.Role == UserRole.Student).ToListAsync();
            var availableCourses = await context.Courses.Take(3).ToListAsync();

            foreach (var user in users)
            {
                // Create cart if doesn't exist
                var cart = await context.Carts.FirstOrDefaultAsync(c => c.UserId == user.Id);
                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = user.Id,
                        CreatedAt = DateTime.UtcNow
                    };
                    await context.Carts.AddAsync(cart);
                    await context.SaveChangesAsync();
                }

                // Add some courses to cart
                var existingCartItems = await context.CartItems
                    .Where(ci => ci.CartId == cart.Id)
                    .ToListAsync();

                if (!existingCartItems.Any())
                {
                    var cartItems = new List<CartItem>
                    {
                        new CartItem
                        {
                            CartId = cart.Id,
                            CourseId = availableCourses[0].Id,
                            CreatedAt = DateTime.UtcNow
                        },
                        new CartItem
                        {
                            CartId = cart.Id,
                            CourseId = availableCourses[1].Id,
                            CreatedAt = DateTime.UtcNow
                        }
                    };

                    await context.CartItems.AddRangeAsync(cartItems);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
