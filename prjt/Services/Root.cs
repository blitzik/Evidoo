using prjt.Domain;
using Perst;
using System;

namespace prjt.Services
{
    public class Root : Perst.Persistent
    {
        private CompoundIndex<Listing> _listings;
        public CompoundIndex<Listing> Listings
        {
            get { return _listings; }
        }


        private FieldIndex<string, DefaultSettings> _defaultSettings;
        public FieldIndex<string, DefaultSettings> DefaultSettings
        {
            get { return _defaultSettings; }
        }


        private PArray<Employer> _employers;
        public PArray<Employer> Employers
        {
            get { return _employers; }
        }


        public Root(Storage db)
        {
            _listings = db.CreateIndex<Listing>(new Type[] { typeof(int), typeof(int) }, false);
            _employers = db.CreateArray<Employer>();
            _defaultSettings = db.CreateFieldIndex<string, DefaultSettings>("_id", true);
        }
    }
}
