using System.Collections.Generic;
using Core.Buildings;
using Core.GameLogic;
using Core.Resourses;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitSpawnersHolder))]
public class UnitSpawnersHolderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UnitSpawnersHolder spawnerHolder = (UnitSpawnersHolder)target;

        // Если список спавнеров не инициализирован
        if (spawnerHolder.Spawners == null)
        {
            spawnerHolder.Spawners = new List<UnitSpawnerDescriptor>();
        }

        // Отображаем каждый спавнер в списке
        for (int i = 0; i < spawnerHolder.Spawners.Count; i++)
        {
            var spawner = spawnerHolder.Spawners[i];

            EditorGUILayout.LabelField($"Spawner {i + 1}", EditorStyles.boldLabel);

            // Выбор типа спавнера
            spawner.Type = (UnitSpawnerType)EditorGUILayout.EnumPopup("Type", spawner.Type);

            // В зависимости от типа спавнера, отображаем соответствующие поля
            switch (spawner.Type)
            {
                case UnitSpawnerType.Infantry:
                    spawner.Infantry = (InfantryType)EditorGUILayout.EnumPopup("Infantry Type", spawner.Infantry);
                    break;
                case UnitSpawnerType.MediumEquipment:
                    spawner.MediumEquipment = (MediumEquipmentType)EditorGUILayout.EnumPopup("Medium Equipment Type", spawner.MediumEquipment);
                    break;
                case UnitSpawnerType.HeavyEquipment:
                    spawner.HeavyEquipment = (HeavyEquipmentType)EditorGUILayout.EnumPopup("Heavy Equipment Type", spawner.HeavyEquipment);
                    break;
            }

            // Поле для выбора префаба
            spawner.Prefab = (BaseUnitSpawnerView)EditorGUILayout.ObjectField("Prefab", spawner.Prefab, typeof(BaseUnitSpawnerView), false);

            // Кнопка для удаления спавнера
            if (GUILayout.Button("Remove Spawner"))
            {
                spawnerHolder.Spawners.RemoveAt(i);
            }

            EditorGUILayout.Space();
        }

        // Кнопка для добавления нового спавнера
        if (GUILayout.Button("Add Spawner"))
        {
            spawnerHolder.Spawners.Add(new UnitSpawnerDescriptor());
        }


        // Если есть изменения, сохраняем их
        if (GUI.changed)
        {
            EditorUtility.SetDirty(spawnerHolder);
        }
    }
}
