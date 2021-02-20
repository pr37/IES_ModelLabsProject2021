using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTN.Common;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
    public class DocumentConverter
    {
		#region Populate ResourceDescription 
		public static void PopulateIdentifiedObjectProperties(IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
		{
			if ((cimIdentifiedObject != null) && (rd != null))
			{
				if (cimIdentifiedObject.MRIDHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cimIdentifiedObject.MRID));
				}
				if (cimIdentifiedObject.NameHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cimIdentifiedObject.Name));
				}
				if (cimIdentifiedObject.AliasNameHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_ALIASNAME, cimIdentifiedObject.AliasName));
				}
			}
		}

		public static void PopulateDocumentProperties(Document cimDocument, ResourceDescription rd)
		{
			if ((cimDocument != null) && (rd != null))
			{
				DocumentConverter.PopulateIdentifiedObjectProperties(cimDocument, rd);
				if (cimDocument.CreatedDateTimeHasValue)
				{
					rd.AddProperty(new Property(ModelCode.DOCUMENT_CREATEDATETIME, cimDocument.CreatedDateTime));
				}
				if (cimDocument.DocStatusHasValue)
				{
					rd.AddProperty(new Property(ModelCode.DOCUMENT_DOCSTATUS, cimDocument.DocStatus));
				}
				if (cimDocument.ElectronicAddressHasValue)
				{
					rd.AddProperty(new Property(ModelCode.DOCUMENT_ELECTRONICADDRESS, cimDocument.ElectronicAddress));
				}
				if (cimDocument.LastModifiedDateTimeHasValue)
				{
					rd.AddProperty(new Property(ModelCode.DOCUMENT_LASTMODIFIEDTIME, cimDocument.LastModifiedDateTime));
				}
				if (cimDocument.RevisionNumberHasValue)
				{
					rd.AddProperty(new Property(ModelCode.DOCUMENT_REVISIONNUMBER, cimDocument.RevisionNumber));
				}
				if (cimDocument.StatusHasValue)
				{
					rd.AddProperty(new Property(ModelCode.DOCUMENT_STATUS, cimDocument.Status));
				}
				if (cimDocument.SubjectHasValue)
				{
					rd.AddProperty(new Property(ModelCode.DOCUMENT_SUBJECT, cimDocument.Subject));
				}
				if (cimDocument.TitleHasValue)
				{
					rd.AddProperty(new Property(ModelCode.DOCUMENT_TITLE, cimDocument.Title));
				}
				if (cimDocument.TypeHasValue)
				{
					rd.AddProperty(new Property(ModelCode.DOCUMENT_TYPE, cimDocument.Type));
				}
			}
		}

		public static void PopulateMarketDocumentProperties(MarketDocument cimMarketDocument, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimMarketDocument != null) && (rd != null))
			{
				//DocumentConverter.PopulateIdentifiedObjectProperties(cimMarketDocument, rd);
				DocumentConverter.PopulateDocumentProperties(cimMarketDocument, rd);

				if (cimMarketDocument.ProcessHasValue)
				{
					long gid = importHelper.GetMappedGID(cimMarketDocument.Process.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimMarketDocument.GetType().ToString()).Append(" rdfID = \"").Append(cimMarketDocument.ID);
						report.Report.Append("\" - Failed to set reference to PowerTransformer: rdfID \"").Append(cimMarketDocument.Process.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.MARKETDOCUMENT_PROCESS, gid));
				}
			}
		}

		public static void PopulateMeasurmentPointProperties(MeasurementPoint cimMeasurmentPoint, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimMeasurmentPoint != null) && (rd != null))
			{
				DocumentConverter.PopulateIdentifiedObjectProperties(cimMeasurmentPoint, rd);

				if (cimMeasurmentPoint.TimeSeriesHasValue)
				{
					long gid = importHelper.GetMappedGID(cimMeasurmentPoint.TimeSeries.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimMeasurmentPoint.GetType().ToString()).Append(" rdfID = \"").Append(cimMeasurmentPoint.ID);
						report.Report.Append("\" - Failed to set reference to PowerTransformer: rdfID \"").Append(cimMeasurmentPoint.TimeSeries.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.MEASURMENTPOINT_TIMESERIES, gid));
				}
			}
		}

		public static void PopulateTimeSeriesProperties(TimeSeries cimTimeSeries, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimTimeSeries != null) && (rd != null))
			{
				if (cimTimeSeries.ObjectAggregationHasValue)
				{
					rd.AddProperty(new Property(ModelCode.TIMESERIES_OBJECTAGGREGATION, cimTimeSeries.ObjectAggregation));
				}
				if (cimTimeSeries.ProductHasValue)
				{
					rd.AddProperty(new Property(ModelCode.TIMESERIES_PRODUCT, cimTimeSeries.Product));
				}
				if (cimTimeSeries.VersionHasValue)
				{
					rd.AddProperty(new Property(ModelCode.TIMESERIES_VERSION, cimTimeSeries.Version));
				}
				if (cimTimeSeries.MarketDocumentHasValue)
				{
					long gid = importHelper.GetMappedGID(cimTimeSeries.MarketDocument.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimTimeSeries.GetType().ToString()).Append(" rdfID = \"").Append(cimTimeSeries.ID);
						report.Report.Append("\" - Failed to set reference to PowerTransformer: rdfID \"").Append(cimTimeSeries.MarketDocument.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.TIMESERIES_MARKETDOCUMENT, gid));
				}
				if (cimTimeSeries.PeriodHasValue)
				{
					long gid = importHelper.GetMappedGID(cimTimeSeries.Period.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimTimeSeries.GetType().ToString()).Append(" rdfID = \"").Append(cimTimeSeries.ID);
						report.Report.Append("\" - Failed to set reference to PowerTransformer: rdfID \"").Append(cimTimeSeries.Period.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.TIMESERIES_PERIOD, gid));
				}
			}
		}

		public static void PopulateProcessProperties(Process cimProcess, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimProcess != null) && (rd != null))
			{
				DocumentConverter.PopulateIdentifiedObjectProperties(cimProcess, rd);

				if (cimProcess.ClassificationTypeHasValue)
				{
					rd.AddProperty(new Property(ModelCode.PROCESS_CLASSIFICATIONTYPE,cimProcess.ClassificationType));
				}
				if (cimProcess.ProcessTypeHasValue)
				{
					rd.AddProperty(new Property(ModelCode.PROCESS_PROCESSTYPE, cimProcess.ProcessType));
				}
				
			}
		}

		public static void PopulatePeriodProperties(Period cimPeriod, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimPeriod != null) && (rd != null))
			{
				DocumentConverter.PopulateIdentifiedObjectProperties(cimPeriod, rd);

				if (cimPeriod.ResolutionHasValue)
				{
					rd.AddProperty(new Property(ModelCode.PERIOD_RESOLUTION, cimPeriod.Resolution));
				}
				if (cimPeriod.MarketDocumentHasValue)
				{
					long gid = importHelper.GetMappedGID(cimPeriod.MarketDocument.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimPeriod.GetType().ToString()).Append(" rdfID = \"").Append(cimPeriod.ID);
						report.Report.Append("\" - Failed to set reference to PowerTransformer: rdfID \"").Append(cimPeriod.MarketDocument.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.PERIOD_MARKETDOCUMENT, gid));
				}
			}
		}

		public static void PopulatePointProperties(Point cimPoint, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimPoint != null) && (rd != null))
			{
				DocumentConverter.PopulateIdentifiedObjectProperties(cimPoint, rd);

				if (cimPoint.PositionHasValue)
				{
					rd.AddProperty(new Property(ModelCode.POINT_POSITION, cimPoint.Position));
				}
				if (cimPoint.PeriodHasValue)
				{
					long gid = importHelper.GetMappedGID(cimPoint.Period.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimPoint.GetType().ToString()).Append(" rdfID = \"").Append(cimPoint.ID);
						report.Report.Append("\" - Failed to set reference to PowerTransformer: rdfID \"").Append(cimPoint.Period.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.PERIOD_MARKETDOCUMENT, gid));
				}
			}
		}

		#endregion Populate ResourceDescription
	}
}
