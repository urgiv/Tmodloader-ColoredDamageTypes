using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using log4net;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Localization;

namespace ColoredDamageTypes
{
	class ColoredDamageTypes : Mod
	{

		public static bool ChangeTooltipColor = true;
		public static bool ChangeDamageColor = true;
		public static ColoredDamageTypes instance;
		public static string GithubUserName { get { return "PvtFudgepants"; } }
		public static string GithubProjectName { get { return "tModLoader---Colored-Damage-Types"; } }

		public static Item[] ProjectileWeaponSpawns = new Item[1001];

		public ColoredDamageTypes()
		{
			instance = this;
		}


		public static void Log(object message, params object[] formatData)
		{
			instance.Logger.InfoFormat("[ColoredDamageTypes] " + string.Format(message.ToString(), formatData), "ColoredDamageTypes");
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI) {
			//Main.NewText("Got packet");
			if ( Main.netMode == NetmodeID.Server ) {
				ModPacket packet = instance.GetPacket();
				packet.Write(reader.ReadByte());
				packet.Write(reader.ReadInt32());
				packet.Write(reader.ReadSingle());
				packet.Write(reader.ReadByte());
				packet.Send(-1, whoAmI);
				packet.Close();
			}
			if (Main.netMode == NetmodeID.MultiplayerClient ) {
				Netcode.recentcolor_in = reader.ReadByte();
				Netcode.recentdmg_in = reader.ReadInt32();
				Netcode.recentkb_in = reader.ReadSingle();
				Netcode.recentcrit_in = reader.ReadByte();
			}
		}
		public override object Call(params object[] args)
		{
			//coloreddamagetypes.Call(ModContent.GetInstance<ExampleDamageClass>(), new Color(255, 0, 0), new Color(0, 255, 0), new Color(0, 0, 255));

			if (args[0] is DamageClass dc)
            {
				if(args[1] is Color ttcolor && args[2] is Color dmgcolor && args[3] is Color critdmgcolor)
                {
					string dcname = dc.ToString();
					zCrossModConfig.DamageType dt = new zCrossModConfig.DamageType(dcname, ttcolor, dmgcolor, critdmgcolor);
					zCrossModConfig.CrossModDamageConfig_Orig.Add(dcname, dt);

					DamageTypes.DamageClasses.Add(dc);
				}
				if (args[1] is (int r1, int g1, int b1) && args[2] is (int r2, int g2, int b2) && args[3] is (int r3, int g3, int b3))
				{
					string dcname = dc.ToString();
					zCrossModConfig.DamageType dt = new zCrossModConfig.DamageType(dcname, new Color(r1,g1,b1), new Color(r2,g2,b2), new Color(r3,g3,b3));
					zCrossModConfig.CrossModDamageConfig_Orig.Add(dcname, dt);

					DamageTypes.DamageClasses.Add(dc);
				}
			}

			return this;
		}
	}
}
