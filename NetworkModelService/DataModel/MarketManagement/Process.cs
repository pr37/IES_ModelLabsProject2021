using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.MarketManagement
{
    public class Process : DataModel.Core.IdentifiedObject
    {
		private string classificationType = string.Empty;
		private string processType = string.Empty;
		private List<long> marketDocuments = new List<long>();

		public Process(long globalId) : base(globalId)
		{
		}



		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				Process x = (Process)obj;
				return (x.classificationType == this.classificationType &&
						x.processType == this.processType &&
					CompareHelper.CompareLists(x.marketDocuments, this.marketDocuments, true));
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
				case ModelCode.PROCESS_CLASSIFICATIONTYPE:
				case ModelCode.PROCESS_MARKETDOCUMENTS:
				case ModelCode.PROCESS_PROCESSTYPE:
					return true;

				default:
					return base.HasProperty(t);
			}
		}

		public override void GetProperty(Property prop)
		{
			switch (prop.Id)
			{
				case ModelCode.PROCESS_CLASSIFICATIONTYPE:
					prop.SetValue(classificationType);
					break;

				case ModelCode.PROCESS_MARKETDOCUMENTS:
					prop.SetValue(marketDocuments);
					break;

				case ModelCode.PROCESS_PROCESSTYPE:
					prop.SetValue(processType);
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
				case ModelCode.PROCESS_PROCESSTYPE:
					processType = property.AsString();
					break;

				case ModelCode.PROCESS_MARKETDOCUMENTS:
					marketDocuments = property.AsLongs();
					break;

				case ModelCode.PROCESS_CLASSIFICATIONTYPE:
					classificationType = property.AsString();
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
				return marketDocuments.Count > 0 || base.IsReferenced;
			}
		}

        public string ClassificationType { get => classificationType; set => classificationType = value; }
        public string ProcessType { get => processType; set => processType = value; }
        public List<long> MarketDocuments { get => marketDocuments; set => marketDocuments = value; }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
		{
			if (marketDocuments != null && marketDocuments.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
			{
				references[ModelCode.PROCESS_MARKETDOCUMENTS] = marketDocuments.GetRange(0, marketDocuments.Count);
			}


			base.GetReferences(references, refType);
		}

		public override void AddReference(ModelCode referenceId, long globalId)
		{
			switch (referenceId)
			{
				case ModelCode.PROCESS_MARKETDOCUMENTS:
					marketDocuments.Add(globalId);
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
				case ModelCode.PROCESS_MARKETDOCUMENTS:

					if (marketDocuments.Contains(globalId))
					{
						marketDocuments.Remove(globalId);
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
