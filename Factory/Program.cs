//     Copyright (C) 2024  Henry Oberholtzer
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.

using Microsoft.EntityFrameworkCore;
using Factory.Utilities;
using Factory.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(option =>
{
    option.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
});

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<FactoryContext>(
    dbContextOptions => dbContextOptions.UseMySql(
        builder.Configuration["ConnectionStrings:DefaultConnection"],
        ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"]
        )
    )
);

WebApplication app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "AdminAreaRoute",
        areaName: "Admin",
        pattern: "admin/{controller:slugify=Dashboard}/{action:slugify=Index}/{id:slugify?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller:slugify}/{action:slugify}/{id:slugify?}",
        defaults: new { controller = "Home", action = "Index" });
});

app.Run();

