using Core.Resourses;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpriteHolder))]
public class SpriteHolderEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    SpriteHolder spriteHolder = (SpriteHolder)target;

    //    // Отображаем заголовки для каждого списка и добавляем кнопку для добавления элементов
    //    GUILayout.Label("Infantry Sprites", EditorStyles.boldLabel);
    //    DrawUnitSpritesList(spriteHolder.InfantrySprites, typeof(UnitSpriteInfantryType));

    //    GUILayout.Label("Medium Equipment Sprites", EditorStyles.boldLabel);
    //    DrawUnitSpritesList(spriteHolder.MediumEquipmentSprites, typeof(UnitSpriteMediumEquipmentType));

    //    GUILayout.Label("Heavy Equipment Sprites", EditorStyles.boldLabel);
    //    DrawUnitSpritesList(spriteHolder.HeavyEquipmentSprites, typeof(UnitSpriteHeavyEquipmentType));


    //    GUILayout.Label("Resource Sprites", EditorStyles.boldLabel);
    //    DrawResourceSpritesList(spriteHolder.ResourceSprites);

    //    GUILayout.Label("Effet Sprite", EditorStyles.boldLabel);
    //    DrawEffectSpritesList(spriteHolder.EffectSprites);

    //    // Сохранение изменений
    //    if (GUI.changed)
    //    {
    //        EditorUtility.SetDirty(spriteHolder);
    //    }
    //}

    //private void DrawUnitSpritesList<T>(List<T> unitSprites, System.Type type) where T : class, new()
    //{
    //    if (unitSprites == null)
    //    {
    //        unitSprites = new List<T>();
    //    }

    //    // Отображаем элементы списка
    //    for (int i = 0; i < unitSprites.Count; i++)
    //    {
    //        GUILayout.BeginHorizontal();
    //        var unitTypeField = unitSprites[i].GetType().GetField("UnitType");
    //        var spriteField = unitSprites[i].GetType().GetField("UnitSpriteImage");

    //        if (unitTypeField != null && spriteField != null)
    //        {
    //            object unitType = unitTypeField.GetValue(unitSprites[i]);
    //            Sprite sprite = (Sprite)spriteField.GetValue(unitSprites[i]);

    //            // Отображаем тип юнита
    //            unitType = EditorGUILayout.EnumPopup("Unit Type", (System.Enum)unitType);
    //            unitTypeField.SetValue(unitSprites[i], unitType);

    //            // Отображаем спрайт
    //            sprite = (Sprite)EditorGUILayout.ObjectField("Unit Sprite Image", sprite, typeof(Sprite), false);
    //            spriteField.SetValue(unitSprites[i], sprite);
    //        }

    //        // Добавляем кнопку для удаления элемента
    //        if (GUILayout.Button("Remove", GUILayout.Width(60)))
    //        {
    //            unitSprites.RemoveAt(i);
    //        }

    //        GUILayout.EndHorizontal();
    //    }

    //    // Кнопка для добавления нового элемента
    //    if (GUILayout.Button("Add Item"))
    //    {
    //        unitSprites.Add(new T());
    //    }
    //}

    //private void DrawResourceSpritesList(List<ResourceSpriteData> resourceSprites)
    //{
    //    if (resourceSprites == null)
    //    {
    //        resourceSprites = new List<ResourceSpriteData>();
    //    }

    //    // Отображаем элементы списка
    //    for (int i = 0; i < resourceSprites.Count; i++)
    //    {
    //        GUILayout.BeginHorizontal();
    //        resourceSprites[i].ResourceType = (ResourceType)EditorGUILayout.EnumPopup("Resource Type", resourceSprites[i].ResourceType);
    //        resourceSprites[i].ResourceSpriteImage = (Sprite)EditorGUILayout.ObjectField("Resource Sprite Image", resourceSprites[i].ResourceSpriteImage, typeof(Sprite), false);

    //        // Добавляем кнопку для удаления элемента
    //        if (GUILayout.Button("Remove", GUILayout.Width(60)))
    //        {
    //            resourceSprites.RemoveAt(i);
    //        }

    //        GUILayout.EndHorizontal();
    //    }

    //    // Кнопка для добавления нового элемента
    //    if (GUILayout.Button("Add Resource"))
    //    {
    //        resourceSprites.Add(new ResourceSpriteData());
    //    }
    //}

    //private void DrawEffectSpritesList(List<EffectSpriteData> effectSprites)
    //{
    //    if (effectSprites == null)
    //    {
    //        effectSprites = new List<EffectSpriteData>();
    //    }

    //    for (int i = 0; i < effectSprites.Count; i++)
    //    {
    //        GUILayout.BeginVertical("Box");

    //        // Отображаем поле для выбора типа эффекта
    //        effectSprites[i].EffectResourType =
    //            (EffectType)EditorGUILayout.EnumPopup("Effect Type", effectSprites[i].EffectResourType);

    //        // Отображаем список спрайтов
    //        GUILayout.Label("Sprites", EditorStyles.boldLabel);
    //        for (int j = 0; j < effectSprites[i].ResourceSpriteImage.Count; j++)
    //        {
    //            GUILayout.BeginHorizontal();

    //            // Редактируем конкретный спрайт
    //            effectSprites[i].ResourceSpriteImage[j] =
    //                (Sprite)EditorGUILayout.ObjectField("Sprite", effectSprites[i].ResourceSpriteImage[j], typeof(Sprite), false);

    //            // Кнопка для удаления спрайта из списка
    //            if (GUILayout.Button("Remove", GUILayout.Width(60)))
    //            {
    //                effectSprites[i].ResourceSpriteImage.RemoveAt(j);
    //                break;
    //            }

    //            GUILayout.EndHorizontal();
    //        }

    //        // Кнопка для добавления нового спрайта в текущий эффект
    //        if (GUILayout.Button("Add Sprite"))
    //        {
    //            effectSprites[i].ResourceSpriteImage.Add(null);
    //        }

    //        // Кнопка для удаления всего эффекта
    //        if (GUILayout.Button("Remove Effect", GUILayout.Width(120)))
    //        {
    //            effectSprites.RemoveAt(i);
    //            break;
    //        }

    //        GUILayout.EndVertical();
    //    }

    //    // Кнопка для добавления нового типа эффекта
    //    if (GUILayout.Button("Add Effect"))
    //    {
    //        effectSprites.Add(new EffectSpriteData
    //        {
    //            EffectResourType = default,
    //            ResourceSpriteImage = new List<Sprite>()
    //        });
    //    }
    //}

}
