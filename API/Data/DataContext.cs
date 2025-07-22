using System;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
   public DbSet<AppUser> Users { get; set; } //DbSet je predstavnik tabele u bazi, tabela se zove user, i svaki red je objekat tipa AppUser
}
