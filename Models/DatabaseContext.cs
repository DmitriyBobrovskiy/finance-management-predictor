using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace finance_management_backend.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Counterpart> Counterparts { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }

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
            // Fill database only first time - there is no need to fill it every user's request
            if (!_firstFill)
            {
                return;
            }
            _firstFill = false;

            Accounts.RemoveRange(Accounts);
            AccountTypes.RemoveRange(AccountTypes);
            Categories.RemoveRange(Categories);
            Counterparts.RemoveRange(Counterparts);
            Currencies.RemoveRange(Currencies);
            LoanTypes.RemoveRange(LoanTypes);
            Transactions.RemoveRange(Transactions);
            TransactionTypes.RemoveRange(TransactionTypes);
            SaveChanges();

            var random = new Random();
            var accountTypes = new List<AccountType>
            {
                new AccountType { Title= "Наличные" },
                new AccountType { Title= "Кредитная карта" },
                new AccountType { Title= "Дебитовая карта" },
            };
            AddRange(accountTypes);

            var currencies = new List<Currency>
            {
                new Currency { Title = "Рубли" },
                new Currency { Title = "Доллары" },
                new Currency { Title = "Евро" },
                new Currency { Title = "Юани" },
                new Currency { Title = "Гривны" },
            };
            AddRange(currencies);

            var accountTitles = new List<string>
            {
                "Тинькофф рубли",
                "Сбербанк",
                "Наличные доллары",
                "Юани оставшиеся с поездки",
            };

            var accounts = Enumerable.Range(1, random.Next(15)).Select(index =>
            {
                return new Account
                {
                    AccountType = accountTypes.GetAny(),
                    Currency = currencies.GetAny(),
                    Title = accountTitles.GetAny()
                };
            });
            AddRange(accounts);

            var categories = new List<Category>
            {
                new Category { Title = "Свободные" },
                new Category { Title = "Продукты" },
                new Category { Title = "Здоровье" },
                new Category { Title = "Дети" },
                new Category { Title = "Животные" },
                new Category { Title = "Автомобиль" },
            };
            AddRange(categories);

            var loanTypes = new List<LoanType>
            {
                new LoanType { Title = "Мне дали/Мне вернули" },
                new LoanType { Title = "Я дал/Я вернул" },
            };
            AddRange(loanTypes);

            var transactionTypes = new List<TransactionType>
            {
                new TransactionType { Title = "Расход" },
                new TransactionType { Title = "Приход" },
                new TransactionType { Title = "Перевод между счетами" },
                new TransactionType { Title = "Долг" },
            };
            AddRange(transactionTypes);

            var counterparts = new List<Counterpart>
            {
                new Counterpart { Title = "Пятерочка" },
                new Counterpart { Title = "Дядя Сережа" },
                new Counterpart { Title = "Петька" },
                new Counterpart { Title = "ООО Рога и копыта" },
                new Counterpart { Title = "ИП Мясников Николай Иванович" },
                new Counterpart { Title = "ИП Овчинников А.П." },
                new Counterpart { Title = "Сбербанк" },
                new Counterpart { Title = "Билайн" },
                new Counterpart { Title = "ОАО Кольцово" },
                new Counterpart { Title = "ПАО Энергосбыт" },
            };
            AddRange(counterparts);


            var transactions = Enumerable.Range(1, random.Next(1000)).Select(index =>
            {
                var transaction = new Transaction
                {
                    Account = accounts.GetAny(),
                    Amount = random.Next(10000),
                    Category = categories.GetAny(),
                    Counterpart = counterparts.GetAny(),
                    Date = DateTime.Now.AddMinutes(new Random().Next(-100000, 0)),
                    TransactionType = transactionTypes.GetAny(),
                };
                if (transaction.TransactionType.Title.Equals("Долг"))
                {
                    transaction.LoanType = loanTypes.GetAny();
                }

                return transaction;
            });
            AddRange(transactions);

            SaveChanges();
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