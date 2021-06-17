using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TowersWebsocketNet31.Server.Game.EquipmentData;
using TowersWebsocketNet31.Server.Game.Mechanics;
using TowersWebsocketNet31.Server.Game.SpellData;
using Utils;

namespace TowersWebsocketNet31.Server.Game.EntityData
{
    public class ClassesWeaponSpell
    {
        public int id { get; set; } = 0;
        public Classes classes { get; set; }
        public CategoryWeapon categoryWeapon { get; set; }
        public Spell spell1 { get; set; }
        public Spell spell2 { get; set; }
        public Spell spell3 { get; set; }
    }
    
    public class ClassesList
    {
        public List<Classes> classes;
        public List<ClassesWeaponSpell> classesWeaponSpell;

        public ClassesList(string jsonObject)
        {
            classes = new List<Classes>();
            classesWeaponSpell = new List<ClassesWeaponSpell>();

            InitClassesFromJson(jsonObject);
        }

        private void InitClassesFromJson(string classesJson)
        {
            try
            {
                ClassesListJsonObject classesListJson = JsonSerializer.Deserialize<ClassesListJsonObject>(classesJson);

                foreach (ClassesJsonObject classesJsonObject in classesListJson.classes)
                {
                    classes.Add(classesJsonObject.ConvertToClasses());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                Console.WriteLine(classesJson);
                Console.WriteLine(e.Message);
            }
        }

        public Classes GetClassesFromId(int id)
        {
            return classes.Find(classe => classe.id == id);
        }

        public Classes GetFirstClasses()
        {
            return classes.First();
        }

        public List<ClassesWeaponSpell> GetSpellCategoryForClasses(Classes classes)
        {
            return classesWeaponSpell.FindAll(data => data.classes.id == classes.id);
        }
        
        public List<Spell> GetSpellForClassesAndCategory(CategoryWeapon categoryWeapon, Classes classes)
        {
            List<Spell> spells = new List<Spell>();

            ClassesWeaponSpell classesWeaponSpell = this.classesWeaponSpell.Find(data =>
                data.classes.id == classes.id && data.categoryWeapon.id == categoryWeapon.id);

            if (classesWeaponSpell != null)
            {
                spells.Add(classesWeaponSpell.spell1);
                spells.Add(classesWeaponSpell.spell2);
                spells.Add(classesWeaponSpell.spell3);
            }
            
            return spells;
        }
        
        public void InitClassesCategorySpells(string json)
        {
            try
            {
                ClassesWeaponSpellJsonList classesListJson = JsonSerializer.Deserialize<ClassesWeaponSpellJsonList>(json);

                foreach (ClassesWeaponSpellJson classesCategorySpell in classesListJson.classesCategory)
                {
                    classesWeaponSpell.Add(classesCategorySpell.Convert());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                Console.WriteLine(json);
                Console.WriteLine(e.Message);
            }
        }
    }
}