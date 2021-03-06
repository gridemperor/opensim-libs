using System;

using libsecondlife;
using libsecondlife.AssetSystem;

namespace libsecondlife.InventorySystem
{
	/// <summary>
	/// Summary description for InventoryNotecard.
	/// </summary>
	public class InventoryWearable : InventoryItem
	{
        // Wearable Inventory/Asset constants
        public enum WearableType : sbyte { Clothing = 5, Body = 13 };


        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="folderID"></param>
        /// <param name="uuidOwnerCreater"></param>
        internal InventoryWearable(InventoryManager manager, string name, string description, LLUUID folderID, LLUUID uuidOwnerCreater, WearableType wtype) 
			: base(manager, name, description, folderID, 18, (sbyte)wtype, uuidOwnerCreater)
		{

		}

        /// <summary>
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="ii"></param>
        internal InventoryWearable(InventoryManager manager, InventoryItem ii)
			: base( manager, ii._Name, ii._Description, ii._FolderID, ii._InvType, ii._Type, ii._CreatorID)
		{
			if( (ii.InvType != 18) )
			{
				throw new Exception("The InventoryItem cannot be converted to a Wearable, wrong InvType.");
			}

			this.iManager = manager;
            this._ItemID = ii._ItemID;
			this._Asset = ii._Asset;
			this._AssetID = ii._AssetID;
			this._BaseMask = ii._BaseMask;
			this._CRC = ii._CRC;
			this._CreationDate = ii._CreationDate;
			this._EveryoneMask = ii._EveryoneMask;
			this._Flags = ii._Flags;
			this._GroupID = ii._GroupID;
			this._GroupMask = ii._GroupMask;
			this._GroupOwned = ii._GroupOwned;
			this._InvType = ii._InvType;
			this._NextOwnerMask = ii._NextOwnerMask;
			this._OwnerID = ii._OwnerID;
			this._OwnerMask = ii._OwnerMask;
			this._SalePrice = ii._SalePrice;
			this._SaleType = ii._SaleType;
			this._Type = ii._Type;
		}

        override internal void SetAssetData(byte[] assetData)
        {
            // Must reference all variables by internal _private names, so as not to trigger accessor code
            if (_Asset == null)
            {
                if (_AssetID != null)
                {
                    if (_Type == (sbyte)WearableType.Clothing)
                    {
                        _Asset = new AssetWearable_Clothing(_AssetID, assetData);
                    }
                    else
                    {
                        _Asset = new AssetWearable_Body(_AssetID, assetData);
                    }
                }
                else
                {
                    if (_Type == (sbyte)WearableType.Clothing)
                    {
                        _Asset = new AssetWearable_Clothing(LLUUID.Random(), assetData);
                    }
                    else
                    {
                        _Asset = new AssetWearable_Body(LLUUID.Random(), assetData);
                    }
                    _AssetID = _Asset.AssetID;
                }
            }
            else
            {
                _Asset.SetAssetData(assetData);
            }
        }

        public override string GetDisplayType()
        {
            return "Wearable_" + ((_Type == (sbyte)WearableType.Clothing) ? "Clothing" : "Body");;
        }


        /// <summary>
        /// Output this image as XML
        /// </summary>
        /// <param name="outputAssets">Include an asset data as well, TRUE/FALSE</param>
        override public string toXML(bool outputAssets)
        {
            string output = "<wearable ";

            output += "name = '" + xmlSafe(Name) + "' ";
            output += "uuid = '" + ItemID + "' ";

            if (Type == (sbyte)WearableType.Body)
            {
                output += "wearabletype = 'Body' ";
            }
            else if (Type == (sbyte)WearableType.Clothing)
            {
                output += "wearabletype = 'Clothing' ";
            }
            else
            {
                output += "wearabletype = 'Unknown' ";
            }

            output += "invtype = '" + InvType + "' ";
            output += "type = '" + Type + "' ";



            output += "description = '" + xmlSafe(Description) + "' ";
            output += "crc = '" + CRC + "' ";
            
            output += "ownerid = '" + OwnerID + "' ";
            output += "creatorid = '" + CreatorID + "' ";

            output += "assetid = '" + AssetID + "' ";
            output += "groupid = '" + GroupID + "' ";

            output += "groupowned = '" + GroupOwned + "' ";
            output += "creationdate = '" + CreationDate + "' ";
            output += "flags = '" + Flags + "' ";

            output += "saletype = '" + SaleType + "' ";
            output += "saleprice = '" + SalePrice + "' ";
            output += "basemask = '" + BaseMask + "' ";
            output += "everyonemask = '" + EveryoneMask + "' ";
            output += "nextownermask = '" + NextOwnerMask + "' ";
            output += "groupmask = '" + GroupMask + "' ";
            output += "ownermask = '" + OwnerMask + "' ";

            output += ">\n";

            if (outputAssets)
            {
                output += xmlSafe(base.Asset.AssetDataToString());
            }

            output += "</image>";


            return output;
        }

	}
}
