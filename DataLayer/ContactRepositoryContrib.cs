using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using Dapper.Contrib.Extensions;

namespace DataLayer
{
    public class ContactRepositoryContrib : IContactRepository
    {
        private IDbConnection db;
        public ContactRepositoryContrib(string connString)
        {
            this.db = new SqlConnection(connString);
        }



        public Contact Add(Contact contact)
        {
            var id = db.Insert(contact);
            contact.Id = (int)id;
            return contact;


        }

        public Contact Find(int id)
        {
            return db.Get<Contact>(id);
        }

        public List<Contact> GetAll()
        {
            return db.GetAll<Contact>().ToList();
        }

        public Contact GetFullContact(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            db.Delete<Contact>(new Contact { Id = id });
        }

        public void Save(Contact contact)
        {
            throw new NotImplementedException();
        }

        public Contact Update(Contact contact)
        {
            db.Update(contact);
            return contact;
        }
    }
}
