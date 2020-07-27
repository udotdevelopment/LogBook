using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogBook.Models.AIMS
{
    [Table("PhysicalLocation")]
    public class PhysicalLocation
    {
        [Key]
        public int IntId { get; set; }

        public int ExtId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        //public DatumType DatumType { get; set; }

        public string Route { get; set; }

        public float? MilePosting { get; set; }

        public string Lrs { get; set; }

        //public string ComponentNames { get; set; }

        //public string UdotAssetTags { get; set; }

        //public string SerialNumbers { get; set; }

        //public string ComponentTypesNames { get; set; }

        //public string ComponentTypesCabinetNames { get; set; }

        //public string ComponentTypesCabinetNamesTags { get; set; }

        //public string TransSuiteId { get; set; }

        //public string TransSuiteIDs { get; set; }

        //public string TransSuiteIDsWithNames { get; set; }

        //public string TransSuiteIDsWithNamesTags { get; set; }

        //public string TransSuiteIDsWithNamesNewLines { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public int? StateId { get; set; }

        //public int? RegionIdNumber { get; set; }

        public string ZipCode { get; set; }

        public bool IsActive { get; set; }

        public int CreatedByUserID { get; set; }

        public int? UpdatedByUserID { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? LastUpdated { get; set; }

        public string County { get; set; }

        public int? SignalId { get; set; }

        //public State StateType { get; set; }

        //public List<double?> LatLon { get; set; }

        public bool IsDefault { get; set; }

        public string Channel { get; set; }

        //public string IsRepaired { get; set; }
    }
}