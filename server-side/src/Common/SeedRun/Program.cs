using ReadModelStore.Seed;

try
{
    await SkillSeed.SeedAsync();
    Console.WriteLine("Seeding completed successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Seeding failed: {ex.Message}");
}
