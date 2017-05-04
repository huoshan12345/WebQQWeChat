using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Application.Models;
using Domain.Entities.Base;
using FclEx.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=WebQQWeChat.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityType = typeof(IEntity<>);
            var entityTypes = entityType.GetTypeInfo().Assembly.GetTypes()
                .Where(m => m.IsInheritedFromGenericType(entityType) && !m.GetTypeInfo().IsGenericType && !m.GetTypeInfo().IsAbstract).ToArray();

            entityTypes.ForEach(m => modelBuilder.Entity(m));
            base.OnModelCreating(modelBuilder);
        }

        private static void SetRowVersion(object entity)
        {
            //if (!(entity is IRowVersion v)) return;
            //v.RowVersion = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        SetRowVersion(entry.Entity);
                        break;
                    case EntityState.Modified:
                        SetRowVersion(entry.Entity);
                        break;
                    case EntityState.Deleted:
                        break;
                }
            }
            return base.SaveChanges();
        }
    }
}
