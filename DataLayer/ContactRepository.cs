﻿using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataLayer
{
    public class ContactRepository : IContactRepository
    {
        private IDbConnection db;

        public ContactRepository(string connString)
        {
            this.db = new SqlConnection(connString);
        }


        public Contact Add(Contact contact)
        {
            var sql =
              "INSERT INTO Contacts (FirstName, LastName, Email, Company, Title) VALUES(@FirstName, @LastName, @Email, @Company, @Title); " +
              "SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = this.db.Query<int>(sql, contact).Single();
            contact.Id = id;
            return contact;
        }

        public Contact Find(int id)
        {
            return this.db.Query<Contact>("SELECT * FROM Contacts WHERE Id = @Id", new { id }).SingleOrDefault();
        }

        public List<Contact> GetAll()
        {
            return this.db.Query<Contact>("SELECT * FROM Contacts").ToList();
            //throw new System.NotImplementedException();
        }

        public Contact GetFullContact(int id)
        {
            var sql =
                "SELECT * FROM Contacts WHERE Id = @Id; " +
                "SELECT * FROM Addresses WHERE ContactId = @Id;";

            using (var multipleResults = db.QueryMultiple(sql, new {Id = id }))
            {
                var contact = multipleResults.Read<Contact>().SingleOrDefault();

                var addresses = multipleResults.Read<Address>().ToList();

                if (contact != null && addresses != null)
                {
                    contact.Addresses.AddRange(addresses);
                }

                return contact;
            }         
        }

        public void Remove(int id)
        {
            this.db.Execute("DELETE FROM Contacts WHERE Id = @Id", new { id });
        }

        public void Save(Contact contact)
        {
            throw new System.NotImplementedException();
        }

        public Contact Update(Contact contact)
        {
            var sql =
               "UPDATE Contacts " +
               "SET FirstName = @FirstName, " +
               "    LastName  = @LastName, " +
               "    Email     = @Email, " +
               "    Company   = @Company, " +
               "    Title     = @Title " +
               "WHERE Id = @Id";
            this.db.Execute(sql, contact);
            return contact;
        }
    }
}
