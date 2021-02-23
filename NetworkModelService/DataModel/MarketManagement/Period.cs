using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.MarketManagement
{
    public class Period : DataModel.Core.IdentifiedObject
    {
		private Duration resolution = new Duration();
		private List<long> marketDocuments = new List<long>();
		private List<long> timeSeries = new List<long>();
		private List<long> points = new List<long>();

		public Period(long globalId) : base(globalId)
		{
		}



		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				Period x = (Period)obj;
				return (x.resolution == this.resolution &&
					CompareHelper.CompareLists(x.marketDocuments, this.marketDocuments, true) && 
					CompareHelper.CompareLists(x.timeSeries, this.timeSeries, true) && 
					CompareHelper.CompareLists(x.points, this.points, true)
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
				case ModelCode.PERIOD_MARKETDOCUMENT:
				case ModelCode.PERIOD_POINT:
				case ModelCode.PERIOD_RESOLUTION:
				case ModelCode.PERIOD_TIMESERIES:
					return true;

				default:
					return base.HasProperty(t);
			}
		}

		public override void GetProperty(Property prop)
		{
			switch (prop.Id)
			{
				case ModelCode.PERIOD_RESOLUTION:
					prop.SetValue(resolution);
					break;

				case ModelCode.PERIOD_MARKETDOCUMENT:
					prop.SetValue(marketDocuments);
					break;

				case ModelCode.PERIOD_POINT:
					prop.SetValue(points);
					break;

				case ModelCode.PERIOD_TIMESERIES:
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
				case ModelCode.PERIOD_MARKETDOCUMENT:
					marketDocuments = property.AsLongs();
					break;

				case ModelCode.PERIOD_POINT:
					points = property.AsLongs();
					break;

				case ModelCode.PERIOD_TIMESERIES:
					timeSeries = property.AsLongs();
					break;

				case ModelCode.PERIOD_RESOLUTION:
					//TODO
					//resolution = property.AsString();
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
				return marketDocuments.Count > 0 || points.Count > 0 || timeSeries.Count > 0 || base.IsReferenced;
			}
		}

        public Duration Resolution { get => resolution; set => resolution = value; }
        public List<long> MarketDocuments { get => marketDocuments; set => marketDocuments = value; }
        public List<long> TimeSeries { get => timeSeries; set => timeSeries = value; }
        public List<long> Points { get => points; set => points = value; }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
		{
			if (marketDocuments != null && marketDocuments.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
			{
				references[ModelCode.PERIOD_MARKETDOCUMENT] = marketDocuments.GetRange(0, marketDocuments.Count);
			}

			if (points != null && points.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
			{
				references[ModelCode.PERIOD_POINT] = points.GetRange(0, points.Count);
			}

			if (timeSeries != null && timeSeries.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
			{
				references[ModelCode.PERIOD_TIMESERIES] = timeSeries.GetRange(0, timeSeries.Count);
			}

			base.GetReferences(references, refType);
		}

		public override void AddReference(ModelCode referenceId, long globalId)
		{
			switch (referenceId)
			{
				case ModelCode.PERIOD_MARKETDOCUMENT:
					marketDocuments.Add(globalId);
					break;

				case ModelCode.PERIOD_POINT:
					points.Add(globalId);
					break;

				case ModelCode.PERIOD_TIMESERIES:
					timeSeries.Add(globalId);
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
				case ModelCode.PERIOD_MARKETDOCUMENT:

					if (marketDocuments.Contains(globalId))
					{
						marketDocuments.Remove(globalId);
					}
					else
					{
						CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
					}

					break;

				case ModelCode.PERIOD_POINT:

					if (points.Contains(globalId))
					{
						points.Remove(globalId);
					}
					else
					{
						CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
					}

					break;

				case ModelCode.PERIOD_TIMESERIES:

					if (timeSeries.Contains(globalId))
					{
						timeSeries.Remove(globalId);
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
