﻿using RealtorMVC.Seeding.Behaviours;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealtorMVC.Seeding.Interfaces;

namespace RealtorMVC.Seeding.Extentions;

public static class SeedingExtentions
{
    public static IServiceCollection AddSeeding(this IServiceCollection services)
    {
        services.AddScoped<ISeedingBehaviour, RoleSeedingBehaviour>();

        return services;
    }

    public static async Task ApplySeedingAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;

        var behaviours = services.GetRequiredService<IEnumerable<ISeedingBehaviour>>();

        foreach (var behaviour in behaviours)
        {
            await behaviour.SeedAsync();
        }
    }
}
