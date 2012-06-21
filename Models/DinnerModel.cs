using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;

namespace DinnerParty.Models
{
    public class Dinner
    {
        /// <summary>
        /// Id (in that exact case) is used by Raven.
        /// If on a Url of /dinners/edit/162 Nancy will bind the Id property to 162 so,
        /// we have to make sure that it starts with dinners/ so Raven can identify it properly
        /// </summary>
        private string id;
        public string Id
        {
            get { return id; }
            set
            {
                if (!value.StartsWith("dinners"))
                    value = "dinners/" + value;

                id = value;
            }
        }

        //[HiddenInput(DisplayValue = false)]
        [JsonIgnore]
        public int DinnerID
        {
            get
            {
                return int.Parse(Id.Substring(Id.LastIndexOf("/") + 1));
            }
        }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(50, ErrorMessage = "Title may not be longer than 50 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Event Date is required")]
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(256, ErrorMessage = "Description may not be longer than 256 characters")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [StringLength(20, ErrorMessage = "Hosted By name may not be longer than 20 characters")]
        [Display(Name = "Host's Name")]
        public string HostedBy { get; set; }

        [Required(ErrorMessage = "Contact info is required")]
        [StringLength(20, ErrorMessage = "Contact info may not be longer than 20 characters")]
        [Display(Name = "Contact Info")]
        public string ContactPhone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(50, ErrorMessage = "Address may not be longer than 50 characters")]
        [Display(Name = "Address, City, State, ZIP")]
        public string Address { get; set; }

        [UIHint("CountryDropDown")]
        public string Country { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string HostedById { get; set; }

        public virtual ICollection<RSVP> RSVPs { get; set; }

        public bool IsHostedBy(string userName)
        {
            return String.Equals(HostedById ?? HostedBy, userName, StringComparison.Ordinal);
        }

        public bool IsUserRegistered(string userName)
        {
            return RSVPs.Any(r => r.AttendeeNameId == userName || (r.AttendeeNameId == null && r.AttendeeName == userName));
        }

        [UIHint("LocationDetail")]
        [JsonIgnore]
        public LocationDetail Location
        {
            get
            {
                return new LocationDetail() { Latitude = this.Latitude, Longitude = this.Longitude, Title = this.Title, Address = this.Address };
            }
            set
            {
                this.Latitude = value.Latitude;
                this.Longitude = value.Longitude;
                this.Title = value.Title;
                this.Address = value.Address;
            }
        }
    }

    public class LocationDetail
    {
        public double Latitude;
        public double Longitude;
        public string Title;
        public string Address;
    }
}