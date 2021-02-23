using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.MarketManagement
{
    public class MeasurmentPoint :  DataModel.Core.IdentifiedObject
    {
		private long timeSeries = 0;

		public MeasurmentPoint(long globalId) : base(globalId)
		{
		}

		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				MeasurmentPoint x = (MeasurmentPoint)obj;
				return (x.timeSeries == this.timeSeries 
					);
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#region IAccess implementation

		public override bool HasProperty(ModelCode t)
		{
			switch (t)
			{
				case ModelCode.MEASURMENTPOINT_TIMESERIES:
					return true;

				default:
					return base.HasProperty(t);
			}
		}

		public override void GetProperty(Property prop)
		{
			switch (prop.Id)
			{
				case ModelCode.MEASURMENTPOINT_TIMESERIES:
					prop.SetValue(timeSeries);
					break;


				default:
					base.GetProperty(prop);
					break;
			}
		}

		public override void SetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.MEASURMENTPOINT_TIMESERIES:
					timeSeries = property.AsLong();
					break;


				default:
					base.SetProperty(property);
					break;
			}
		}

		#endregion IAccess implementation

		#region IReference implementation	

		public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
		{
			if (timeSeries != 0 && (refType != TypeOfReference.Reference || refType != TypeOfReference.Both))
			{
				references[ModelCode.MEASURMENTPOINT_TIMESERIES] = new List<long>();
				references[ModelCode.MEASURMENTPOINT_TIMESERIES].Add(timeSeries);
			}

			base.GetReferences(references, refType);
		}

		#endregion IReference implementation
	}
}
