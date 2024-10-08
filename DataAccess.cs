using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ConsoleAppVisuals;
using ConsoleAppVisuals.AnimatedElements;
using ConsoleAppVisuals.InteractiveElements;
using ConsoleAppVisuals.PassiveElements;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Anssi;

namespace Projet_SouidiCazac
{
    public class DataAccess
    {
        MySqlConnection connexion;

        /// <summary>
        /// Constructeur de la classe DataAccess
        /// </summary>
        /// <param name="user"> Nom de l'utilisateur de connexion a la BDD </param>
        /// <param name="password"> Mot de passe de connexion a la BDD </param>
        public DataAccess(string user, string password)
        {
            bool connected = false;

            do
            {
                try
                {
                    string connectionString =
                        $"SERVER=localhost;PORT=3306;DATABASE=VeloMax;UID={user};PASSWORD={password};";
                    connexion = new MySqlConnection(connectionString);
                    connexion.Open();

                    Dialog txt;
                    if (user == "root")
                    {
                        txt = new Dialog(
                            new List<string>()
                            {
                                "Bienvenue dans la base de données VeloMax !",
                                "Vous êtes connecté en tant qu'administrateur"
                            },
                            null,
                            "ok"
                        );
                    }
                    else
                    {
                        txt = new Dialog(
                            new List<string>()
                            {
                                "Bienvenue dans la base de données VeloMax !",
                                "Vous êtes connecté en tant que visiteur"
                            },
                            null,
                            "ok"
                        );
                    }

                    Window.AddElement(txt);
                    Window.ActivateElement(txt);

                    connected = true; // La connexion a réussi, sortie de la boucle
                }
                catch (Exception ex)
                {
                    Dialog txtCon = new Dialog(
                        new List<string>()
                        {
                            "Erreur lors de la connexion à la base de données.",
                            ex.Message
                        },
                        null,
                        "ok"
                    );
                    Window.AddElement(txtCon);
                    Window.ActivateElement(txtCon);

                    // Redemander les informations d'identification
                    Prompt askUser = new Prompt("Entrez votre nom d'utilisateur :");
                    Window.AddElement(askUser);
                    Window.ActivateElement(askUser);
                    user = askUser.GetResponse().Value;

                    Prompt askPassword = new Prompt("Entrez votre mot de passe :");
                    Window.AddElement(askPassword);
                    Window.ActivateElement(askPassword);
                    password = askPassword.GetResponse().Value;
                }
            } while (!connected);
        }

        /// <summary>
        /// Méthode pour terminer la connexion à la base de données
        /// </summary>
        public void Terminer()
        {
            connexion.Close();
            Dialog txt = new Dialog(new List<string>() { "Connexion terminée.", }, null, "ok");

            Window.AddElement(txt);
            Window.ActivateElement(txt);

            FakeLoadingBar loadingBar = new FakeLoadingBar();
            Window.AddElement(loadingBar);
            Window.Render(loadingBar);
            Window.ActivateElement(loadingBar);
        }

        /// <summary>
        /// Méthode pour avoir la liste des attributs d'une table
        /// </summary>
        /// <param name="table"> table qu'on veut etudier </param>
        /// <returns> liste des attributs </returns>
        public List<string> GetColumnsNames(string table)
        {
            List<string> columns = new List<string>();
            string query = $"SHOW COLUMNS FROM VeloMax.{table};";
            MySqlCommand command = new MySqlCommand(query, connexion);
            MySqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    columns.Add(reader.GetString(0));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la récupération des colonnes : " + ex.Message);
            }
            reader.Close();
            command.Dispose();
            return columns;
        }

        /// <summary>
        /// Méthode pour avoir la liste des types des attributs d'une table
        /// </summary>
        /// <param name="table"> table qu'on veut etudier </param>
        /// <returns> liste des types des attributs </returns>
        public List<string> GetColumnsType(string table)
        {
            List<string> columns = new List<string>();
            string query = $"SHOW COLUMNS FROM VeloMax.{table};";
            MySqlCommand command = new MySqlCommand(query, connexion);
            MySqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    columns.Add(reader.GetString(1));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la récupération des colonnes : " + ex.Message);
            }
            reader.Close();
            command.Dispose();
            return columns;
        }

        /// <summary>
        /// Méthode generique qui permet de faire des insertions dans une table
        /// </summary>
        /// <param name="table"> table dans laquelle on veut insérer des données </param>
        /// <param name="primaryKey"> si la table a une clé primaire ou non </param>
        /// <returns> Insertion </returns>
        public void createDataTable(string table, bool primaryKey)
        {
            try
            {
                Prompt prompt;
                List<string> ColumnsNames = GetColumnsNames(table);
                List<string> ColumnsType = GetColumnsType(table);

                MySqlCommand command = new MySqlCommand($"SELECT * FROM {table}", connexion);
                MySqlDataReader reader = command.ExecuteReader();
                List<List<string>> data = new List<List<string>>();
                while (reader.Read())
                {
                    List<string> row = new List<string>();
                    for (int i = 0; i < ColumnsNames.Count; i++)
                    {
                        object value = reader.GetValue(i);
                        string stringValue;
                        if (value is int)
                        {
                            stringValue = ((int)value).ToString(); // Convertir l'entier en chaîne de caractères
                        }
                        else
                        {
                            stringValue = value.ToString(); // Convertir la valeur en chaîne de caractères
                        }
                        row.Add(stringValue);
                    }
                    data.Add(row);
                }
                reader.Close();
                command.Dispose();

                TableView tableview = new TableView($"liste {table}", ColumnsNames, data);
                Window.AddElement(tableview);
                Window.ActivateElement(tableview);

                if (!primaryKey)
                {
                    ColumnsNames.RemoveAt(0);
                    ColumnsType.RemoveAt(0);
                }

                string query =
                    $"INSERT INTO {table} ({string.Join(",", ColumnsNames)}) VALUES ({string.Join(",", ColumnsNames.ConvertAll(c => "@" + c))})";
                command = new MySqlCommand(query, connexion);

                for (int i = 0; i < ColumnsNames.Count; i++)
                {
                    bool inputIsValid = false;

                    do
                    {
                        if (ColumnsType[i] == "date")
                        {
                            prompt = new Prompt(
                                $"Entrez {ColumnsNames[i]} de type ({ColumnsType[i]} sous le format yyyy-mm-dd) :"
                            );
                        }
                        else
                        {
                            prompt = new Prompt(
                                $"Entrez {ColumnsNames[i]} de type ({ColumnsType[i]}) :"
                            );
                        }

                        Window.AddElement(prompt);
                        Window.ActivateElement(prompt);

                        string value = prompt.GetResponse().Value;

                        if (string.IsNullOrEmpty(value))
                        {
                            command.Parameters.AddWithValue("@" + ColumnsNames[i], DBNull.Value);
                            inputIsValid = true;
                        }
                        else if (ColumnsType[i] == "int")
                        {
                            inputIsValid = int.TryParse(value, out int result);
                            if (inputIsValid)
                            {
                                command.Parameters.AddWithValue("@" + ColumnsNames[i], result);
                            }
                            else
                            {
                                Dialog txt = new Dialog(
                                    new List<string>() { "Veuillez entrer un entier valide.", },
                                    null,
                                    "ok"
                                );
                                Window.AddElement(txt);
                                Window.ActivateElement(txt);
                            }
                        }
                        else if (ColumnsType[i] == "decimal")
                        {
                            inputIsValid = decimal.TryParse(value, out decimal result);
                            if (inputIsValid)
                            {
                                command.Parameters.AddWithValue("@" + ColumnsNames[i], result);
                            }
                            else
                            {
                                Dialog txt = new Dialog(
                                    new List<string>() { "Veuillez entrer un decimal valide.", },
                                    null,
                                    "ok"
                                );
                                Window.AddElement(txt);
                                Window.ActivateElement(txt);
                            }
                        }
                        else if (ColumnsType[i] == "date")
                        {
                            inputIsValid = DateTime.TryParse(value, out DateTime result);
                            if (inputIsValid)
                            {
                                command.Parameters.AddWithValue("@" + ColumnsNames[i], result);
                            }
                            else
                            {
                                Dialog txt = new Dialog(
                                    new List<string>() { "Veuillez entrer une date valide.", },
                                    null,
                                    "ok"
                                );
                                Window.AddElement(txt);
                                Window.ActivateElement(txt);
                            }
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@" + ColumnsNames[i], value);
                            inputIsValid = true;
                        }
                    } while (!inputIsValid);
                }
                int rowsAffected = command.ExecuteNonQuery();
                Dialog Finaltxt = new Dialog(
                    new List<string>() { $"{rowsAffected} lignes affectées.", },
                    null,
                    "ok"
                );
                Window.AddElement(Finaltxt);
                Window.ActivateElement(Finaltxt);
                Window.Freeze();
                Window.RemoveElement(tableview);
                Window.Render();
            }
            catch (Exception ex)
            {
                Dialog txt = new Dialog(
                    new List<string>() { "Erreur lors de l'insertion des données.", ex.Message },
                    null,
                    "ok"
                );
                Window.AddElement(txt);
                Window.ActivateElement(txt);
            }
        }

        /// <summary>
        /// Methode qui permet de creer une commande
        /// </summary>
        /// <param name="table"> table dans laquelle on veut insérer des données </param>
        /// <returns> Insertion </returns>
        public void askCommmand(string table)
        {
            List<string> columns = GetColumnsNames(table);
            List<string> types = GetColumnsType(table);
            try
            {

                Prompt prompt;
                string query = $"SELECT * FROM {table}";

                MySqlCommand command = new MySqlCommand(query, connexion);
                MySqlDataReader reader = command.ExecuteReader();
                List<List<string>> data = new List<List<string>>();
                while (reader.Read())
                {
                    List<string> row = new List<string>();
                    for (int i = 0; i < columns.Count; i++)
                    {
                        object value = reader.GetValue(i);
                        string stringValue;
                        if (value is int)
                        {
                            stringValue = ((int)value).ToString(); // Convertir l'entier en chaîne de caractères
                        }
                        else
                        {
                            stringValue = value.ToString(); // Convertir la valeur en chaîne de caractères
                        }
                        row.Add(stringValue);
                    }
                    data.Add(row);
                }
                reader.Close();
                command.Dispose();

                TableView tableview = new TableView($"liste {table}", columns, data);
                Window.AddElement(tableview);
                Window.ActivateElement(tableview);

                int[] answer = new int[3];
                if (table == "stock_velo")
                {
                    columns = GetColumnsNames("passe_velo");
                    types = GetColumnsType("passe_velo");
                }
                else
                {
                    columns = GetColumnsNames("passe_piece");
                    types = GetColumnsType("passe_piece");
                }

                for (int i = 0; i < columns.Count; i++)
                {
                    bool inputIsValid = false;

                    do
                    {
                        prompt = new Prompt($"Entrez {columns[i]} de type ({types[i]}) :");
                        Window.AddElement(prompt);
                        Window.ActivateElement(prompt);

                        string value = prompt.GetResponse().Value;

                        inputIsValid = int.TryParse(value, out int result);
                        if (inputIsValid)
                        {
                            answer[i] = result;
                        }
                        else
                        {
                            Dialog txt = new Dialog(
                                new List<string>() { "Veuillez entrer un entier valide.", },
                                null,
                                "ok"
                            );
                            Window.AddElement(txt);
                            Window.ActivateElement(txt);
                        }
                    } while (!inputIsValid);
                }
                if (table == "stock_velo")
                {
                    CreateCommandVelo(answer[0], answer[1], answer[2]);
                }
                else if (table == "stock_piece")
                {
                    CreateCommandPiece(answer[0], answer[1], answer[2]);
                }
            }
            catch (Exception ex)
            {
                Dialog txt = new Dialog(
                    new List<string>() { "Erreur lors de l'insertion des données.", ex.Message },
                    null,
                    "ok"
                );
                Window.AddElement(txt);
                Window.ActivateElement(txt);

            }
        }

        /// <summary>
        /// Methode qui permet de creer une commande de velo et de mettre a jour le stock et le chiffre d'affaire
        /// </summary>
        /// <param name="idVelo"> id du velo </param>
        /// <param name="idCommande"> id de la commande </param>
        /// <param name="quantite"> quantite de velo </param>
        /// <returns> Insertion </returns>
        public void CreateCommandVelo(int idVelo, int idCommande, int quantite)
        {
            try
            {
                // Insérer la commande dans la table passe_velo
                string query = "INSERT INTO passe_velo ( numDeProdVelo,numeroDeCommande, quantite_V) VALUES ( @idVelo,@idCommande, @quantite)";
                MySqlCommand command = new MySqlCommand(query, connexion);
                command.Parameters.AddWithValue("@idCommande", idCommande);
                command.Parameters.AddWithValue("@idVelo", idVelo);
                command.Parameters.AddWithValue("@quantite", quantite);
                int rowsAffected = command.ExecuteNonQuery();
                command.Dispose();

                // Mettre à jour le stock de vélos
                string query2 = "UPDATE stock_velo SET quantite_stock = quantite_stock - @quantite WHERE numDeProdVelo = @idVelo";
                MySqlCommand command2 = new MySqlCommand(query2, connexion);
                command2.Parameters.AddWithValue("@quantite", quantite);
                command2.Parameters.AddWithValue("@idVelo", idVelo);
                int rowsAffected2 = command2.ExecuteNonQuery();
                command2.Dispose();

                // Obtenir le montant total de la commande
                string montantTotalQuery = @"
            SELECT quantite_V * prixUnitaireVelo 
            FROM passe_velo 
            JOIN Velo ON passe_velo.numDeProdVelo = Velo.numDeProdVelo
            WHERE numeroDeCommande = @idCommande AND passe_velo.numDeProdVelo = @idVelo";
                MySqlCommand montantTotalCommand = new MySqlCommand(montantTotalQuery, connexion);
                montantTotalCommand.Parameters.AddWithValue("@idVelo", idVelo);
                montantTotalCommand.Parameters.AddWithValue("@idCommande", idCommande);
                decimal montantTotal = Convert.ToDecimal(montantTotalCommand.ExecuteScalar());
                montantTotalCommand.Dispose();

                // Obtenir l'identifiant du magasin à partir des produits commandés
                string idMagasinQuery = @"
            SELECT idMagasin 
            FROM stock_velo 
            JOIN Velo ON stock_velo.numDeProdVelo = Velo.numDeProdVelo 
            WHERE Velo.numDeProdVelo = @idVelo";
                MySqlCommand idMagasinCommand = new MySqlCommand(idMagasinQuery, connexion);
                idMagasinCommand.Parameters.AddWithValue("@idVelo", idVelo);
                int idMagasin = Convert.ToInt32(idMagasinCommand.ExecuteScalar());
                idMagasinCommand.Dispose();

                // Mettre à jour le chiffre d'affaires du magasin
                string updateChiffreAffairesQuery = @"
            UPDATE Magasin 
            SET chiffre_affaire = chiffre_affaire + @montantTotal 
            WHERE idMagasin = @idMagasin";
                MySqlCommand updateChiffreAffairesCommand = new MySqlCommand(updateChiffreAffairesQuery, connexion);
                updateChiffreAffairesCommand.Parameters.AddWithValue("@montantTotal", montantTotal);
                updateChiffreAffairesCommand.Parameters.AddWithValue("@idMagasin", idMagasin);
                updateChiffreAffairesCommand.ExecuteNonQuery();
                updateChiffreAffairesCommand.Dispose();

                Dialog txt = new Dialog(
                    new List<string>() { $"{rowsAffected} lignes affectées.", },
                    null,
                    "ok"
                );
                Window.AddElement(txt);
                Window.ActivateElement(txt);
            }
            catch (Exception ex)
            {
                Dialog txt = new Dialog(
                    new List<string>() { "Erreur lors de l'insertion des données.", ex.Message },
                    null,
                    "ok"
                );
                Window.AddElement(txt);
                Window.ActivateElement(txt);
            }
        }

        /// <summary>
        /// Methode qui permet de creer une commande de piece et de mettre a jour le stock et le chiffre d'affaire
        /// </summary>
        /// <param name="idPiece"> id de la piece </param>
        /// <param name="idCommande"> id de la commande </param>
        /// <param name="quantite"> quantite de piece </param>
        /// <returns> Insertion </returns>
        public void CreateCommandPiece(int idPiece, int idCommande, int quantite)
        {
            try
            {
                string query = "INSERT INTO passe_piece ( numDeProdPiece,numeroDeCommande, quantite_P) VALUES ( @idPiece,@idCommande, @quantite)";
                MySqlCommand command = new MySqlCommand(query, connexion);
                command.Parameters.AddWithValue("@idCommande", idCommande);
                command.Parameters.AddWithValue("@idPiece", idPiece);
                command.Parameters.AddWithValue("@quantite", quantite);
                int rowsAffected = command.ExecuteNonQuery();
                command.Dispose();

                string query2 = "UPDATE stock_piece SET quantite_stock = quantite_stock - @quantite WHERE numDeProdPiece = @idPiece";
                MySqlCommand command2 = new MySqlCommand(query2, connexion);
                command2.Parameters.AddWithValue("@quantite", quantite);
                command2.Parameters.AddWithValue("@idPiece", idPiece);
                int rowsAffected2 = command2.ExecuteNonQuery();
                command2.Dispose();

                // Obtenir le montant total de la commande
                string montantTotalQuery = @"
            SELECT quantite_P * prixUnitairePiece 
            FROM passe_piece 
            JOIN piece_detachee pd ON passe_piece.numDeProdPiece = pd.numDeProdpiece
            WHERE numeroDeCommande = @idCommande AND pd.numDeProdpiece = @idPiece";
                MySqlCommand montantTotalCommand = new MySqlCommand(montantTotalQuery, connexion);
                montantTotalCommand.Parameters.AddWithValue("@idPiece", idPiece);
                montantTotalCommand.Parameters.AddWithValue("@idCommande", idCommande);
                decimal montantTotal = Convert.ToDecimal(montantTotalCommand.ExecuteScalar());
                montantTotalCommand.Dispose();

                // Obtenir l'identifiant du magasin à partir des produits commandés
                string idMagasinQuery = @"
            SELECT idMagasin 
            FROM stock_piece 
            JOIN piece_detachee pd ON stock_piece.numDeProdPiece = pd.numDeProdPiece 
            WHERE pd.numDeProdPiece = @idPiece";
                MySqlCommand idMagasinCommand = new MySqlCommand(idMagasinQuery, connexion);
                idMagasinCommand.Parameters.AddWithValue("@idPiece", idPiece);
                int idMagasin = Convert.ToInt32(idMagasinCommand.ExecuteScalar());
                idMagasinCommand.Dispose();

                // Mettre à jour le chiffre d'affaires du magasin
                string updateChiffreAffairesQuery = @"
            UPDATE Magasin 
            SET chiffre_affaire = chiffre_affaire + @montantTotal 
            WHERE idMagasin = @idMagasin";
                MySqlCommand updateChiffreAffairesCommand = new MySqlCommand(updateChiffreAffairesQuery, connexion);
                updateChiffreAffairesCommand.Parameters.AddWithValue("@montantTotal", montantTotal);
                updateChiffreAffairesCommand.Parameters.AddWithValue("@idMagasin", idMagasin);
                updateChiffreAffairesCommand.ExecuteNonQuery();
                updateChiffreAffairesCommand.Dispose();

                Dialog txt = new Dialog(
                    new List<string>() { $"{rowsAffected} lignes affectées.", },
                    null,
                    "ok"
                );
                Window.AddElement(txt);
                Window.ActivateElement(txt);
            }
            catch (Exception ex)
            {
                Dialog txt = new Dialog(
                    new List<string>() { "Erreur lors de l'insertion des données.", ex.Message },
                    null,
                    "ok"
                );
                Window.AddElement(txt);
                Window.ActivateElement(txt);
            }
        }

        /// <summary>
        /// Méthode pour supprimer des données d'une table et de ses dependances
        /// </summary>
        /// <param name="table"> table dans laquelle on veut supprimer des données </param>
        /// <returns> Suppression </returns>
        public void deleteDataTable(string table)
        {
            List<string> columns = GetColumnsNames(table);
            try
            {
                List<string> ColumnsNames = GetColumnsNames(table);
                MySqlCommand command = new MySqlCommand($"SELECT * FROM {table}", connexion);
                MySqlDataReader reader = command.ExecuteReader();
                List<string> rows = new List<string>();
                List<List<string>> data = new List<List<string>>();
                while (reader.Read())
                {
                    List<string> row = new List<string>();
                    for (int i = 0; i < ColumnsNames.Count; i++)
                    {
                        object value = reader.GetValue(i);
                        string stringValue;
                        if (value is int)
                        {
                            stringValue = ((int)value).ToString(); // Convertir l'entier en chaîne de caractères
                        }
                        else
                        {
                            stringValue = value.ToString(); // Convertir la valeur en chaîne de caractères
                        }
                        row.Add(stringValue);
                    }
                    data.Add(row);
                }
                reader.Close();
                command.Dispose();
                TableView tableview = new TableView($"liste {table}", ColumnsNames, data);
                Window.AddElement(tableview);
                Window.ActivateElement(tableview);

                if (columns.Count == 0)
                {
                    Dialog txt = new Dialog(
                        new List<string>() { "La table est vide.", },
                        null,
                        "ok"
                    );
                    Window.AddElement(txt);
                    Window.ActivateElement(txt);
                    return;
                }
                Prompt prompt = new Prompt("Entrez la valeur de la clé primaire:");
                Window.AddElement(prompt);
                Window.ActivateElement(prompt);
                int primaryKeyValue = int.Parse(prompt.GetResponse().Value);

                DeleteFromReferences(table, columns[0], primaryKeyValue);
                DeleteFromTarget(table, columns[0], primaryKeyValue);

                Dialog Finaltxt = new Dialog(
                    new List<string>() { "Suppression effectuée.", },
                    null,
                    "ok"
                );
                Window.AddElement(Finaltxt);
                Window.ActivateElement(Finaltxt);
                Window.Freeze();
                Window.RemoveElement(tableview);
                Window.Render();
            }
            catch (Exception ex)
            {
                Dialog txt = new Dialog(
                    new List<string>() { "Erreur lors de la suppression des données.", ex.Message },
                    null,
                    "ok"
                );
                Window.AddElement(txt);
                Window.ActivateElement(txt);
            }
        }

        /// <summary>
        /// Méthode pour supprimer des données d'une table
        /// </summary>
        /// <param name="table"> table dans laquelle on veut supprimer des données </param>
        /// <param name="primaryKeyColumns"> colonne de la clé primaire </param>
        /// <param name="primaryKeyValue"> valeur de la clé primaire </param>
        /// <returns> Suppression </returns>
        public void DeleteFromTarget(string table, string primaryKeyColumns, int primaryKeyValue)
        {
            try
            {
                string query = $"DELETE FROM {table} WHERE {primaryKeyColumns} = {primaryKeyValue}";
                MySqlCommand command = new MySqlCommand(query, connexion);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Dialog txt = new Dialog(
                    new List<string>() { "Erreur lors de la suppression des données.", ex.Message },
                    null,
                    "ok"
                );
                Window.AddElement(txt);
                Window.ActivateElement(txt);
            }
        }

        /// <summary>
        /// Méthode pour supprimer des données des dependances d'une table
        /// </summary>
        /// <param name="table"> table dans laquelle on veut supprimer des données </param>
        /// <param name="primaryKeyColumns"> colonne de la clé primaire </param>
        /// <param name="primaryKeyValue"> valeur de la clé primaire </param>
        /// <returns> Suppression </returns>
        public void DeleteFromReferences(
            string table,
            string primaryKeyColumns,
            int primaryKeyValue
        )
        {
            List<string> references = GetReferences(table);

            try
            {
                foreach (string reference in references)
                {
                    string query =
                        $"DELETE FROM {reference} WHERE {primaryKeyColumns} = {primaryKeyValue}";
                    MySqlCommand command = new MySqlCommand(query, connexion);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Dialog txt = new Dialog(
                    new List<string>() { "Erreur lors de la suppression des données.", ex.Message },
                    null,
                    "ok"
                );
                Window.AddElement(txt);
                Window.ActivateElement(txt);
            }
        }

        /// <summary>
        /// Méthode pour avoir la liste des references d'une table
        /// </summary>
        /// <param name="table"> table qu'on veut etudier </param>
        /// <returns> liste des references </returns>
        public List<string> GetReferences(string table)
        {
            List<string> references = new List<string>();

            string query =
                @"
                SELECT TABLE_NAME
                FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                WHERE REFERENCED_TABLE_NAME = @TableName AND CONSTRAINT_SCHEMA = 'velomax';";

            MySqlCommand command = new MySqlCommand(query, connexion);
            command.Parameters.AddWithValue("@TableName", table);
            MySqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    references.Add(reader.GetString(0));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la récupération des références : " + ex.Message);
            }
            reader.Close();
            command.Dispose();
            return references;
        }

        /// <summary>
        /// Méthode pour mettre à jour des données d'une table
        /// </summary>
        /// <param name="table"> table dans laquelle on veut mettre à jour des données </param>
        /// <returns> Change une donnée </returns>
        public void updateDataTable(string table)
        {
            List<string> columnsNames = GetColumnsNames(table);
            string primaryKeyName = columnsNames[0];

            MySqlCommand command = new MySqlCommand($"SELECT * FROM {table}", connexion);
            MySqlDataReader reader = command.ExecuteReader();
            List<string> rows = new List<string>();
            List<List<string>> data = new List<List<string>>();
            while (reader.Read())
            {
                List<string> row = new List<string>();
                for (int i = 0; i < columnsNames.Count; i++)
                {
                    object value = reader.GetValue(i);
                    string stringValue;
                    if (value is int)
                    {
                        stringValue = ((int)value).ToString(); // Convertir l'entier en chaîne de caractères
                    }
                    else
                    {
                        stringValue = value.ToString(); // Convertir la valeur en chaîne de caractères
                    }
                    row.Add(stringValue);
                }
                data.Add(row);
            }
            reader.Close();
            command.Dispose();
            TableView tableview = new TableView($"liste {table}", columnsNames, data);
            Window.AddElement(tableview);
            Window.ActivateElement(tableview);

            columnsNames.RemoveAt(0);

            List<string> columnsType = GetColumnsType(table);
            columnsType.RemoveAt(0);

            try
            {
                if (columnsNames.Count == 0)
                {
                    Dialog txt = new Dialog(
                        new List<string>() { "La table est vide.", },
                        null,
                        "ok"
                    );
                    Window.AddElement(txt);
                    Window.ActivateElement(txt);
                    return;
                }

                Prompt prompt = new Prompt("Entrez la valeur de la clé primaire:");
                Window.AddElement(prompt);
                Window.ActivateElement(prompt);
                int primaryKeyValue = int.Parse(prompt.GetResponse().Value);

                string query =
                    $"UPDATE {table} SET {string.Join(",", columnsNames.ConvertAll(c => c + " = @" + c))} WHERE {primaryKeyName} = {primaryKeyValue}";
                command = new MySqlCommand(query, connexion);
                for (int i = 0; i < columnsNames.Count; i++)
                {
                    bool inputIsValid = false;

                    do
                    {
                        if (columnsType[i] == "date")
                        {
                            prompt = new Prompt(
                                $"Entrez {columnsNames[i]} de type ({columnsType[i]} sous le format yyyy-mm-dd) :"
                            );
                        }
                        else
                        {
                            prompt = new Prompt(
                                $"Entrez {columnsNames[i]} de type ({columnsType[i]}) :"
                            );
                        }

                        Window.AddElement(prompt);
                        Window.ActivateElement(prompt);

                        string value = prompt.GetResponse().Value;

                        if (string.IsNullOrEmpty(value))
                        {
                            command.Parameters.AddWithValue("@" + columnsNames[i], DBNull.Value);
                            inputIsValid = true;
                        }
                        else if (columnsType[i] == "int")
                        {
                            inputIsValid = int.TryParse(value, out int result);
                            if (inputIsValid)
                            {
                                command.Parameters.AddWithValue("@" + columnsNames[i], result);
                            }
                            else
                            {
                                Dialog txt = new Dialog(
                                    new List<string>() { "Veuillez entrer un entier valide.", },
                                    null,
                                    "ok"
                                );
                                Window.AddElement(txt);
                                Window.ActivateElement(txt);
                            }
                        }
                        else if (columnsType[i] == "decimal")
                        {
                            inputIsValid = decimal.TryParse(value, out decimal result);
                            if (inputIsValid)
                            {
                                command.Parameters.AddWithValue("@" + columnsNames[i], result);
                            }
                            else
                            {
                                Dialog txt = new Dialog(
                                    new List<string>() { "Veuillez entrer un decimal valide.", },
                                    null,
                                    "ok"
                                );
                                Window.AddElement(txt);
                                Window.ActivateElement(txt);
                            }
                        }
                        else if (columnsType[i] == "date")
                        {
                            inputIsValid = DateTime.TryParse(value, out DateTime result);
                            if (inputIsValid)
                            {
                                command.Parameters.AddWithValue("@" + columnsNames[i], result);
                            }
                            else
                            {
                                Dialog txt = new Dialog(
                                    new List<string>() { "Veuillez entrer une date valide.", },
                                    null,
                                    "ok"
                                );
                                Window.AddElement(txt);
                                Window.ActivateElement(txt);
                            }
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@" + columnsNames[i], value);
                            inputIsValid = true;
                        }
                    } while (!inputIsValid);
                }
                int rowsAffected = command.ExecuteNonQuery();
                Dialog Finaltxt = new Dialog(
                    new List<string>() { $"{rowsAffected} lignes affectées.", },
                    null,
                    "ok"
                );
                Window.AddElement(Finaltxt);
                Window.ActivateElement(Finaltxt);
            }
            catch (Exception ex)
            {
                Dialog txt = new Dialog(
                    new List<string>() { "Erreur lors de la mise à jour des données.", ex.Message },
                    null,
                    "ok"
                );
                Window.AddElement(txt);
                Window.ActivateElement(txt);
            }

            Window.Freeze();
        }

        /// <summary>
        /// Méthode pour afficher les données d'une table
        /// </summary>
        /// <param name="table"> table qu'on veut afficher </param>
        /// <returns> Affiche la table </returns>
        public void showTable(string table)
        {
            List<string> columns = GetColumnsNames(table);
            try
            {
                if (columns.Count == 0)
                {
                    Dialog txt = new Dialog(
                        new List<string>() { "La table est vide.", },
                        null,
                        "ok"
                    );
                    Window.AddElement(txt);
                    Window.ActivateElement(txt);
                    return;
                }
                MySqlCommand command = new MySqlCommand($"SELECT * FROM {table}", connexion);
                MySqlDataReader reader = command.ExecuteReader();
                List<List<string>> data = new List<List<string>>();
                while (reader.Read())
                {
                    List<string> row = new List<string>();
                    for (int i = 0; i < columns.Count; i++)
                    {
                        object value = reader.GetValue(i);
                        string stringValue;
                        if (value is int)
                        {
                            stringValue = ((int)value).ToString(); // Convertir l'entier en chaîne de caractères
                        }
                        else
                        {
                            stringValue = value.ToString(); // Convertir la valeur en chaîne de caractères
                        }
                        row.Add(stringValue);
                    }
                    data.Add(row);
                }
                reader.Close();
                command.Dispose();
                TableView tableview = new TableView($"liste {table}", columns, data);
                Window.AddElement(tableview);
                Window.ActivateElement(tableview);
                Window.Freeze();
            }
            catch (Exception ex)
            {
                Dialog txt = new Dialog(
                    new List<string>()
                    {
                        "Erreur lors de la récupération des données.",
                        ex.Message
                    },
                    null,
                    "ok"
                );
                Window.AddElement(txt);
                Window.ActivateElement(txt);
            }
        }

        /// <summary>
        /// Méthode pour afficher les données d'une table en fonction d'une commande
        /// </summary>
        /// <param name="title"> titre de la table </param>
        /// <param name="columns"> liste des colonnes de la table </param>
        /// <param name="command"> commande pour afficher les données </param>
        public void showCommand(string title, List<string> columns, string command)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(command, connexion);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<List<string>> data = new List<List<string>>();
                while (reader.Read())
                {
                    List<string> row = new List<string>();
                    for (int i = 0; i < columns.Count; i++)
                    {
                        object value = reader.GetValue(i);
                        string stringValue;
                        if (value is int)
                        {
                            stringValue = ((int)value).ToString(); // Convertir l'entier en chaîne de caractères
                        }
                        else
                        {
                            stringValue = value.ToString(); // Convertir la valeur en chaîne de caractères
                        }
                        row.Add(stringValue);
                    }
                    data.Add(row);
                }
                reader.Close();
                cmd.Dispose();
                TableView tableview = new TableView(title, columns, data);
                Window.AddElement(tableview);
                Window.ActivateElement(tableview);
                Window.Freeze();
            }
            catch (Exception ex)
            {
                Dialog txt = new Dialog(
                    new List<string>()
                    {
                        "Erreur lors de la récupération des données.",
                        ex.Message
                    },
                    null,
                    "ok"
                );
                Window.AddElement(txt);
                Window.ActivateElement(txt);
            }
        }
    }
}
