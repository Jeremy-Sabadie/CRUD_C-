﻿using connectDB2.Properties;
using Dapper;
using MySql.Data.MySqlClient;

namespace connectDB2
{
    //This class is dedicated to the application's various database queries.
    internal class DBrequest
    {
        //This sqlConnection object ConectDB déclared as a global variable   variable is used to establish or close the connection to the database.
        MySqlConnection ConectDB;

        public DBrequest()
        {
            //Connection string stored in properties:
            ConectDB = new(Settings.Default.stringConnect);
        }
        public void OpenConnection()
        {
            ConectDB.Open();
        }
        //Function to read the list of users.
        public IEnumerable<User> GetUsers()
        {//Oprning the connection:
            ConectDB.Open();
            String request = "select * from utilisateurs";
            //List of all the users retrived during the query:
            var users = ConectDB.Query<User>(request);
            //Closing the connection:
            ConectDB.Close();
            return users;
        }
        //Function to create a new user.
        public int InsertUser(string nom, string prenom, DateTime dtNaiss)
        {
            try
            {
                ConectDB.Open();
                //New user u created:
                User u = new();
                //Assigning attribute name value to function argument.
                u.Nom = nom;
                //Tequest stored.
                var request = "insert into utilisateurs(nom, prenom, dtNaiss) values (@nom,@prenom,@dtNaiss);";
                //This Execute method wil return the number of created items.
                return ConectDB.Execute(request, new { nom, prenom, dtNaiss });
            }
            finally
            {
                ConectDB.Close();
            }
        }
        //Function to delete the selected user.
        public int DeleteUser(int id)
        {
            try
            {
                ConectDB.Open();

                string request = "DELETE FROM  utilisateurs WHERE Id = @id;";
                //This Execue method wil return the number of deleted items.
                var deletedRows = ConectDB.Execute(request, new { id });
                return deletedRows;
            }
            finally
            {
                ConectDB.Close();
            }
        }


        //Function to update the selected user.
        public int UpdateUser(int id, string nom, string prenom, DateTime dtNaiss, string currentNom, string currentPrenom, DateTime currentDtNaiss)
        {
            try
            {
                ////Open connection.
                ConectDB.Open();
                //Query for user creation 
                //Variable names with "@" are filled in via the application, thus avoiding SQL injection.
                var sql = "UPDATE db09.utilisateurs SET Nom = @Nom, Prenom=@Prenom, DtNaiss=@DtNaiss WHERE Id = @Id AND Nom = @currentNom AND Prenom=@currentPrenom AND DtNaiss=@currentDtNaiss;";
                //Effectue la commande "Execute" qui retourne le nombre de ligne modifier dans la BDD. 
                //Passage des paramétre qui iront automatiquement remplace les variables avec "@".
                return ConectDB.Execute(sql, new { id, nom, prenom, dtNaiss, currentNom, currentPrenom, currentDtNaiss });
            }
            finally
            {
                //Closing the connection even if the SQL query fails.
                ConectDB.Close();
            }
        }

    }

}

