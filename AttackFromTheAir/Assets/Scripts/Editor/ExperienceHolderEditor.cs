using Core.Resourses;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ExperienceHolder))]
public class ExperienceHolderEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    // Рисуем стандартный интерфейс
    //    DrawDefaultInspector();

    //    // Получаем ссылку на ExperienceHolder
    //    ExperienceHolder experienceHolder = (ExperienceHolder)target;

    //    // Проверяем и исправляем индексы для PlayerLevelExperienceList
    //    for (int i = 0; i < experienceHolder.PlayerLevelExperienceList.Count; i++)
    //    {
    //        experienceHolder.PlayerLevelExperienceList[i].Level = i + 1; // Последовательные уровни
    //    }

    //    // Проверяем и исправляем индексы для BaseLevelExperienceList
    //    for (int i = 0; i < experienceHolder.BaseLevelExperienceList.Count; i++)
    //    {
    //        experienceHolder.BaseLevelExperienceList[i].Level = i + 1; // Последовательные уровни
    //    }

    //    // Обновляем объект, если что-то изменилось
    //    if (GUI.changed)
    //    {
    //        EditorUtility.SetDirty(experienceHolder);
    //    }
    //}
}
