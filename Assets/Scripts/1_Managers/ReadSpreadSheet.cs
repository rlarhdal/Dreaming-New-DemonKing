using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public enum sheetType
{
    Enemy,
    Dialogue
}

public class ReadSpreadSheet
{
    public readonly string adress = "https://docs.google.com/spreadsheets/d/189qqcrkBljcPWWYn-_7m1JIb0vEo0q1GcQbklTAAahI";
    public readonly string range = "A2:J";
    public readonly int sheetID = 658248956;

    public List<EnemyStatus> enemys;

    // key      > ����   ���� ��Ʈ ����
    // value    > ���������Ʈ ������ (ó���� ��ũ)
    private Dictionary<Type, string> sheetDatas = new Dictionary<Type, string>();
    private Type currentType;

    public void Init()
    {
        // ��ųʸ��� Ű�� �߰��Ѵ�.
        sheetDatas.Add(typeof(EnemyStatus), GetCSVAddress(adress, range, sheetID));
    }

    public IEnumerator LoadData(sheetType type)
    {
        switch (type)
        {
            case sheetType.Enemy:
                currentType = typeof(EnemyStatus);
                break;
            case sheetType.Dialogue:
                //currentType = typeof(EnemyStatus);
                break;
        }

        foreach (KeyValuePair<Type, string> kv in sheetDatas)
        {
            if (kv.Key == currentType)
            {
                UnityWebRequest www = UnityWebRequest.Get(sheetDatas[currentType]);
                yield return www.SendWebRequest();

                // ��ųʸ��� value �� ����
                sheetDatas[currentType] = www.downloadHandler.text;

                if (currentType == typeof(EnemyStatus))
                {
                    enemys = GetDatasAsChildren<EnemyStatus>(sheetDatas[currentType]);

                    foreach (EnemyStatus enemyStatus in enemys)
                    {
                        // ����� enemy ������ ó��

                        // Resource�� ������Ʈ�� �ش� ���� ����
                        GameObject SpawnEnemy = Resources.Load<GameObject>($"Enemy/{enemyStatus.name}");
                        SpawnEnemy.name = enemyStatus.name;
                        SpawnEnemy.GetComponentInChildren<Enemy>().status = enemyStatus;
                        Managers.Pool.CreatePool(SpawnEnemy, 20);
                    }
                }
                break;
            }
        }
    }

    //public IEnumerator LoadData()
    //{
    //    List<Type> sheetTypes = new List<Type>(sheetDatas.Keys);

    //    foreach (Type type in sheetTypes)
    //    {
    //        UnityWebRequest www = UnityWebRequest.Get(sheetDatas[type]);
    //        yield return www.SendWebRequest();

    //         ��ųʸ��� value �� ����
    //        sheetDatas[type] = www.downloadHandler.text;

    //        if (type == typeof(EnemyStatus))
    //        {
    //            enemys = GetDatasAsChildren<EnemyStatus>(sheetDatas[type]);

    //            foreach (EnemyStatus enemyStatus in enemys)
    //            {
    //                 ����� enemy ������ ó��

    //                 Resource�� ������Ʈ�� �ش� ���� ����
    //                GameObject SpawnEnemy = Resources.Load<GameObject>($"Enemy/{enemyStatus.name}");
    //                SpawnEnemy.name = enemyStatus.name;
    //                SpawnEnemy.GetComponentInChildren<Enemy>().status = enemyStatus;
    //                Managers.Pool.CreatePool(SpawnEnemy, 20);
    //            }
    //        }
    //    }
    //}

    public static string GetCSVAddress(string address, string range, long sheetID)
    {
        return $"{address}/export?format=tsv&range={range}&gid={sheetID}";
    }

    List<T> GetDatasAsChildren<T>(string data)
    {
        List<T> dataList = new List<T>();

        // ���� ���� ���� �迭�� ���� (���, �ɲ� �� �� ������ �迭�� �����Ѵٴ� ��)
        string[] splitedData = data.Split('\n');

        // ������ �迭�� ���� ����
        foreach (string element in splitedData)
        {
            // �ش� ��ü�� ������ �迭�� ����(����� ü��, ���ݷ� �� ������ �迭�� ����)
            string[] datas = element.Split('\t');
            dataList.Add(GetData<T>(datas, datas[0]));
        }

        return dataList;
    }

    T GetData<T>(string[] datas, string typeName = "")
    {
        object data;

        // childType�� ����ְų� �׷� Type�� ���� ��
        if (string.IsNullOrEmpty(typeName) || Type.GetType(typeName) == null)
        {
            data = Activator.CreateInstance(typeof(T));
        }
        else
        {
            data = Activator.CreateInstance(Type.GetType(typeName));
        }

        // Ŭ������ �ִ� �������� ������� ������ �迭 (������� ������ �ǹǷ� ���������Ʈ�� Ŭ������ ������ ���ƾ���)
        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        for (int i = 0; i < datas.Length; i++)
        {
            // �ʵ��� ������ �ν��Ͻ� data�� ����
            try
            {
                // string > parse
                // EnemyStatus�� ������ �������� ������� Ÿ���� Ȯ��
                Type type = fields[i].FieldType;

                if (string.IsNullOrEmpty(datas[i])) continue;

                if (type == typeof(int))
                    fields[i].SetValue(data, int.Parse(datas[i]));

                else if (type == typeof(float))
                    fields[i].SetValue(data, float.Parse(datas[i]));

                else if (type == typeof(bool))
                    fields[i].SetValue(data, bool.Parse(datas[i]));

                else if (type == typeof(string))
                    fields[i].SetValue(data, datas[i]);

                // enum
                else
                    fields[i].SetValue(data, Enum.Parse(type, datas[i]));
            }

            catch (Exception e)
            {
                Debug.LogError($"SpreadSheet Error : {e.Message}");
            }
        }

        return (T)data;
    }
}
