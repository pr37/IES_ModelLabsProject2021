using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class ACLineSegment //: Conductor
    {/*
        public ACLineSegment(long globalId) : base(globalId)
        {
        }

        public bool Feeder { get; set; }

        public float R { get; set; }



        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                ACLineSegment x = (ACLineSegment)obj;
                return (x.Feeder == this.Feeder &&
                        x.R == this.R);
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


        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.ACLS_FEEDERCABLE:
                case ModelCode.ACLS_R:

                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ACLS_FEEDERCABLE:
                    property.SetValue((bool)Feeder);
                    break;

                case ModelCode.ACLS_R:
                    property.SetValue(R);
                    break;

                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ACLS_FEEDERCABLE:
                    Feeder = property.AsBool();
                    break;
                case ModelCode.ACLS_R:
                    R = property.AsFloat();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        } */
    }
}
