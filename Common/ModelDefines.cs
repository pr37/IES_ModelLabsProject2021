using System;
using System.Collections.Generic;
using System.Text;

namespace FTN.Common
{
	
	public enum DMSType : short
	{		
		MASK_TYPE							= unchecked((short)0xFFFF),

		//BASEVOLTAGE							= 0x0001,
		//LOCATION							= 0x0002,
		//POWERTR								= 0x0003,
		//POWERTRWINDING						= 0x0004,
		//WINDINGTEST							= 0x0005,
		MARKETDOCUMENT							= 0x0001,
		PROCESS									= 0x0002,
		PERIOD									= 0x0003,
		POINT									= 0x0004,
		TIMESERIES								= 0x0005,
		MEASURMENTPOINT							= 0x0006
	}

    [Flags]
	public enum ModelCode : long
	{
		IDOBJ								= 0x1000000000000000,
		IDOBJ_GID							= 0x1000000000000104,
		IDOBJ_ALIASNAME						= 0x1000000000000207,
		IDOBJ_MRID							= 0x1000000000000307,
		IDOBJ_NAME							= 0x1000000000000407,
		
		DOCUMENT							= 0x1100000000000000,
		DOCUMENT_CREATEDATETIME				= 0x1100000000000108,
		DOCUMENT_DOCSTATUS					= 0x1100000000000209,
		DOCUMENT_ELECTRONICADDRESS			= 0x1100000000000309,
		DOCUMENT_LASTMODIFIEDTIME			= 0x1100000000000408,
		DOCUMENT_REVISIONNUMBER				= 0x1100000000000507,
		DOCUMENT_STATUS						= 0x1100000000000609,
		DOCUMENT_SUBJECT					= 0x1100000000000707,
		DOCUMENT_TITLE						= 0x1100000000000807,
		DOCUMENT_TYPE						= 0x1100000000000907,

		MEASURMENTPOINT						= 0x1200000000060000,
		MEASURMENTPOINT_TIMESERIES			= 0x1200000000060119,

		MARKETDOCUMENT						= 0x1110000000010000,
		MARKETDOCUMENT_PROCESS				= 0x1110000000010109,
		MARKETDOCUMENT_PERIOD				= 0x1110000000010209,
		MARKETDOCUMENT_TIMESERIES			= 0x1110000000010319,

		TIMESERIES							= 0x1300000000050000,
		TIMESERIES_OBJECTAGGREGATION		= 0x1300000000050107,
		TIMESERIES_PRODUCT					= 0x1300000000050207,
		TIMESERIES_VERSION					= 0x1300000000050307,
		TIMESERIES_MARKETDOCUMENT			= 0x1300000000050409,
		TIMESERIES_MEASURMENTPOINT			= 0x1300000000050519,
		TIMESERIES_PERIOD					= 0x1300000000050609,

		PROCESS								= 0x1400000000020000,
		PROCESS_CLASSIFICATIONTYPE			= 0x1400000000020107,
		PROCESS_PROCESSTYPE					= 0x1400000000020207,
		PROCESS_MARKETDOCUMENTS				= 0x1400000000020319,

		PERIOD								= 0x1500000000030000,
		PERIOD_RESOLUTION					= 0x1500000000030109,
		PERIOD_MARKETDOCUMENT				= 0x1500000000030209,
		PERIOD_TIMESERIES					= 0x1500000000030319,
		PERIOD_POINT						= 0x1500000000030419,

		POINT								= 0x1600000000040000,
		POINT_BIDQUANTITY					= 0x1600000000040105,
		POINT_POSITION						= 0x1600000000040203,
		POINT_QUANTITY						= 0x1600000000040305,
		POINT_PERIOD						= 0x1600000000040409


		//PSR									= 0x1100000000000000,
		//PSR_CUSTOMTYPE						= 0x1100000000000107,
		//PSR_LOCATION						= 0x1100000000000209,

		//BASEVOLTAGE							= 0x1200000000010000,
		//BASEVOLTAGE_NOMINALVOLTAGE			= 0x1200000000010105,
		//BASEVOLTAGE_CONDEQS					= 0x1200000000010219,

		//LOCATION							= 0x1300000000020000,
		//LOCATION_CORPORATECODE				= 0x1300000000020107,
		//LOCATION_CATEGORY					= 0x1300000000020207,
		//LOCATION_PSRS						= 0x1300000000020319,

		//WINDINGTEST							= 0x1400000000050000,
		//WINDINGTEST_LEAKIMPDN				= 0x1400000000050105,
		//WINDINGTEST_LOADLOSS				= 0x1400000000050205,
		//WINDINGTEST_NOLOADLOSS				= 0x1400000000050305,
		//WINDINGTEST_PHASESHIFT				= 0x1400000000050405,
		//WINDINGTEST_LEAKIMPDN0PERCENT		= 0x1400000000050505,
		//WINDINGTEST_LEAKIMPDNMINPERCENT		= 0x1400000000050605,
		//WINDINGTEST_LEAKIMPDNMAXPERCENT		= 0x1400000000050705,
		//WINDINGTEST_POWERTRWINDING			= 0x1400000000050809,
		/*
		EQUIPMENT							= 0x1110000000000000,
		EQUIPMENT_ISUNDERGROUND				= 0x1110000000000101,
		EQUIPMENT_ISPRIVATE					= 0x1110000000000201,		

		CONDEQ								= 0x1111000000000000,
		CONDEQ_PHASES						= 0x111100000000010a,
		CONDEQ_RATEDVOLTAGE					= 0x1111000000000205,
		CONDEQ_BASVOLTAGE					= 0x1111000000000309,

		POWERTR								= 0x1112000000030000,
		POWERTR_FUNC						= 0x111200000003010a,
		POWERTR_AUTO						= 0x1112000000030201,
		POWERTR_WINDINGS					= 0x1112000000030319,

		POWERTRWINDING						= 0x1111100000040000,
		POWERTRWINDING_CONNTYPE				= 0x111110000004010a,
		POWERTRWINDING_GROUNDED				= 0x1111100000040201,
		POWERTRWINDING_RATEDS				= 0x1111100000040305,
		POWERTRWINDING_RATEDU				= 0x1111100000040405,
		POWERTRWINDING_WINDTYPE				= 0x111110000004050a,
		POWERTRWINDING_PHASETOGRNDVOLTAGE	= 0x1111100000040605,
		POWERTRWINDING_PHASETOPHASEVOLTAGE	= 0x1111100000040705,
		POWERTRWINDING_POWERTRW				= 0x1111100000040809,
		POWERTRWINDING_TESTS				= 0x1111100000040919,

		CONDUCTOR = 0x1111200000060000,
		CONDUCTOR_MATERIAL = 0x111120000006010a,
		CONDUCTOR_LEN = 0x1111200000060204,

		ACLS = 0x1111210000070000,
		ACLS_FEEDERCABLE = 0x1111210000070101,
		ACLS_R = 0x1111210000070205,
		*/

	}

    [Flags]
	public enum ModelCodeMask : long
	{
		MASK_TYPE			 = 0x00000000ffff0000,
		MASK_ATTRIBUTE_INDEX = 0x000000000000ff00,
		MASK_ATTRIBUTE_TYPE	 = 0x00000000000000ff,

		MASK_INHERITANCE_ONLY = unchecked((long)0xffffffff00000000),
		MASK_FIRSTNBL		  = unchecked((long)0xf000000000000000),
		MASK_DELFROMNBL8	  = unchecked((long)0xfffffff000000000),		
	}																		
}


