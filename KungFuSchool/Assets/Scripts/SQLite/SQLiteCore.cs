﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;

public static class SQLiteCore {
    public static string connectString = "URI=file:" + Application.dataPath + "/KungFuSchool.db";
    public static IDbConnection conn = new SqliteConnection(connectString);
    public static List<LevelManager> levelManager = getLevelManager();
    public static int LastAddedItem = -1;
    public static void Connect() {
        if (conn.State != ConnectionState.Open) {
            conn.Open();
        }
    }

    public static Player getPlayerInfor() {
        Player player = new Player();
        string query = "Select * From RootPlayer";
        Connect();
        try {
            using (conn) {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    IDataReader datas = cmd.ExecuteReader();
                    while (datas.Read()) {
                        player.Level = Convert.ToInt32(datas[0]);
                        player.Exp = Convert.ToInt32(datas[1]);
                        player.Gold = Convert.ToInt32(datas[2]);
                        player.Str = Convert.ToInt32(datas[3]);
                        player.Agi = Convert.ToInt32(datas[4]);
                        player.Sta = Convert.ToInt32(datas[5]);
                        player.Intel = Convert.ToInt32(datas[6]);
                        player.RemainPoint = Convert.ToInt32(datas[7]);
                    }
                    return player;
                }
            }
        } catch (Exception excp) {
            Debug.Log("ERROR:getPlayerInfor " + excp);
            return null;
        }
    }
    public static void UpdatePlayer(Player player){
        string query = "UPDATE RootPlayer SET Level = " + player.Level + ", " +
            "EXP = " + player.Exp + ", " +
            "Gold = " + player.Gold + ", " +
            "Str = " + player.Str + ", " +
            "Agi = " + player.Agi + ", " +
            "Sta = " + player.Sta + ", " +
            "Int = " + player.Intel + ", " +
            "RemainPoint = " + player.RemainPoint + 
            " WHERE id = 1";
        Connect();
        try
        {
            using (conn)
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception excp)
        {
            Debug.Log("ERROR:UpdatePlayer " + excp);
        }
    }

    public static List<LevelManager> getLevelManager()
    {
        string query = "Select * From Level";
        Connect();
        List<LevelManager> levels = new List<LevelManager>();
        try
        {
            using (conn)
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    IDataReader datas = cmd.ExecuteReader();
                    while (datas.Read())
                    {
                        LevelManager level = new LevelManager();
                        level.Level = Convert.ToInt32(datas[0]);
                        level.Exp = Convert.ToInt32(datas[1]);
                        levels.Add(level);
                    }

                    return levels;
                }
            }
        }
        catch (Exception excp)
        {
            Debug.Log("ERROR:getLevelManager " + excp);
            return levels;
        }
    }

    public static List<Item> getItemInInvengory()
    {   
        string query = "Select * From Inventory WHERE isEquiping = 0";
        Connect();
        List<Item> items = new List<Item>();
        try
        {
            using (conn)
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    IDataReader datas = cmd.ExecuteReader();
                    while (datas.Read())
                    {
                        Item item = ScriptableObject.CreateInstance<Item>();
                        item.id = Convert.ToInt32(datas[0]);
                        item.name = Convert.ToString(datas[1]);
                        string itemIcon = Convert.ToString(datas[2]);
                        item.icon = Resources.Load<Sprite>(itemIcon);
                        item.details = Convert.ToString(datas[3]);
                        item.itemType = (Item.ItemType)Enum.Parse(typeof(Item.ItemType), Convert.ToString(datas[10]));
                        item.isEquiping = false; 
                        if (Convert.ToInt32(datas[11]) == 0) { item.isStacking = false; } else { item.isStacking = true; }
                        item.count = Convert.ToInt32(datas[12]);
                        item.priceSell = Convert.ToInt32(datas[13]);
                        item.itemColor = (Item.ItemColor)Enum.Parse(typeof(Item.ItemColor), Convert.ToString(datas[14]));
                        if ((int)item.itemType == 0) {
                            item.equipSlot = (Item.EquipmentSlot)Enum.Parse(typeof(Item.EquipmentSlot), Convert.ToString(datas[4]));
                            Option op = ScriptableObject.CreateInstance<Option>();
                            op.title = Convert.ToString(datas[5]);
                            op.unit = Convert.ToString(datas[7]);
                            op.tag = (StatManager.TagManager)Enum.Parse(typeof(StatManager.TagManager), Convert.ToString(datas[8]));
                            item.defaultOption = op;
                            item.defaultPoint = Convert.ToInt32(datas[6]);
                        }
                        items.Add(item);
                    }

                    return items;
                }
            }
        }
        catch (Exception excp)
        {
            Debug.Log("ERROR:getItemInInvengory " + excp);
            return items;
        }
    }

    public static List<Item> getEquipingitem()
    {
        string query = "Select * From Inventory WHERE isEquiping = 1";
        Connect();
        List<Item> items = new List<Item>();
        try
        {
            using (conn)
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    IDataReader datas = cmd.ExecuteReader();
                    while (datas.Read())
                    {
                        Item item = ScriptableObject.CreateInstance<Item>();
                        item.id = Convert.ToInt32(datas[0]);
                        item.name = Convert.ToString(datas[1]);
                        string itemIcon = Convert.ToString(datas[2]);
                        item.icon = Resources.Load<Sprite>(itemIcon);
                        item.details = Convert.ToString(datas[3]);
                        item.equipSlot = (Item.EquipmentSlot)Enum.Parse(typeof(Item.EquipmentSlot), Convert.ToString(datas[4]));
                        item.itemColor = (Item.ItemColor)Enum.Parse(typeof(Item.ItemColor), Convert.ToString(datas[14]));
                        Option op = ScriptableObject.CreateInstance<Option>();
                        op.title = Convert.ToString(datas[5]);
                        op.unit = Convert.ToString(datas[7]);
                        op.tag = (StatManager.TagManager)Enum.Parse(typeof(StatManager.TagManager), Convert.ToString(datas[8]));
                        item.priceSell = Convert.ToInt32(datas[13]);
                        item.defaultOption = op;
                        item.defaultPoint = Convert.ToInt32(datas[6]);
                        item.isEquiping = true;
                        item.itemType = Item.ItemType.Equipment;
                        item.isStacking = false;
                        items.Add(item);
                    }

                    return items;
                }
            }
        }
        catch (Exception excp)
        {
            Debug.Log("ERROR:getItemInInvengory " + excp);
            return items;
        }
    }

    public static List<Option> getOptionItem(int itemID)
    {
        string query = "Select * From ItemOption WHERE itemID = "+itemID;
        Connect();
        List<Option> options = new List<Option>();
        try
        {
            using (conn)
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    IDataReader datas = cmd.ExecuteReader();
                    while (datas.Read())
                    {
                        Option op = ScriptableObject.CreateInstance<Option>();
                        op.tag = (StatManager.TagManager)Enum.Parse(typeof(StatManager.TagManager), Convert.ToString(datas[2]));
                        op.title = Convert.ToString(datas[3]);
                        op.point = Convert.ToInt16(datas[4]);
                        op.unit = Convert.ToString(datas[5]);
                        options.Add(op);
                    }

                    return options;
                }
            }
        }
        catch (Exception excp)
        {
            Debug.Log("ERROR:getOptionItem " + excp);
            return options;
        }
    }

    public static int AddItemToInventory(Item item)
    {
        int isStacking = 0;
        if (item.isStacking) { isStacking = 1; }
        string query;
        Debug.Log(item.defaultOption.tag + "");
        if ((int)item.itemType == 0) {
            query = String.Format("INSERT INTO Inventory (id,name,icon,detail,itemColor,equipSlot,defaultOptionTitle,defaultOptionPoint,defaultOptionUnit,defaultOptionTag,itemType,isStacking,count,price)" +
                " VALUES (NULL,'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')",
                item.name, item.icon.name, item.details,item.itemColor+"", item.equipSlot + "", item.defaultOption.title, item.defaultPoint, item.defaultOption.unit, item.defaultOption.tag + "", item.itemType + "",isStacking, 1,item.priceSell);
        }
        else
        {
            query = String.Format("INSERT INTO Inventory (id,name,icon,detail,itemColor,itemType,isStacking,count,price)" +
                " VALUES (NULL,'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                item.name, item.icon.name, item.details,item.itemColor, item.itemType + "",isStacking, item.count,item.priceSell);
        }
        
        Connect();
        try
        {
            using (conn)
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "select last_insert_rowid()";
                    Int64 LastRowID64 = (Int64)cmd.ExecuteScalar();
                    LastAddedItem = (int)LastRowID64;
                    if ((int)item.itemType == 0 && item.options!=null && LastAddedItem != -1)
                    {
                        Debug.Log("Save Option");
                        for (int i = 0; i < item.options.Length; i++) {
                            AddItemOption(LastAddedItem,item.points[i],item.options[i]);
                        }
                    }
                    return LastAddedItem;
                }
            }
        }
        catch (Exception excp)
        {
            Debug.Log("ERROR:AddItemToInventory " + excp);
            return -1;
        }
    }

    public static void AddItemOption(int itemID, int point, Option option)
    {
        string query = String.Format("INSERT INTO ItemOption (id,itemID,tag,title,point,unit) VALUES (NULL,'{0}','{1}','{2}','{3}','{4}')", itemID, option.tag+ "", option.title, point,option.unit);
        Connect();
        try
        {
            using (conn)
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception excp)
        {
            Debug.Log("ERROR:AddItemOption " + excp);
        }
    }

    public static bool UpdateItem(Item item)
    {
        string query = "UPDATE Inventory SET count = " + item.count + " WHERE id = " + item.id + ";";
        Connect();
        try
        {
            using (conn)
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
        }
        catch (Exception excp)
        {
            Debug.Log("ERROR:UpdateItem " + excp);
            return false;
        }
    }

    public static void UpdateEquipment(Item item)
    {
        int isEquiping = 0;
        if (item.isEquiping) { isEquiping = 1; }
        string query = "UPDATE Inventory SET isEquiping = " + isEquiping + " WHERE id = "+ item.id + ";";
        Connect();
        try
        {
            using (conn)
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception excp)
        {
            Debug.Log("ERROR:UpdateEquipMent " + excp);
        }
    }

    public static bool RemoveItem(int itemID)
    {
        string query = "DELETE FROM Inventory WHERE ID = " + itemID;
        Connect();
        try
        {
            using (conn)
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
        }
        catch (Exception excp)
        {
            Debug.Log("ERROR:RemoveItem " + excp);
            return false;
        }
    }
    public static bool RemoveOptionItem(int itemID)
    {
        string query = "DELETE FROM ItemOption WHERE itemID = " + itemID;
        Connect();
        try
        {
            using (conn)
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
        }
        catch (Exception excp)
        {
            Debug.Log("ERROR:RemoveOptionItem " + excp);
            return false;
        }
    }
}
