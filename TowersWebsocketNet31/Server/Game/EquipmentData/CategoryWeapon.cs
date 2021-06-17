using System;
using System.Collections.Generic;
using System.Text.Json;
using TowersWebsocketNet31.Server.Game.Mechanics;
using TowersWebsocketNet31.Server.Game.SpellData;

namespace TowersWebsocketNet31.Server.Game.EquipmentData
{
    public class CategoryWeaponList
    {
        public List<CategoryWeapon> categories;

        public CategoryWeaponList(string json)
        {
            categories = new List<CategoryWeapon>();
            InitCategoriesFromJson(json);
        }

        public CategoryWeapon GetCategoryFromId(int id)
        {
            CategoryWeapon categoryWeapon = categories?.Find(c => c.id == id);

            return categoryWeapon != null ? Utils.Clone(categoryWeapon) : null;
        }

        private void InitCategoriesFromJson(string json)
        {
            try
            {
                CategoryWeaponListJsonObject categoryWeaponListJsonObject = JsonSerializer.Deserialize<CategoryWeaponListJsonObject>(json);

                foreach (CategoryWeaponJsonObject categoryWeaponJsonObject in categoryWeaponListJsonObject.categories)
                {
                    categories.Add(categoryWeaponJsonObject.ConvertToCategoryWeapon());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                Console.WriteLine(json);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Data);
            }
        }
    }
    
    public class CategoryWeapon
    {
        public int id { get; set; }
        public string name { get; set; }
        public Spell spellAttack { get; set; }
        
        public override string ToString()
        {
            return name;
        }
    }
}