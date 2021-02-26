﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Threading;
using System.Diagnostics;
using FTN.Common;
using FTN.ServiceContracts;
using FTN.Services.NetworkModelService.TestClient;


namespace TelventDMS.Services.NetworkModelService.TestClient.Tests
{
	public class TestGda : IDisposable
	{			

		private ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();

		private NetworkModelGDAProxy gdaQueryProxy = null;
		private NetworkModelGDAProxy GdaQueryProxy
		{
			get
			{
				if (gdaQueryProxy != null)
				{
					gdaQueryProxy.Abort();
                    gdaQueryProxy = null;
				}

				gdaQueryProxy = new NetworkModelGDAProxy("NetworkModelGDAEndpoint");
				gdaQueryProxy.Open();

				return gdaQueryProxy;
			}
		}
		
		public TestGda()
		{
		}

		#region GDAQueryService

		public ResourceDescription GetValues(long globalId)
		{
            string message = "Getting values method started.";
            Console.WriteLine(message);
            CommonTrace.WriteTrace(CommonTrace.TraceError, message);

			XmlTextWriter xmlWriter = null;
			ResourceDescription rd = null;
						
			try
			{
				short type = ModelCodeHelper.ExtractTypeFromGlobalId(globalId);
				List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds((DMSType)type);

                rd = GdaQueryProxy.GetValues(globalId, properties);

                xmlWriter = new XmlTextWriter(Config.Instance.ResultDirecotry + "\\GetValues_Results.xml", Encoding.Unicode);
				xmlWriter.Formatting = Formatting.Indented;
				rd.ExportToXml(xmlWriter);
				xmlWriter.Flush();

                message = "Getting values method successfully finished.";
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
			}
			catch (Exception e)
            {
                message = string.Format("Getting values method for entered id = {0} failed.\n\t{1}", globalId, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
   			}
			finally
			{
				if (xmlWriter != null)
				{
					xmlWriter.Close();
				}
			}

			return rd;
		}

		public List<long> GetExtentValues(ModelCode modelCode)
		{
            string message = "Getting extent values method started.";
            Console.WriteLine(message);
            CommonTrace.WriteTrace(CommonTrace.TraceError, message);

			XmlTextWriter xmlWriter = null;
			int iteratorId = 0;
			List<long> ids = new List<long>();

			try
			{				
				int numberOfResources = 2;
				int resourcesLeft = 0;

                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(modelCode);
				
				iteratorId = GdaQueryProxy.GetExtentValues(modelCode, properties);
				resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
								

				xmlWriter = new XmlTextWriter(Config.Instance.ResultDirecotry + "\\GetExtentValues_Results.xml", Encoding.Unicode);
				xmlWriter.Formatting = Formatting.Indented;

				while (resourcesLeft > 0)
				{
					List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

					for (int i = 0; i < rds.Count; i++)
					{
						ids.Add(rds[i].Id);
						rds[i].ExportToXml(xmlWriter);
						xmlWriter.Flush();
					}

					resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
				}

				GdaQueryProxy.IteratorClose(iteratorId);

                message = "Getting extent values method successfully finished.";
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
			
			}			
			catch (Exception e)
            {
                message = string.Format("Getting extent values method failed for {0}.\n\t{1}", modelCode, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
			}
			finally
			{				
				if (xmlWriter != null)
				{
					xmlWriter.Close();
				}
			}
			
			return ids;
		}

		public List<long> GetRelatedValues(long sourceGlobalId, Association association)
		{
            string message = "Getting related values method started.";
            Console.WriteLine(message);
            CommonTrace.WriteTrace(CommonTrace.TraceError, message);
			
			List<long> resultIds = new List<long>();

			
			XmlTextWriter xmlWriter = null;
			int numberOfResources = 2;

			try
			{						
				List<ModelCode> properties = new List<ModelCode>();
                properties.Add(ModelCode.IDOBJ_ALIASNAME);
                properties.Add(ModelCode.IDOBJ_MRID);
                properties.Add(ModelCode.IDOBJ_NAME);
						
				int iteratorId = GdaQueryProxy.GetRelatedValues(sourceGlobalId, properties, association);
				int resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                xmlWriter = new XmlTextWriter(Config.Instance.ResultDirecotry + "\\GetRelatedValues_Results.xml", Encoding.Unicode);
                xmlWriter.Formatting = Formatting.Indented; 

				while (resourcesLeft > 0)
				{
					List<ResourceDescription> rds =  GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

					for (int i = 0; i < rds.Count; i++)
					{
						resultIds.Add(rds[i].Id);
						rds[i].ExportToXml(xmlWriter);
						xmlWriter.Flush();
					}
							
					resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
				}

				GdaQueryProxy.IteratorClose(iteratorId);

                message = "Getting related values method successfully finished.";
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);				
			}
			catch (Exception e)
			{
                message = string.Format("Getting related values method  failed for sourceGlobalId = {0} and association (propertyId = {1}, type = {2}). Reason: {3}", sourceGlobalId, association.PropertyId, association.Type, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }
			finally
			{
				if (xmlWriter != null)
				{
					xmlWriter.Close();
				}
			}
						
			return resultIds;
		}

        #endregion GDAQueryService

        #region Test Methods

        public List<long> TestGetExtentValuesAllTypes()
        {
            string message = "Getting extent values for all DMS types started.";
            Console.WriteLine(message);
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

            List<ModelCode> properties = new List<ModelCode>();
            List<long> ids = new List<long>();

            int iteratorId = 0;
            int numberOfResources = 1000;
            DMSType currType = 0;
            try
            {
                foreach (DMSType type in Enum.GetValues(typeof(DMSType)))
                {
                    currType = type;
                    properties = modelResourcesDesc.GetAllPropertyIds(type);

                    iteratorId = GdaQueryProxy.GetExtentValues(modelResourcesDesc.GetModelCodeFromType(type), properties);
                    int count = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                    while (count > 0)
                    {
                        List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                        for (int i = 0; i < rds.Count; i++)
                        {
                            ids.Add(rds[i].Id);
                        }

                        count = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                    }

                    bool ok = GdaQueryProxy.IteratorClose(iteratorId);

                    message = string.Format("Number of {0} in model {1}.", type, ids.Count);
                    Console.WriteLine(message);
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
                }


                message = "Getting extent values for all DMS types successfully ended.";
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
            }

            catch (Exception e)
            {
                message = string.Format("Getting extent values for all DMS types failed for type {0}.\n\t{1}", currType, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

                throw;
            }

            return ids;
        }
        
        #region GDAUpdate Service

        public UpdateResult TestApplyDeltaInsert()
        {
            string message = "Apply update method started.";
            Console.WriteLine(message);
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);


            UpdateResult updateResult = null;

            try
            {
                Dictionary<DMSType, ResourceDescription> updates = CreateResourcesToInsert();
                Delta delta = new Delta();

                foreach (ResourceDescription rd in updates.Values)
                {
                    delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                }

                updateResult = GdaQueryProxy.ApplyUpdate(delta);

                message = "Apply update method finished. \n" + updateResult.ToString();
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

            }
            catch (Exception ex)
            {
                message = string.Format("Apply update method failed. {0}\n", ex.Message);

                if (updateResult != null)
                {
                    message += updateResult.ToString();
                }

                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return updateResult;
        }

        public UpdateResult TestApplyDeltaUpdate(List<long> gids)
        {
            string message = "Testing apply delta update method started.";
            Console.WriteLine(message);
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

            UpdateResult updateResult = null;

            try
            {
                Dictionary<DMSType, ResourceDescription> updates = CreateResourcesForUpdate(gids);
                Delta delta = new Delta();

                foreach (ResourceDescription rd in updates.Values)
                {
                    delta.AddDeltaOperation(DeltaOpType.Update, rd, true);
                }

                updateResult = GdaQueryProxy.ApplyUpdate(delta);

                message = "Testing apply delta update method finished. \n" + updateResult.ToString();
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
            }
            catch (Exception ex)
            {
                message = string.Format("Testing apply delta update method failed. {0}\n", ex.Message);

                if (updateResult != null)
                {
                    message += updateResult.ToString();
                }

                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return updateResult;
        }

        public UpdateResult TestApplyDeltaDelete(List<long> gids)
        {
            string message = "Testing apply delta delete method started.";
            Console.WriteLine(message);
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);

            UpdateResult updateResult = null;

            try
            {
                Delta delta = new Delta();
                ResourceDescription rd = null;

                foreach (long gid in gids)
                {
                    rd = new ResourceDescription(gid);
                    delta.AddDeltaOperation(DeltaOpType.Delete, rd, true);
                }

                updateResult = GdaQueryProxy.ApplyUpdate(delta);

                message = "Testing apply delta delete method finished. \n" + updateResult.ToString();
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
            }
            catch (Exception ex)
            {
                message = string.Format("Testing apply delta delete method failed. {0}\n", ex.Message);

                if (updateResult != null)
                {
                    message += updateResult.ToString();
                }

                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return updateResult;
        }

        public UpdateResult TestApplyDeltaInsertUpdateDelete()
        {
            UpdateResult updateResult = null;

            try
            {
                updateResult = TestApplyDeltaInsert();

                if (updateResult != null && updateResult.Result == ResultType.Succeeded)
                {
                    List<long> gids = new List<long>();
                    foreach (KeyValuePair<long, long> kvp in updateResult.GlobalIdPairs)
                    {
                        gids.Add(kvp.Value);
                    }

                    updateResult = TestApplyDeltaUpdate(gids);

                    updateResult = TestApplyDeltaDelete(gids);
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Test apply delta: Insert - Update - Delete failed.\t{0}", ex.Message);

                if (updateResult != null)
                {
                    message += updateResult.ToString();
                }

                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }

            return updateResult;
        }

        public void TestApplyDeltaInsertUpdate(long modelVersionId)
        {
            UpdateResult updateResult = null;
            try
            {
                updateResult = TestApplyDeltaInsert();

                if (updateResult != null && updateResult.Result == ResultType.Succeeded)
                {
                    List<long> gids = new List<long>();
                    foreach (KeyValuePair<long, long> kvp in updateResult.GlobalIdPairs)
                    {
                        gids.Add(kvp.Value);
                    }

                    updateResult = TestApplyDeltaUpdate(gids);
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Test apply delta: Insert - Update failed.\t{0}", ex.Message);

                if (updateResult != null)
                {
                    message += updateResult.ToString();
                }

                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
            }
        }

        private Dictionary<DMSType, ResourceDescription> CreateResourcesToInsert()
        {
            long globalId = 0;
            ResourceDescription rd = null;
            List<ModelCode> propertyIDs = null;
            Dictionary<DMSType, ResourceDescription> updates = new Dictionary<DMSType, ResourceDescription>(new DMSTypeComparer());

            #region Create resources

            foreach (DMSType type in modelResourcesDesc.AllDMSTypes)
            {
                if (type != DMSType.MASK_TYPE)
                {
                    globalId = ModelCodeHelper.CreateGlobalId(0, (short)type, -1);
                    propertyIDs = modelResourcesDesc.GetAllPropertyIds(modelResourcesDesc.GetModelCodeFromType(type));
                    rd = new ResourceDescription(globalId);

                    foreach (ModelCode propertyId in propertyIDs)
                    {
                        if (!modelResourcesDesc.NotSettablePropertyIds.Contains(propertyId))
                        {
                            switch (Property.GetPropertyType(propertyId))
                            {
                                case PropertyType.Bool:
                                    rd.AddProperty(new Property(propertyId, true));
                                    break;

                                case PropertyType.Byte:
                                    rd.AddProperty(new Property(propertyId, (byte)100));
                                    break;

                                case PropertyType.Int32:
                                    rd.AddProperty(new Property(propertyId, (int)4));
                                    break;

                                case PropertyType.Int64:
                                case PropertyType.TimeSpan:
                                case PropertyType.DateTime:
                                    rd.AddProperty(new Property(propertyId, (long)101));
                                    break;

                                case PropertyType.Enum:
                                    rd.AddProperty(new Property(propertyId, (short)1));
                                    break;

                                case PropertyType.Reference:
                                    rd.AddProperty(new Property(propertyId));
                                    break;

                                case PropertyType.Float:
                                    rd.AddProperty(new Property(propertyId, (float)10.5));
                                    break;

                                case PropertyType.String:
                                    rd.AddProperty(new Property(propertyId, "TestString"));
                                    break;

                                case PropertyType.Int64Vector:
                                    List<long> longVector = new List<long>();
                                    longVector.Add((long)10);
                                    longVector.Add((long)11);
                                    longVector.Add((long)12);
                                    longVector.Add((long)13);
                                    longVector.Add((long)14);
                                    longVector.Add((long)15);
                                    rd.AddProperty(new Property(propertyId, longVector));
                                    break;

                                case PropertyType.FloatVector:
                                    List<float> floatVector = new List<float>();
                                    floatVector.Add((float)11.1);
                                    floatVector.Add((float)12.2);
                                    floatVector.Add((float)13.3);
                                    floatVector.Add((float)14.4);
                                    floatVector.Add((float)15.5);
                                    rd.AddProperty(new Property(propertyId, floatVector));
                                    break;

                                case PropertyType.EnumVector:
                                    List<short> enumVector = new List<short>();
                                    enumVector.Add((short)1);
                                    enumVector.Add((short)2);
                                    enumVector.Add((short)3);
                                    rd.AddProperty(new Property(propertyId, enumVector));
                                    break;

                                case PropertyType.StringVector:
                                    List<string> stringVector = new List<string>();
                                    stringVector.Add("TestString1");
                                    stringVector.Add("TestString2");
                                    stringVector.Add("TestString3");
                                    stringVector.Add("TestString4");
                                    rd.AddProperty(new Property(propertyId, stringVector));
                                    break;

                                case PropertyType.Int32Vector:
                                    List<int> intVector = new List<int>();
                                    intVector.Add(11);
                                    intVector.Add(12);
                                    intVector.Add(13);
                                    intVector.Add(14);
                                    rd.AddProperty(new Property(propertyId, intVector));
                                    break;

                                default:
                                    break;
                            }
                        }
                    }

                    updates[type] = rd;
                }
            }

            #endregion Create resources

            #region Set references

            SetMarketDocumentReferences(updates);
            SetMeasurmentPointRefernces(updates);
            SetPeriodRefernces(updates);
            SetPointRefernces(updates);
            SetProcessRefernces(updates);
            SetTimeSeriesReferences(updates);
            

            #endregion Set references

            return updates;
        }

        private Dictionary<DMSType, ResourceDescription> CreateResourcesForUpdate(List<long> gids)
        {
            Dictionary<DMSType, ResourceDescription> updates = new Dictionary<DMSType, ResourceDescription>(new DMSTypeComparer());
            Delta delta = new Delta();

            ResourceDescription rd = null;
            List<ModelCode> propertyIDs = null;
            DMSType type;

            #region Creating resources

            foreach (long gid in gids)
            {
                type = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(gid);
                propertyIDs = modelResourcesDesc.GetAllPropertyIds(modelResourcesDesc.GetModelCodeFromType(type));
                rd = new ResourceDescription(gid);

                foreach (ModelCode propertyId in propertyIDs)
                {
                    if (!modelResourcesDesc.NotSettablePropertyIds.Contains(propertyId))
                    {
                        switch (Property.GetPropertyType(propertyId))
                        {
                            case PropertyType.Bool:
                                rd.AddProperty(new Property(propertyId, true));
                                break;

                            case PropertyType.Byte:
                                rd.AddProperty(new Property(propertyId, (byte)7));
                                break;

                            case PropertyType.Int32:
                                rd.AddProperty(new Property(propertyId, (int)500));
                                break;

                            case PropertyType.Int64:
                            case PropertyType.TimeSpan:
                            case PropertyType.DateTime:
                                rd.AddProperty(new Property(propertyId, (long)3112));
                                break;

                            case PropertyType.Enum:
                                rd.AddProperty(new Property(propertyId, (short)2));
                                break;

                            case PropertyType.Reference:
                                rd.AddProperty(new Property(propertyId, (long)0));
                                break;

                            case PropertyType.Float:
                                rd.AddProperty(new Property(propertyId, (float)1.05));
                                break;

                            case PropertyType.String:
                                rd.AddProperty(new Property(propertyId, "UpdateString"));
                                break;

                            case PropertyType.Int64Vector:
                                List<long> longVector = new List<long>();
                                longVector.Add((long)20);
                                longVector.Add((long)21);
                                longVector.Add((long)22);
                                longVector.Add((long)23);
                                longVector.Add((long)24);
                                longVector.Add((long)25);
                                rd.AddProperty(new Property(propertyId, longVector));
                                break;

                            case PropertyType.FloatVector:
                                List<float> floatVector = new List<float>();
                                floatVector.Add((float)21.1);
                                floatVector.Add((float)22.2);
                                floatVector.Add((float)23.3);
                                floatVector.Add((float)24.4);
                                floatVector.Add((float)25.5);
                                rd.AddProperty(new Property(propertyId, floatVector));
                                break;

                            case PropertyType.EnumVector:
                                List<short> enumVector = new List<short>();
                                enumVector.Add((short)44);
                                enumVector.Add((short)45);
                                enumVector.Add((short)46);
                                rd.AddProperty(new Property(propertyId, enumVector));
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (!updates.ContainsKey(type))
                {
                    updates.Add(type, rd);
                    delta.AddDeltaOperation(DeltaOpType.Update, rd, true);
                }
            }
            #endregion Creating resources

            #region Set references

            SetMarketDocumentReferences(updates);
            SetMeasurmentPointRefernces(updates);
            SetPeriodRefernces(updates);
            SetPointRefernces(updates);
            SetProcessRefernces(updates);
            SetTimeSeriesReferences(updates);

            #endregion Set references

            return updates;
        }

        #region set references

        private void SetMarketDocumentReferences(Dictionary<DMSType, ResourceDescription> updates)
        {
            for (int i = 0; i < updates[DMSType.MARKETDOCUMENT].Properties.Count; i++)
            {
                if (updates[DMSType.MARKETDOCUMENT].Properties[i].Id == ModelCode.MARKETDOCUMENT_PROCESS)
                {
                    updates[DMSType.MARKETDOCUMENT].Properties[i].SetValue(updates[DMSType.PROCESS].Id);
                }
            }
        }

        private void SetTimeSeriesReferences(Dictionary<DMSType, ResourceDescription> updates)
        {
            for (int i = 0; i < updates[DMSType.TIMESERIES].Properties.Count; i++)
            {
                if (updates[DMSType.TIMESERIES].Properties[i].Id == ModelCode.TIMESERIES_MARKETDOCUMENT)
                {
                    updates[DMSType.TIMESERIES].Properties[i].SetValue(updates[DMSType.MARKETDOCUMENT].Id);
                }

                if (updates[DMSType.TIMESERIES].Properties[i].Id == ModelCode.MARKETDOCUMENT_PROCESS)
                {
                    updates[DMSType.TIMESERIES].Properties[i].SetValue(updates[DMSType.PROCESS].Id);
                }

                if (updates[DMSType.TIMESERIES].Properties[i].Id == ModelCode.TIMESERIES_PERIOD)
                {
                    updates[DMSType.TIMESERIES].Properties[i].SetValue(updates[DMSType.PERIOD].Id);
                }
            }
        }

        private void SetMeasurmentPointRefernces(Dictionary<DMSType, ResourceDescription> updates)
        {
            for (int i = 0; i < updates[DMSType.MEASURMENTPOINT].Properties.Count; i++)
            {
                if (updates[DMSType.MEASURMENTPOINT].Properties[i].Id == ModelCode.MEASURMENTPOINT_TIMESERIES)
                {
                    updates[DMSType.MEASURMENTPOINT].Properties[i].SetValue(updates[DMSType.TIMESERIES].Id);
                }
            }
        }

        private void SetProcessRefernces(Dictionary<DMSType, ResourceDescription> updates)
        {
            for (int i = 0; i < updates[DMSType.PROCESS].Properties.Count; i++)
            {
                if (updates[DMSType.PROCESS].Properties[i].Id == ModelCode.MEASURMENTPOINT_TIMESERIES)
                {
                    //nema ref
                   // updates[DMSType.PROCESS].Properties[i].SetValue(updates[DMSType.TIMESERIES].Id);
                }
            }
        }

        private void SetPeriodRefernces(Dictionary<DMSType, ResourceDescription> updates)
        {
            for (int i = 0; i < updates[DMSType.PERIOD].Properties.Count; i++)
            {
                if (updates[DMSType.PERIOD].Properties[i].Id == ModelCode.PERIOD_MARKETDOCUMENT)
                {
                     updates[DMSType.PERIOD].Properties[i].SetValue(updates[DMSType.MARKETDOCUMENT].Id);
                }
            }
        }

        private void SetPointRefernces(Dictionary<DMSType, ResourceDescription> updates)
        {
            for (int i = 0; i < updates[DMSType.POINT].Properties.Count; i++)
            {
                if (updates[DMSType.POINT].Properties[i].Id == ModelCode.POINT_PERIOD)
                {
                    updates[DMSType.POINT].Properties[i].SetValue(updates[DMSType.PERIOD].Id);
                }
            }
        }

        #endregion set references

        #endregion GDAUpdate Service

        #endregion Test Methods

        public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
