using System.Collections.Generic;
using Core.Buildings;
using Core.GameLogic;
using Core.Resourses;
using Core.Units;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitsHolder))]
public class UnitsHolderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UnitsHolder unitsHolder = (UnitsHolder)target;

        // Отображаем список юнитов
        if (unitsHolder.Units == null)
        {
            unitsHolder.Units = new List<UnitDescriptor>();
        }

        //// Добавляем кнопку для добавления нового юнита
        //if (GUILayout.Button("Add Unit"))
        //{
        //    unitsHolder.Units.Add(new UnitDescriptor());
        //}

        // Для каждого юнита в списке рисуем интерфейс
        for (int i = 0; i < unitsHolder.Units.Count; i++)
        {
            var unit = unitsHolder.Units[i];

            EditorGUILayout.LabelField($"Unit {i + 1}", EditorStyles.boldLabel);

            // Выбор типа юнита (Infantry, MediumEquipment, HeavyEquipment)
            unit.Type = (UnitSpawnerType)EditorGUILayout.EnumPopup("Type", unit.Type);

            // В зависимости от типа юнита отображаем нужные поля
            switch (unit.Type)
            {
                case UnitSpawnerType.Infantry:
                    unit.Infantry = (InfantryType)EditorGUILayout.EnumPopup("Infantry Type", unit.Infantry);
                    break;
                case UnitSpawnerType.MediumEquipment:
                    unit.MediumEquipment = (MediumEquipmentType)EditorGUILayout.EnumPopup("Medium Equipment Type", unit.MediumEquipment);
                    break;
                case UnitSpawnerType.HeavyEquipment:
                    unit.HeavyEquipment = (HeavyEquipmentType)EditorGUILayout.EnumPopup("Heavy Equipment Type", unit.HeavyEquipment);
                    break;
            }

            // Поле для выбора префаба юнита
            unit.Prefab = (BaseUnitView)EditorGUILayout.ObjectField("Prefab", unit.Prefab, typeof(BaseUnitView), false);

            // Добавляем кнопку для удаления юнита
            if (GUILayout.Button("Remove Unit"))
            {
                unitsHolder.Units.RemoveAt(i);
            }

            EditorGUILayout.Space();
        }


        // Добавляем кнопку для добавления нового юнита
        if (GUILayout.Button("Add Unit"))
        {
            unitsHolder.Units.Add(new UnitDescriptor());
        }

        // Если что-то изменилось, сохраняем изменения
        if (GUI.changed)
        {
            EditorUtility.SetDirty(unitsHolder);
        }
    }
}



