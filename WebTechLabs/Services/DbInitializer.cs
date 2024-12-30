using Microsoft.AspNetCore.Identity;
using WebTechLabs.DAL.Data;
using WebTechLabs.DAL.Entities;

namespace WebTechLabs.Services
{
    public static class DbInitializer
    {
        public static async Task Seed(this WebApplication app)
        {
            using var scoped = app.Services.CreateScope();
            using var context = scoped.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            using var userManager = scoped.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            using var roleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // создать БД, если она еще не создана
            context.Database.EnsureCreated();
            // проверка наличия ролей
            if (!context.Roles.Any())
            {
                var roleAdmin = new IdentityRole
                {
                    Name = "admin",
                    NormalizedName = "admin"
                };
                // создать роль admin
                await roleManager.CreateAsync(roleAdmin);
            }
            // проверка наличия пользователей
            if (!context.Users.Any())
            {
                // создать пользователя user@mail.ru
                var user = new ApplicationUser
                {
                    Email = "user@mail.ru",
                    UserName = "user@mail.ru"
                };
                await userManager.CreateAsync(user, "123456");
                // создать пользователя admin@mail.ru
                var admin = new ApplicationUser
                {
                    Email = "admin@mail.ru",
                    UserName = "admin@mail.ru"
                };
                await userManager.CreateAsync(admin, "123456");
                // назначить роль admin
                admin = await userManager.FindByEmailAsync("admin@mail.ru");
                await userManager.AddToRoleAsync(admin, "admin");
            }

            if (!context.DishGroups.Any())
            {
                context.DishGroups.AddRange(
                new List<DishGroup>
                {
                     new DishGroup { GroupName="Стартеры"},
                     new DishGroup {GroupName="Салаты"},
                     new DishGroup {GroupName="Супы"},
                     new DishGroup {GroupName="Основные блюда"},
                     new DishGroup {GroupName="Напитки"},
                     new DishGroup {GroupName="Десерты"}
                });
                await context.SaveChangesAsync();
            }

            if (!context.Dishes.Any())
            {
                context.Dishes.AddRange(
                new List<Dish>
                {
                    new Dish { DishName="Домашняя солянка", Description="Очень вкусная", Calories =200, DishGroupId=3, Image="soliyanka.jpg" },
                    new Dish { DishName="Уха", Description="Наваристая", Calories =330, DishGroupId=3, Image="uha.jpg" },
                    new Dish { DishName="Грибной крем-суп", Description="Домашний", Calories =330, DishGroupId=3, Image="gribnoui cream-soup.jpg" },
                    new Dish { DishName="Куриный суп", Description="", Calories =330, DishGroupId=3, Image="chiken soup.jpg" },
                    new Dish { DishName="Котлета папараць-кветка", Description="Хлеб - 80%, Морковь - 20%", Calories =635, DishGroupId=4, Image="папара.jpg" },
                    new Dish { DishName="Макароны по-флотски", Description="С тушенкой", Calories =524, DishGroupId=4, Image="makarons po-flotski.jpg" },
                    new Dish { DishName="Морс", Description="Из клюквы, 1 графин", Calories =180, DishGroupId=5, Image="mors.jpg" }
                });

                await context.SaveChangesAsync();
            }
        }
    }
}