using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DatabaseManager: MonoBehaviour {

    //Metodo utilizzato per inserire un nuovo account nel database
    public static bool InsertTherapist(string username, string password)
    {
        //Controlla se il nome è già inserito
        if (CheckIfAlreadyExist(username))
            return false;

        //Effettuiamo la query di inserimento
        string connectionString = "URI=file:" + Application.dataPath + "/EmotionPuzzleDB.s3db"; //Path to database
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //Open connection to the database
            dbConnection.Open();
            IDbCommand dbCommand = dbConnection.CreateCommand();
            string sqlQuery = string.Format("INSERT INTO Therapists (Username, Password) VALUES (\"{0}\", \"{1}\")", username, password);
            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnection.Close();
        }
        return true;
    }

    public static void InsertResult(string username, string emotion, string game, int result)
    {
        //Effettuiamo la query di inserimento
        string connectionString = "URI=file:" + Application.dataPath + "/EmotionPuzzleDB.s3db"; //Path to database
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //Open connection to the database
            dbConnection.Open();
            IDbCommand dbCommand = dbConnection.CreateCommand();
            string sqlQuery = string.Format("INSERT INTO Results (Name, Emotion, Game, Result) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\")", username, emotion, game, result);
            dbCommand.CommandText = sqlQuery;
            dbCommand.ExecuteScalar();
            dbConnection.Close();
        }
    }

    //Metodo che vede se nel database è presente uno username con una data password
    public static bool GetTherapist(string username, string password)
    {
        bool flag = false;
        string connectionString = "URI=file:" + Application.dataPath + "/EmotionPuzzleDB.s3db"; //Path to database
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //Open connection to the database
            dbConnection.Open();
            IDbCommand dbCommand = dbConnection.CreateCommand();
            string sqlQuery = string.Format("SELECT * FROM Therapists WHERE username = (\"{0}\") AND password = (\"{1}\")", username, password);
            dbCommand.CommandText = sqlQuery;
            IDataReader reader = dbCommand.ExecuteReader();

            if (reader.Read())
            {
                Debug.Log("Ho trovato " + reader.GetString(0) + " " + reader.GetString(1));
                flag = true;
            }
            dbConnection.Close();
        }
        return flag;
    }
    
    //Metodo utilizzato per vedere se nel database è gia presente un certo username: return true se il nome esiste già
    public static bool CheckIfAlreadyExist(string username)
    {
        bool flag = false ;
        string connectionString = "URI=file:" + Application.dataPath + "/EmotionPuzzleDB.s3db"; //Path to database
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //Open connection to the database
            dbConnection.Open();
            IDbCommand dbCommand = dbConnection.CreateCommand();
            string sqlQuery = string.Format("SELECT * FROM Therapists WHERE Username = (\"{0}\")", username);
            dbCommand.CommandText = sqlQuery;
            IDataReader reader = dbCommand.ExecuteReader();
            if (reader.Read())
            {
                flag = true;
            }
            dbConnection.Close();
        }
        return flag;
    }

    public static int GetTotalErrorsByEmotion(string username, string emotion)
    {
        int total = 0;
        string connectionString = "URI=file:" + Application.dataPath + "/EmotionPuzzleDB.s3db"; //Path to database
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //Open connection to the database
            dbConnection.Open();
            IDbCommand dbCommand = dbConnection.CreateCommand();
            string sqlQuery = string.Format("SELECT count(*) FROM Results WHERE Name = (\"{0}\") AND Emotion = (\"{1}\") AND Result = 0", username, emotion);
            dbCommand.CommandText = sqlQuery;
            IDataReader reader = dbCommand.ExecuteReader();
            if (reader.Read())
            {
                total = reader.GetInt32(0);
            }
            dbConnection.Close();
        }
        return total;
    }

    public static int GetTotalRoundsByEmotion(string username, string emotion)
    {
        int total = 0;
        string connectionString = "URI=file:" + Application.dataPath + "/EmotionPuzzleDB.s3db"; //Path to database
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //Open connection to the database
            dbConnection.Open();
            IDbCommand dbCommand = dbConnection.CreateCommand();
            string sqlQuery = string.Format("SELECT count(*) FROM Results WHERE Name = (\"{0}\") AND Emotion = (\"{1}\")", username, emotion);
            dbCommand.CommandText = sqlQuery;
            IDataReader reader = dbCommand.ExecuteReader();
            if (reader.Read())
            {
                total = reader.GetInt32(0);
            }
            dbConnection.Close();
        }
        return total;
    }

    public static int GetTotalErrorsByGame(string username, string game)
    {
        int total = 0;
        string connectionString = "URI=file:" + Application.dataPath + "/EmotionPuzzleDB.s3db"; //Path to database
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //Open connection to the database
            dbConnection.Open();
            IDbCommand dbCommand = dbConnection.CreateCommand();
            string sqlQuery = string.Format("SELECT count(*) FROM Results WHERE Name = (\"{0}\") AND Game = (\"{1}\") AND Result = 0", username, game);
            dbCommand.CommandText = sqlQuery;
            IDataReader reader = dbCommand.ExecuteReader();
            if (reader.Read())
            {
                total = reader.GetInt32(0);
            }
            dbConnection.Close();
        }
        return total;
    }

    public static int GetTotalRoundsByGame(string username, string game)
    {
        int total = 0;
        string connectionString = "URI=file:" + Application.dataPath + "/EmotionPuzzleDB.s3db"; //Path to database
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //Open connection to the database
            dbConnection.Open();
            IDbCommand dbCommand = dbConnection.CreateCommand();
            string sqlQuery = string.Format("SELECT count(*) FROM Results WHERE Name = (\"{0}\") AND Game = (\"{1}\")", username, game);
            dbCommand.CommandText = sqlQuery;
            IDataReader reader = dbCommand.ExecuteReader();
            if (reader.Read())
            {
                total = reader.GetInt32(0);
            }
            dbConnection.Close();
        }
        return total;
    }



    public static int GetTotalErrors(string username)
    {
        int total = 0;
        string connectionString = "URI=file:" + Application.dataPath + "/EmotionPuzzleDB.s3db"; //Path to database
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //Open connection to the database
            dbConnection.Open();
            IDbCommand dbCommand = dbConnection.CreateCommand();
            string sqlQuery = string.Format("SELECT count(*) FROM Results WHERE Name = (\"{0}\") AND Result = 0", username);
            dbCommand.CommandText = sqlQuery;
            IDataReader reader = dbCommand.ExecuteReader();
            if (reader.Read())
            {
                Debug.Log("Count: " + reader.GetInt32(0));
                total = reader.GetInt32(0);
            }
            dbConnection.Close();
        }
        return total;
    }

    //Metodo utilizzato per vedere se nel database è gia presente un certo username: return true se il nome esiste già
    public static bool CheckIfAlreadyExistInResult(string username)
    {
        bool flag = false;
        string connectionString = "URI=file:" + Application.dataPath + "/EmotionPuzzleDB.s3db"; //Path to database
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //Open connection to the database
            dbConnection.Open();
            IDbCommand dbCommand = dbConnection.CreateCommand();
            string sqlQuery = string.Format("SELECT * FROM Results WHERE Name = (\"{0}\")", username);
            dbCommand.CommandText = sqlQuery;
            IDataReader reader = dbCommand.ExecuteReader();
            if (reader.Read())
            {
                flag = true;
            }
            dbConnection.Close();
        }
        return flag;
    }


}
