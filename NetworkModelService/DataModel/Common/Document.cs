using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Common
{
    public class Document : DataModel.Core.IdentifiedObject
    {
		private DateTime createdDateTime = default(DateTime);
		private Status docStatus;
		private ElectronicAddress electronicAddress;
		private DateTime lastModifiedDateTime = default(DateTime);
		private string revisionNumber = string.Empty;
		private Status status;
		private string subject = string.Empty;
		private string title = string.Empty;
		private string type = string.Empty;

        public DateTime CreatedDateTime { get => createdDateTime; set => createdDateTime = value; }
        public Status DocStatus { get => docStatus; set => docStatus = value; }
        public ElectronicAddress ElectronicAddress { get => electronicAddress; set => electronicAddress = value; }
        public DateTime LastModifiedDateTime { get => lastModifiedDateTime; set => lastModifiedDateTime = value; }
        public string RevisionNumber { get => revisionNumber; set => revisionNumber = value; }
        public Status Status { get => status; set => status = value; }
        public string Subject { get => subject; set => subject = value; }
        public string Title { get => title; set => title = value; }
        public string Type { get => type; set => type = value; }

        public Document(long globalId)
			: base(globalId)
		{
		}


		public static bool operator ==(Document x, Document y)
		{
			if (Object.ReferenceEquals(x, null) && Object.ReferenceEquals(y, null))
			{
				return true;
			}
			else if ((Object.ReferenceEquals(x, null) && !Object.ReferenceEquals(y, null)) || (!Object.ReferenceEquals(x, null) && Object.ReferenceEquals(y, null)))
			{
				return false;
			}
			else
			{
				return x.Equals(y);
			}
		}

		public static bool operator !=(Document x, Document y)
		{
			return !(x == y);
		}

		public override bool Equals(object x)
		{
			if (Object.ReferenceEquals(x, null))
			{
				return false;
			}
			else
			{
				Document io = (Document)x;
				return ((io.createdDateTime == this.createdDateTime) &&
					(io.status == this.status) && (io.electronicAddress == this.electronicAddress) &&
					(io.lastModifiedDateTime == this.lastModifiedDateTime) && (io.revisionNumber == this.revisionNumber) &&
					(io.docStatus == this.docStatus) && (io.subject == this.subject) &&
					(io.title == this.title) && (io.type == this.type)
					);
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#region IAccess implementation		

		public virtual bool HasProperty(ModelCode property)
		{
			switch (property)
			{
				case ModelCode.DOCUMENT_CREATEDATETIME:
				case ModelCode.DOCUMENT_DOCSTATUS:
				case ModelCode.DOCUMENT_ELECTRONICADDRESS:
				case ModelCode.DOCUMENT_LASTMODIFIEDTIME:
				case ModelCode.DOCUMENT_REVISIONNUMBER:
				case ModelCode.DOCUMENT_STATUS:
				case ModelCode.DOCUMENT_SUBJECT:
				case ModelCode.DOCUMENT_TITLE:
				case ModelCode.DOCUMENT_TYPE:
					return true;

				default:
					return false;
			}
		}

		public virtual void GetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.DOCUMENT_CREATEDATETIME:
					property.SetValue(createdDateTime);
					break;

				case ModelCode.DOCUMENT_DOCSTATUS:
					property.SetValue(docStatus);
					break;

				case ModelCode.DOCUMENT_ELECTRONICADDRESS:
					property.SetValue(electronicAddress);
					break;

				case ModelCode.DOCUMENT_LASTMODIFIEDTIME:
					property.SetValue(lastModifiedDateTime);
					break;

				case ModelCode.DOCUMENT_REVISIONNUMBER:
					property.SetValue(revisionNumber);
					break;
				case ModelCode.DOCUMENT_STATUS:
					property.SetValue(status);
					break;
				case ModelCode.DOCUMENT_SUBJECT:
					property.SetValue(subject);
					break;
				case ModelCode.DOCUMENT_TITLE:
					property.SetValue(title);
					break;
				case ModelCode.DOCUMENT_TYPE:
					property.SetValue(type);
					break;

				default:
					string message = string.Format("Unknown property id = {0} for entity (GID = 0x{1:x16}).", property.Id.ToString(), this.GlobalId);
					CommonTrace.WriteTrace(CommonTrace.TraceError, message);
					throw new Exception(message);
			}
		}

		public virtual void SetProperty(Property property)
		{
			switch (property.Id)
			{

				case ModelCode.DOCUMENT_CREATEDATETIME:
					createdDateTime = property.AsDateTime();
					break;

				case ModelCode.DOCUMENT_DOCSTATUS:
					//docStatus = property.; //TODO
					break;

				case ModelCode.DOCUMENT_ELECTRONICADDRESS:
					//TODO
					break;

				case ModelCode.DOCUMENT_LASTMODIFIEDTIME:
					lastModifiedDateTime = property.AsDateTime();
					break;

				case ModelCode.DOCUMENT_REVISIONNUMBER:
					revisionNumber = property.AsString();
					break;
				case ModelCode.DOCUMENT_STATUS:
					//TODO
					break;
				case ModelCode.DOCUMENT_SUBJECT:
					subject = property.AsString();
					break;
				case ModelCode.DOCUMENT_TITLE:
					title = property.AsString();
					break;
				case ModelCode.DOCUMENT_TYPE:
					type = property.AsString();
					break;

				default:
					string message = string.Format("Unknown property id = {0} for entity (GID = 0x{1:x16}).", property.Id.ToString(), this.GlobalId);
					CommonTrace.WriteTrace(CommonTrace.TraceError, message);
					throw new Exception(message);
			}
		}

		#endregion IAccess implementation

		
	}
}
