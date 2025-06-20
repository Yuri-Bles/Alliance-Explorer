using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4DTOs
{
	public class AllianceDto
	{
		public int Id;
		public string? Name;
		public int MemberCount;
		public int? MinimumAge = null;
		public int? MaximumAge = null;
		public string? Language;
		public double? Latitude = null;
		public double? Longitude = null;
		public string? Rules = null;
		public bool AgeIsForced;
		public bool IsOnLocation;
		public bool IsOnline;
		public bool AllowCrewmemberEvents;
	}
}
