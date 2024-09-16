using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace JerseyShop.Data
{
    public class SeedData
    {
        public static async Task CreateDefaultAdmin(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Verifica se il ruolo Admin esiste
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
                if (roleResult.Succeeded)
                {
                    Console.WriteLine("Ruolo Admin creato con successo.");
                }
                else
                {
                    Console.WriteLine("Errore durante la creazione del ruolo Admin.");
                }
            }

            // Crea l'utente Admin di default se non esiste
            var adminEmail = "admin@jerseyshop.com";
            var adminPassword = "Admin@123";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                var result = await userManager.CreateAsync(newAdmin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                    Console.WriteLine("Utente Admin creato con successo.");
                }
                else
                {
                    Console.WriteLine("Errore durante la creazione dell'utente Admin.");
                }
            }
            else
            {
                Console.WriteLine("Utente Admin già esistente.");
            }
        }
    }
}