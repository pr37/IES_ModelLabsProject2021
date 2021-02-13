using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class Conductor : ConductingEquipment
    {
        public Conductor(long globalId) : base(globalId)
        {
        }
        public ConductorMaterialType ConductorMaterial { get; set; }

        public int Length { get; set; }

        

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Conductor x = (Conductor)obj;
                return (x.ConductorMaterial == this.ConductorMaterial &&
                        x.Length == this.Length);
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
                case ModelCode.CONDUCTOR_MATERIAL:
                case ModelCode.CONDUCTOR_LEN:

                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.CONDUCTOR_MATERIAL:
                    property.SetValue((short)ConductorMaterial);
                    break;

                case ModelCode.CONDUCTOR_LEN:
                    property.SetValue(Length);
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
                case ModelCode.CONDUCTOR_MATERIAL:
                    ConductorMaterial = (ConductorMaterialType)property.AsEnum();
                    break;
                case ModelCode.CONDUCTOR_LEN:
                    Length = property.AsInt();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

    }
}
