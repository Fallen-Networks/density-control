using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Fnrp.Fivem.Common.Client;

namespace DensitySuppression.Client
{
	public class Main : ClientScript
	{
		[Tick]
		internal async Task ClearAudio()
		{
			API.StartAudioScene("CHARACTER_CHANGE_IN_SKY_SCENE");
		}

		[Tick]
		internal async Task DisableDispatch()
		{
			for (int i = 0; i < 16; i++)
			{
				API.EnableDispatchService(i, false);
			}
			await BaseScript.Delay(1500);
		}

		[Tick]
		internal async Task ClearCops()
		{
			Ped p = Game.PlayerPed;
			Vector3 pos = p.Position;
			if (Entity.Exists(p))
			{
				API.ClearAreaOfCops(pos.X, pos.Y, pos.Z, 800f, 0);
			}
		}

		[Tick]
		internal async Task SuppressModels()
		{
			foreach (string model in this._modelsToSuppress)
			{
				API.SetVehicleModelIsSuppressed((uint)API.GetHashKey(model), true);
			}
			await Task.FromResult<int>(0);
		}

		[Tick]
		internal async Task DisableScenarioTypes()
		{
			foreach (string scenarioType in this._scenarioTypes)
			{
				API.SetScenarioTypeEnabled(scenarioType, false);
			}
			await BaseScript.Delay(3000);
		}

		[Tick]
		internal async Task DisableScenarioGroups()
		{
			foreach (string scenarioGroup in this._scenarioGroups)
			{
				API.SetScenarioGroupEnabled(scenarioGroup, false);
			}
			await BaseScript.Delay(3000);
		}

		[Tick]
		internal async Task DeleteBlacklistedVehicles()
		{
			World.GetAllVehicles().ToList<Vehicle>().ForEach(async delegate (Vehicle v)
			{
				if (this._modelsToSuppress.Any((string m) => API.GetHashKey(m) == v.Model.Hash) && Entity.Exists(v) && !v.PreviouslyOwnedByPlayer)
				{
					API.NetworkRequestControlOfEntity(v.Handle);
					int timeout = 5000;
					while (timeout > 0 && !API.NetworkHasControlOfEntity(v.Handle))
					{
						await BaseScript.Delay(100);
						timeout -= 100;
					}
					v.Delete();
				}
			});
			await BaseScript.Delay(100);
		}

		private readonly List<string> _modelsToSuppress = new List<string>
		{
			"police",
			"police2",
			"police3",
			"police4",
			"policeb",
			"policeold1",
			"policeold2",
			"policet",
			"polmav",
			"pranger",
			"sheriff",
			"sheriff2",
			"stockade3",
			"buffalo3",
			"fbi",
			"fbi2",
			"firetruk",
			"jester2",
			"lguard",
			"ambulance",
			"riot",
			"SHAMAL",
			"LUXOR",
			"LUXOR2",
			"JET",
			"LAZER",
			"TITAN",
			"BARRACKS",
			"BARRACKS2",
			"CRUSADER",
			"RHINO",
			"AIRTUG",
			"RIPLEY",
			"docktrailer",
			"trflat",
			"trailersmall",
			"boattrailer",
			"cargobob",
			"cargobob2",
			"cargobob3",
			"cargobob4",
			"volatus",
			"buzzard",
			"buzzard2",
			"besra"
		};

		private readonly List<string> _scenarioTypes = new List<string>
		{
			"WORLD_VEHICLE_POLICE_BIKE",
			"WORLD_VEHICLE_POLICE_CAR",
			"WORLD_VEHICLE_POLICE_NEXT_TO_CAR",
			"WORLD_VEHICLE_MILITARY_PLANES_SMALL",
			"WORLD_VEHICLE_MILITARY_PLANES_BIG",
			"CODE_HUMAN_POLICE_CROWD_CONTROL",
			"CODE_HUMAN_POLICE_INVESTIGATE",
			"WORLD_HUMAN_COP_IDLES"
		};

		private readonly List<string> _scenarioGroups = new List<string>
		{
			"FIB_GROUP_1",
			"FIB_GROUP_2",
			"MP_POLICE",
			"ARMY_HELI",
			"POLICE_POUND1",
			"POLICE_POUND2",
			"POLICE_POUND3",
			"POLICE_POUND4",
			"POLICE_POUND5",
			"SANDY_PLANES",
			"LSA_Planes",
			"GRAPESEED_PLANES",
			"Grapeseed_Planes",
			"ALAMO_PLANES",
			"ng_planes"
		};
	}
}
