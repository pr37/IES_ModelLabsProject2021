using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIM.Model;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
    public class DocumentImporter
    {
		/// <summary> Singleton </summary>
		private static DocumentImporter ptImporter = null;
		private static object singletoneLock = new object();

		private ConcreteModel concreteModel;
		private Delta delta;
		private ImportHelper importHelper;
		private TransformAndLoadReport report;


		#region Properties
		public static DocumentImporter Instance
		{
			get
			{
				if (ptImporter == null)
				{
					lock (singletoneLock)
					{
						if (ptImporter == null)
						{
							ptImporter = new DocumentImporter();
							ptImporter.Reset();
						}
					}
				}
				return ptImporter;
			}
		}

		public Delta NMSDelta
		{
			get
			{
				return delta;
			}
		}
		#endregion Properties


		public void Reset()
		{
			concreteModel = null;
			delta = new Delta();
			importHelper = new ImportHelper();
			report = null;
		}

		public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
		{
			LogManager.Log("Importing PowerTransformer Elements...", LogLevel.Info);
			report = new TransformAndLoadReport();
			concreteModel = cimConcreteModel;
			delta.ClearDeltaOperations();

			if ((concreteModel != null) && (concreteModel.ModelMap != null))
			{
				try
				{
					// convert into DMS elements
					ConvertModelAndPopulateDelta();
				}
				catch (Exception ex)
				{
					string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
					LogManager.Log(message);
					report.Report.AppendLine(ex.Message);
					report.Success = false;
				}
			}
			LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
			return report;
		}

		/// <summary>
		/// Method performs conversion of network elements from CIM based concrete model into DMS model.
		/// </summary>
		private void ConvertModelAndPopulateDelta()
		{
			LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

			//// import all concrete model types (DMSType enum)
			ImportProcess();
			ImportMeasurmentPoint();
			ImportTimeSeriesDocument();
			ImportMarketDocument();
			ImportPoint();
			ImportPeriodDocument();

			LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
		}
		private void ImportProcess()
		{
			SortedDictionary<string, object> cimProcesses = concreteModel.GetAllObjectsOfType("FTN.Process");
			if (cimProcesses != null)
			{
				foreach (KeyValuePair<string, object> cimProcessPair in cimProcesses)
				{
					FTN.Process cimProcess = cimProcessPair.Value as FTN.Process;

					ResourceDescription rd = CreateProcessResourceDescription(cimProcess);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("Process ID = ").Append(cimProcess.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("Process ID = ").Append(cimProcess.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateProcessResourceDescription(FTN.Process cimProcess)
		{
			ResourceDescription rd = null;
			if (cimProcess != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.PROCESS, importHelper.CheckOutIndexForDMSType(DMSType.PROCESS));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimProcess.ID, gid);

				////populate ResourceDescription
				DocumentConverter.PopulateProcessProperties(cimProcess, rd, importHelper, report);
			}
			return rd;
		}

		private void ImportMarketDocument()
		{
			SortedDictionary<string, object> cimMarketDocuments = concreteModel.GetAllObjectsOfType("FTN.MarketDocument");
			if (cimMarketDocuments != null)
			{
				foreach (KeyValuePair<string, object> cimProcessPair in cimMarketDocuments)
				{
					FTN.MarketDocument cimMarketDocument = cimProcessPair.Value as FTN.MarketDocument;

					ResourceDescription rd = CreateMarketDocumentResourceDescription(cimMarketDocument);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("Process ID = ").Append(cimMarketDocument.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("Process ID = ").Append(cimMarketDocument.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateMarketDocumentResourceDescription(FTN.MarketDocument cimMarketDocument)
		{
			ResourceDescription rd = null;
			if (cimMarketDocument != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.MARKETDOCUMENT, importHelper.CheckOutIndexForDMSType(DMSType.MARKETDOCUMENT));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimMarketDocument.ID, gid);

				////populate ResourceDescription
				DocumentConverter.PopulateMarketDocumentProperties(cimMarketDocument, rd, importHelper, report);
			}
			return rd;
		}

		private void ImportPoint()
		{
			SortedDictionary<string, object> cimPoints = concreteModel.GetAllObjectsOfType("FTN.Point");
			if (cimPoints != null)
			{
				foreach (KeyValuePair<string, object> cimPointPair in cimPoints)
				{
					FTN.Point cimPoint = cimPointPair.Value as FTN.Point;

					ResourceDescription rd = CreatePointResourceDescription(cimPoint);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("Process ID = ").Append(cimPoint.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("Process ID = ").Append(cimPoint.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreatePointResourceDescription(FTN.Point cimPoint)
		{
			ResourceDescription rd = null;
			if (cimPoint != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.POINT, importHelper.CheckOutIndexForDMSType(DMSType.POINT));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimPoint.ID, gid);

				////populate ResourceDescription
				DocumentConverter.PopulatePointProperties(cimPoint, rd, importHelper, report);
			}
			return rd;
		}

		private void ImportPeriodDocument()
		{
			SortedDictionary<string, object> cimPeriods = concreteModel.GetAllObjectsOfType("FTN.Period");
			if (cimPeriods != null)
			{
				foreach (KeyValuePair<string, object> cimPeriodPair in cimPeriods)
				{
					FTN.Period cimPeriod = cimPeriodPair.Value as FTN.Period;

					ResourceDescription rd = CreatePeriodResourceDescription(cimPeriod);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("Process ID = ").Append(cimPeriod.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("Process ID = ").Append(cimPeriod.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreatePeriodResourceDescription(FTN.Period cimPeriod)
		{
			ResourceDescription rd = null;
			if (cimPeriod != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.PERIOD, importHelper.CheckOutIndexForDMSType(DMSType.PERIOD));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimPeriod.ID, gid);

				////populate ResourceDescription
				DocumentConverter.PopulatePeriodProperties(cimPeriod, rd, importHelper, report);
			}
			return rd;
		}

		private void ImportTimeSeriesDocument()
		{
			SortedDictionary<string, object> cimTimeSeriess = concreteModel.GetAllObjectsOfType("FTN.TimeSeries");
			if (cimTimeSeriess != null)
			{
				foreach (KeyValuePair<string, object> cimTimeSeriesPair in cimTimeSeriess)
				{
					FTN.TimeSeries cimTimeSeries = cimTimeSeriesPair.Value as FTN.TimeSeries;

					ResourceDescription rd = CreateTimeSeriesResourceDescription(cimTimeSeries);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("Process ID = ").Append(cimTimeSeries.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("Process ID = ").Append(cimTimeSeries.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateTimeSeriesResourceDescription(FTN.TimeSeries cimTimeSeries)
		{
			ResourceDescription rd = null;
			if (cimTimeSeries != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.TIMESERIES, importHelper.CheckOutIndexForDMSType(DMSType.TIMESERIES));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimTimeSeries.ID, gid);

				////populate ResourceDescription
				DocumentConverter.PopulateTimeSeriesProperties(cimTimeSeries, rd, importHelper, report);
			}
			return rd;
		}

		private void ImportMeasurmentPoint()
		{
			SortedDictionary<string, object> cimMeasurmentPoints = concreteModel.GetAllObjectsOfType("FTN.MeasurementPoint");
			if (cimMeasurmentPoints != null)
			{
				foreach (KeyValuePair<string, object> cimMeasurmentPointPair in cimMeasurmentPoints)
				{
					FTN.MeasurementPoint cimMeasurmentPoint = cimMeasurmentPointPair.Value as FTN.MeasurementPoint;

					ResourceDescription rd = CreateMeasurmentPointResourceDescription(cimMeasurmentPoint);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("Process ID = ").Append(cimMeasurmentPoint.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("Process ID = ").Append(cimMeasurmentPoint.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateMeasurmentPointResourceDescription(FTN.MeasurementPoint cimMeasurmentPoint)
		{
			ResourceDescription rd = null;
			if (cimMeasurmentPoint != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.MEASURMENTPOINT, importHelper.CheckOutIndexForDMSType(DMSType.MEASURMENTPOINT));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimMeasurmentPoint.ID, gid);

				////populate ResourceDescription
				DocumentConverter.PopulateMeasurmentPointProperties(cimMeasurmentPoint, rd, importHelper, report);
			}
			return rd;
		}
	}
}
