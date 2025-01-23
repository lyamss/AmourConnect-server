using API.Entities;
using API.Persistence;

namespace API.Seeders
{
    internal sealed class UserSeeder(BackendDbContext context) : IUserSeeder
    {
        public async Task Seed()
        {
            if (await context.Database.CanConnectAsync())
            {
                if (!context.User.Any())
                {
                    Random random = new();
                    for (int i = 0; i < 300; i++)
                    {
                        User newUser = new()
                        {
                            Pseudo = this._GenerateRandomName(),
                            Description = this._GenerateRandomName() + this._GenerateRandomPassword(),
                            EmailGoogle = this._GenerateRandomEmail(),
                            userIdGoogle = this._GenerateRandomPassword(),
                            city = this._GenerateRandomCity(),
                            sex = this._GenerateRandomGender(),
                            date_of_birth = DateTime.UtcNow.AddYears(-random.Next(18, 65)),
                            account_created_at = DateTime.UtcNow
                        };
                        context.User.Add(newUser);
                    }
                    await context.SaveChangesAsync();
                }
            }
        }
        private string _GenerateRandomName()
        {
            string[] names = { "John", "Jane", "Bob", "Alice", "Charlie", "Emma", "Oliver", "Sophia", "William", "Ava" };
            Random rand = new();
            return names[rand.Next(names.Length)];
        }

        private string _GenerateRandomEmail()
        {
            string[] domains = { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com" };
            Random rand = new();
            return $"{this._GenerateRandomName().ToLower()}@{domains[rand.Next(domains.Length)]}";
        }

        private string _GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random rand = new();
            return new string(Enumerable.Repeat(chars, 8).Select(s => s[rand.Next(s.Length)]).ToArray());
        }

        private string _GenerateRandomCity()
        {
            string[] cities = { "Paris", "Lyon", "Marseille", "Toulouse", "Nice", "Nantes", "Strasbourg", "Montpellier", "Bordeaux", "Lille" };
            Random rand = new();
            return cities[rand.Next(cities.Length)];
        }

        private string _GenerateRandomGender()
        {
            string[] genders = { "M", "F" };
            Random rand = new();
            return genders[rand.Next(genders.Length)];
        }
    }
}