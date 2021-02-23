using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.MarketManagement
{
   public  class Point : DataModel.Core.IdentifiedObject
    {
		private int position = 0;
		private long period = 0;

        public int Position { get => position; set => position = value; }
        public long Period { get => period; set => period = value; }

        //TODO bidQuantity i quantity

        public Point(long globalId) : base(globalId)
		{
		}



		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				Point x = (Point)obj;
				return (x.position == this.position &&
						x.period == this.period
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
				case ModelCode.POINT_POSITION:
				case ModelCode.POINT_PERIOD:
				//case ModelCode.POINT_BIDQUANTITY: TODO Odkoment
				//case ModelCode.POINT_QUANTITY:
					return true;

				default:
					return base.HasProperty(t);
			}
		}

		public override void GetProperty(Property prop)
		{
			switch (prop.Id)
			{
				case ModelCode.POINT_POSITION:
					prop.SetValue(position);
					break;

				case ModelCode.POINT_PERIOD:
					prop.SetValue(period);
					break;
/*	TODO
				case ModelCode.POINT_BIDQUANTITY:
					prop.SetValue(bidQuantity);
					break;

				case ModelCode.POINT_QUANTITY:
					prop.SetValue(quantity);
					break;
*/


				default:
					base.GetProperty(prop);
					break;
			}
		}

		public override void SetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.POINT_POSITION:
					position = property.AsInt();
					break;

				case ModelCode.POINT_PERIOD:
					period = property.AsLong();
					break;
/*	TODO
				case ModelCode.POINT_QUANTITY:
					timeSeries = property.AsLongs();
					break;

				case ModelCode.POINT_BIDQUANTITY:
					timeSeries = property.AsLongs();
					break;
*/
				

				default:
					base.SetProperty(property);
					break;
			}
		}

		#endregion IAccess implementation

		#region IReference implementation	

		public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
		{
			//TODO DODAJ OVO U SVE KLASE KOJE IMAJU BAR JEDAN LONG
			if (period != 0 && (refType != TypeOfReference.Reference || refType != TypeOfReference.Both))
			{
				references[ModelCode.POINT_PERIOD] = new List<long>();
				references[ModelCode.POINT_PERIOD].Add(period);
			}

			base.GetReferences(references, refType);
		}

		#endregion IReference implementation

	}
}
