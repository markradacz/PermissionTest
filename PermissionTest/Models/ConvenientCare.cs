using System;
using System.Linq;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace PermissionTest
{
    public class ConvenientCare 
    {
        public ConvenientCare()
        {
            Name = string.Empty;
            Address = string.Empty;
            PhoneNumber = string.Empty;
            NumberOfPatientsWaiting = 0;
			WaitTimeColor = "#FFFF00"; //string.Empty;
			EstimatedWaitTime = 0;
            URL = string.Empty;
   			Longitude = 0.00;
            Latitude = 0.00;
            DistanceFromLocation = 0.00;
        }

 		[JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
		[JsonProperty(PropertyName = "longitude")]
		public double Longitude { get; set; }
		[JsonProperty(PropertyName = "latitude")]
		public double Latitude { get; set; }
		[JsonProperty(PropertyName = "url")]
		public string URL { get; set; }
		[JsonProperty(PropertyName = "phoneNumber")]
		public string PhoneNumber { get; set; }
		[JsonProperty(PropertyName = "estimatedWaitTime")]
		public int EstimatedWaitTime { get; set; }
		[JsonProperty(PropertyName = "waitTimeColor")]
		public string WaitTimeColor { get; set; }
		[JsonProperty(PropertyName = "hours")]
		public string Hours { get; set; }
		[JsonProperty(PropertyName = "numberOfPatientsWaiting")]
        public int NumberOfPatientsWaiting { get; set; }

        [JsonIgnore]
        public bool IsClosed
        {
            get { return this.Hours.Equals("Closed", StringComparison.OrdinalIgnoreCase); }
        }

        [JsonIgnore]
        public string WaitTimeText
        {
			get
			{
				if (this.IsClosed)
					return string.Format("- {0} -", this.Hours);
				else
					return string.Format("- {0} min -", this.EstimatedWaitTime);
			}
		}

        [JsonIgnore]
        public Color WaitTimeColorAsColor
        {
            get
            {
                try
                {
                    if (this.IsClosed)
                    return Color.FromHex("#333333");

                    if (string.IsNullOrWhiteSpace(this.WaitTimeColor))
                        return Color.White;

                    if (this.WaitTimeColor.SafeTrim().ToUpper() == "#FFFF00")
                        return Color.FromHex("#FF9800");

                    return Color.FromHex(this.WaitTimeColor);
                }
                catch (Exception)
                {
                    return Color.White;
                }
            }
        }

        [JsonIgnore]
        public Color WaitTimeTextAsColor
        {
            get
            {
                if (this.IsClosed)
                    return Color.White;

                if (string.IsNullOrWhiteSpace(this.WaitTimeColor))
                    return Color.FromHex("#333333");

                try
                {
                    if (this.WaitTimeColor == "#FFFF00")
                        return Color.FromHex("#333333");

                    return Color.White;
                }
                catch (Exception)
                {
                    return Color.White;
                    throw;
                }
            }
        }

		[JsonIgnore]
        public double DistanceFromLocation { get; set; }
    }
}