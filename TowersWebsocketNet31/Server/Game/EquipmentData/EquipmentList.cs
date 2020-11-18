using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Games.Global.Armors;

namespace TowersWebsocketNet31.Server.Game.EquipmentData
{
    public class EquipmentList
    {
        public List<Weapon> weapons;
        public List<Armor> armors;

        public EquipmentList(string jsonObject)
        {
            weapons = new List<Weapon>();
            armors = new List<Armor>();
            InitEquipmentDictionnary(jsonObject);
        }

        public Weapon GetWeaponWithName(string findName)
        {
            Weapon findingWeapon = weapons.First(we => we.equipmentName == findName);
            Weapon cloneWeapon = Utils.Clone(findingWeapon);
            return cloneWeapon;
        }

        public Weapon GetWeaponWithId(int id)
        {
            Weapon findingWeapon = weapons.First(we => we.id == id);
            Weapon cloneWeapon = Utils.Clone(findingWeapon);
            return cloneWeapon;
        }
        
        public Armor GetArmorWithName(string findName)
        {
            Armor findingWeapon = armors.First(ar => ar.equipmentName == findName);
            Armor cloneWeapon = Utils.Clone(findingWeapon);
            return cloneWeapon;
        }

        public Armor GetArmorWithId(int id)
        {
            Armor findingWeapon = armors.First(we => we.id == id);
            Armor cloneWeapon = Utils.Clone(findingWeapon);
            return cloneWeapon;
        }

        public void PrintDictionnary()
        {
            foreach (Weapon weapon in weapons)
            {
                Console.WriteLine(weapon);
                Console.WriteLine(weapon.category);
                Console.WriteLine(weapon.damage);
            }
        }

        private void InitEquipmentDictionnary(string json)
        {
            try
            {
                EquipmentJsonList equipmentList = JsonSerializer.Deserialize<EquipmentJsonList>(json);
                foreach (EquipmentJsonObject equipmentJsonObject in equipmentList.equipment)
                {
                    EquipmentType type = (EquipmentType) Int32.Parse(equipmentJsonObject.equipmentType);

                    if (type == EquipmentType.WEAPON)
                    {
                        Weapon loadedWeapon = equipmentJsonObject.ConvertToWeapon();

                        weapons.Add(loadedWeapon);
                    }
                    else
                    {
                        Armor loadedArmor = equipmentJsonObject.ConvertToArmor();

                        armors.Add(loadedArmor);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error at the creation of EquipmentList");
                Console.WriteLine(json);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Data);
            }
        }
    }
}