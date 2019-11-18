using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace finance_management_backend.Models
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        private static bool _firstFill = true;
        public void Fill()
        {
            if (!_firstFill)
            {
                return;
            }
            _firstFill = false;


        }


    }

    internal static class Extensions
    {
        public static T GetAny<T>(this IEnumerable<T> elements)
        {
            return elements.ElementAt(new Random().Next(elements.Count()));
        }
    }
}