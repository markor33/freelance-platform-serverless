using ReadModelStore.Seed;

try
{
    await LanguageSeed.SeedAsync();
    Console.WriteLine("Seeding completed successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Seeding failed: {ex.Message}");
}
