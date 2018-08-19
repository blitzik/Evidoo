using Caliburn.Micro;
using Perst;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        [NonSerialized()]
        private IReadOnlyCollection<Listing> _readOnlyCollection;
        public IReadOnlyCollection<Listing> Listings
        {
            get
            {
                if (_readOnlyCollection == null) {
                    _readOnlyCollection = new ReadOnlyCollection<Listing>(_listings);
                }
                return _readOnlyCollection;
            }
        }


        public Employer() { }

        public Employer(Storage db, string name)
        {
            Name = name;
            CreatedAt = DateTime.Now;

            _listings = db.CreateScalableList<Listing>();
        }


        public void AddListing(Listing listing)
        {
            _listings.Add(listing);
        }


        public void RemoveListing(Listing listing)
        {
            _listings.Remove(listing);
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
