using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.MarketManagement
{
    public class TimeSeries : DataModel.Core.IdentifiedObject
    {
		private string objectAggregation = string.Empty;
		private string product = string.Empty;
		private string version = string.Empty;
		private List<long> measurmentPoints = new List<long>();
		private long marketDocument = 0;
		private long period = 0;

		public TimeSeries(long globalId) : base(globalId)
		{
		}



		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				TimeSeries x = (TimeSeries)obj;
				return (x.objectAggregation == this.objectAggregation &&
						x.product == this.product &&
						x.version == this.version &&
					CompareHelper.CompareLists(x.measurmentPoints, this.measurmentPoints, true) &&
					x.marketDocument == this.marketDocument &&
					x.period == this.period);
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
				case ModelCode.TIMESERIES_MARKETDOCUMENT:
				case ModelCode.TIMESERIES_MEASURMENTPOINT:
				case ModelCode.TIMESERIES_OBJECTAGGREGATION:
				case ModelCode.TIMESERIES_PERIOD:
				case ModelCode.TIMESERIES_PRODUCT:
				case ModelCode.TIMESERIES_VERSION:
					return true;

				default:
					return base.HasProperty(t);
			}
		}

		public override void GetProperty(Property prop)
		{
			switch (prop.Id)
			{
				case ModelCode.TIMESERIES_MARKETDOCUMENT:
					prop.SetValue(marketDocument);
					break;

				case ModelCode.TIMESERIES_MEASURMENTPOINT:
					prop.SetValue(measurmentPoints);
					break;

				case ModelCode.TIMESERIES_OBJECTAGGREGATION:
					prop.SetValue(objectAggregation);
					break;

				case ModelCode.TIMESERIES_PERIOD:
					prop.SetValue(period);
					break;

				case ModelCode.TIMESERIES_PRODUCT:
					prop.SetValue(product);
					break;

				case ModelCode.TIMESERIES_VERSION:
					prop.SetValue(version);
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
				case ModelCode.TIMESERIES_MARKETDOCUMENT:
					marketDocument = property.AsLong();
					break;

				case ModelCode.TIMESERIES_MEASURMENTPOINT:
					measurmentPoints = property.AsLongs();
					break;

				case ModelCode.TIMESERIES_OBJECTAGGREGATION:
					objectAggregation = property.AsString();
					break;

				case ModelCode.TIMESERIES_PERIOD:
					period = property.AsLong();
					break;

				case ModelCode.TIMESERIES_PRODUCT:
					product = property.AsString();
					break;

				case ModelCode.TIMESERIES_VERSION:
					version = property.AsString();
					break;

				default:
					base.SetProperty(property);
					break;
			}
		}

		#endregion IAccess implementation

		#region IReference implementation

		public override bool IsReferenced
		{
			get
			{
				return measurmentPoints.Count > 0  || base.IsReferenced;
			}
		}

        public string ObjectAggregation { get => objectAggregation; set => objectAggregation = value; }
        public string Product { get => product; set => product = value; }
        public string Version { get => version; set => version = value; }
        public List<long> MeasurmentPoints { get => measurmentPoints; set => measurmentPoints = value; }
        public long MarketDocument { get => marketDocument; set => marketDocument = value; }
        public long Period { get => period; set => period = value; }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
		{
			if (measurmentPoints != null && measurmentPoints.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
			{
				references[ModelCode.TIMESERIES_MEASURMENTPOINT] = measurmentPoints.GetRange(0, measurmentPoints.Count);
			}


			base.GetReferences(references, refType);
		}

		public override void AddReference(ModelCode referenceId, long globalId)
		{
			switch (referenceId)
			{
				case ModelCode.TIMESERIES_MEASURMENTPOINT:
					measurmentPoints.Add(globalId);
					break;

				default:
					base.AddReference(referenceId, globalId);
					break;
			}
		}

		public override void RemoveReference(ModelCode referenceId, long globalId)
		{
			switch (referenceId)
			{
				case ModelCode.TIMESERIES_MEASURMENTPOINT:

					if (measurmentPoints.Contains(globalId))
					{
						measurmentPoints.Remove(globalId);
					}
					else
					{
						CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
					}

					break;

				

				default:
					base.RemoveReference(referenceId, globalId);
					break;
			}
		}

		#endregion IReference implementation	
	
	}
}
