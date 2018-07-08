using Caliburn.Micro;
using Perst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Domain
{
    public class Employer : PropertyChangedBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }


        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get { return _createdAt; }
            private set { _createdAt = value; }
        }


        private IPersistentList<Listing> _listings;
        private IPersistentList<Listing> Listings
        {
            get { return _listings; }
            set { _listings = value; }
        }


        public Employer() { }

        public Employer(Storage db, string name)
        {
            Name = name;
            CreatedAt = DateTime.Now;

            Listings = db.CreateScalableList<Listing>();
        }


        public void AddListing(Listing listing)
        {
            Listings.Add(listing);
        }


        public void RemoveListing(Listing listing)
        {
            Listings.Remove(listing);
        }


        public List<Listing> GetListings()
        {
            return new List<Listing>(Listings);
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
