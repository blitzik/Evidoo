using prjt.Domain;
using prjt.Services;
using Perst;
using System;
using System.Collections.Generic;
using System.Linq;

namespace prjt.Facades
{
    public class ListingFacade : BaseFacade
    {
        public ListingFacade(StoragePool db) : base (db)
        {
        }


        public void StoreListing(Listing listing)
        {
            Storage().Store(listing);
            Root().Listings.Put(new Key(new Object[] { listing.Year, listing.Month }), listing);
            Storage().Commit();
        }


        public void Update(Listing listing)
        {
            // index does not have to be updated because the Year and Month of the Listing is immutable
            Storage().Modify(listing);
            Storage().Commit();
        }


        public List<Listing> FindAllListings()
        {
            IEnumerable<Listing> listings = from Listing l in Root().Listings select l;

            return new List<Listing>(listings);
        }


        public List<Listing> FindListings(int year, int month)
        {
            IEnumerable<Listing> listings;
            if (month < 1 || month > 12) {
                listings = from Listing l in Root().Listings where l.Year == year orderby l.Month descending select l;
            } else {
                listings = from Listing l in Root().Listings where l.Year == year && l.Month == month orderby l.Month descending select l;
            }

            return new List<Listing>(listings);
        }


        public void DeleteListing(Listing listing)
        {
            Root().Listings.Remove(new Key(new Object[] { listing.Year, listing.Month }), listing);
            if (listing.Employer != null) {
                listing.Employer.RemoveListing(listing);
            }
            Storage().Deallocate(listing);
            Storage().Commit();
        }
        
    }
}
