using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using ConsoleAppVisuals;
using ConsoleAppVisuals.AnimatedElements;
using ConsoleAppVisuals.Enums;
using ConsoleAppVisuals.InteractiveElements;
using ConsoleAppVisuals.PassiveElements;
using Projet_SouidiCazac;

namespace Projet
{
    public class Interface
    {
        /// <summary>
        /// Demande à l'utilisateur de rentrer son nom d'utilisateur et son mot de passe pour se connecter à la base de données.
        /// </summary>
        /// <returns> Coonexion a une BDD</returns>
        public static DataAccess dbConnection()
        {
            Prompt prompt = new Prompt("Entrez votre nom d'utilisateur :");
            Window.AddElement(prompt);
            Window.ActivateElement(prompt);
            string txtUser = prompt.GetResponse()!.Value;

            prompt = new Prompt("Entrez votre mot de passe :");
            Window.AddElement(prompt);
            Window.ActivateElement(prompt);
            string txtPassword = prompt.GetResponse()!.Value;
            return new DataAccess(txtUser, txtPassword);
        }

        /// <summary>
        /// Affiche le menu principal de l'application.
        /// </summary>
        /// <param name="db">Connexion à la base de données.</param>
        /// <returns>Menu principal</returns>
        public static void mainMenu(DataAccess db)
        {
            string[] options = new string[]
            {
                "Ajouter des données",
                "Modifier des données",
                "Supprimer des données",
                "Gestion du stock",
                "Statistiques",
                "Quitter"
            };

            ScrollingMenu menu = new ScrollingMenu(
                "Choisir une option parmi les suivantes :",
                0,
                Placement.TopCenter,
                options
            );

            while (true)
            {
                Window.RemoveAllElements();
                Window.Render();
                Header header = new Header();
                Title title = new Title("VeloMax");
                Footer footer = new Footer();
                Window.AddElement(header, footer, title);
                Window.AddElement(menu);
                Window.ActivateElement(title);
                Window.ActivateElement(footer);
                Window.ActivateElement(menu);

                var response = menu.GetResponse();
                switch (response!.Status)
                {
                    case Status.Selected:
                        switch (response.Value)
                        {
                            case 0:
                                Window.RemoveElement(title);
                                Window.Render();
                                CreateData(db);
                                break;
                            case 1:
                                Window.RemoveElement(title);
                                Window.Render();
                                UpdateData(db);
                                break;
                            case 2:
                                Window.RemoveElement(title);
                                Window.Render();
                                DeleteData(db);
                                break;
                            case 3:
                                // Afficher des données
                                Window.RemoveElement(title);
                                Window.Render();
                                ManageStock(db);
                                break;
                            case 4:
                                // Statistiques
                                Window.RemoveElement(title);
                                Window.Render();
                                Statistiques(db);
                                break;
                            case 5:
                                db.Terminer();
                                Window.Close();
                                return;
                        }
                        break;
                    case Status.Escaped:
                    case Status.Deleted:
                        // Quit the app anyway
                        return;
                }
            }
        }

        /// <summary>
        /// Affiche le menu pour ajouter des données.
        /// </summary>
        /// <param name="db">Connexion à la base de données.</param>
        /// <returns>Menu pour ajouter des données</returns>
        public static void CreateData(DataAccess db)
        {
            string[] options = new string[]
            {
                "Fournisseur",
                "Client",
                "Vélo",
                "Pièce",
                "Commande",
                "Magasin",
                "Gerant",
                "Vendeur",
                "Retour"
            };

            ScrollingMenu menu = new ScrollingMenu(
                "Dans quelle table voulez-vous ajouter des données ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // Fournisseur
                            db.createDataTable("fournisseur", false);
                            break;
                        case 1:
                            // Demander les informations du client
                            CreateClient(db);
                            break;
                        case 2:
                            // Demander les informations du vélo
                            CreateVelo(db);
                            break;
                        case 3:
                            // Demander les informations de la pièce
                            CreatePiece(db);
                            break;
                        case 4:
                            // Demander les informations de la commande
                            CreateCommand(db);
                            break;
                        case 5:
                            // Demander les informations du magasin
                            db.createDataTable("magasin", false);
                            break;
                        case 6:
                            // Demander les informations du gérant
                            db.createDataTable("gerant", false);
                            break;
                        case 7:
                            // Demander les informations du vendeur
                            db.createDataTable("vendeur", false);
                            break;
                        case 8:
                            // Retour
                            break;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }

        /// <summary>
        /// Affiche le menu pour supprimer des données.
        /// </summary>
        /// <param name="db">Connexion à la base de données.</param>
        /// <returns>Menu pour supprimer des données</returns>
        public static void DeleteData(DataAccess db)
        {
            string[] options = new string[]
            {
                "Fournisseur",
                "Client",
                "Vélo",
                "Pièce",
                "Commande",
                "Magasin",
                "Gerant",
                "Vendeur",
                "Retour"
            };

            ScrollingMenu menu = new ScrollingMenu(
                "Dans quelle table voulez-vous supprimer des données ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // Fournisseur
                            db.deleteDataTable("fournisseur");
                            break;
                        case 1:
                            // Demander les informations du client
                            db.deleteDataTable("client");
                            break;
                        case 2:
                            // Demander les informations du vélo
                            db.deleteDataTable("velo");
                            break;
                        case 3:
                            // Demander les informations de la pièce
                            db.deleteDataTable("piece_detachee");
                            break;
                        case 4:
                            // Demander les informations de la commande
                            db.deleteDataTable("commande");
                            break;
                        case 5:
                            // Demander les informations du magasin
                            db.deleteDataTable("magasin");
                            break;
                        case 6:
                            // Demander les informations du gérant
                            db.deleteDataTable("gerant");
                            break;
                        case 7:
                            // Demander les informations du vendeur
                            db.deleteDataTable("vendeur");
                            break;
                        case 8:
                            // Retour
                            return;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }

        /// <summary>
        /// Affiche le menu pour modifier des données.
        /// </summary>
        /// <param name="db">Connexion à la base de données.</param>
        /// <returns>Menu pour modifier des données</returns>
        public static void UpdateData(DataAccess db)
        {
            string[] options = new string[]
            {
                "Fournisseur",
                "Client",
                "Vélo",
                "Pièce",
                "Commande",
                "Magasin",
                "Gerant",
                "Vendeur",
                "Retour"
            };

            ScrollingMenu menu = new ScrollingMenu(
                "Dans quelle table voulez-vous modifier des données ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // Fournisseur
                            db.updateDataTable("fournisseur");
                            break;
                        case 1:
                            // Demander les informations du client
                            db.updateDataTable("client");
                            break;
                        case 2:
                            // Demander les informations du vélo
                            db.updateDataTable("velo");
                            break;
                        case 3:
                            // Demander les informations de la pièce
                            db.updateDataTable("piece_detachee");
                            break;
                        case 4:
                            // Demander les informations de la commande
                            db.updateDataTable("commande");
                            break;
                        case 5:
                            // Demander les informations du magasin
                            db.updateDataTable("magasin");
                            break;
                        case 6:
                            // Demander les informations du gérant
                            db.updateDataTable("gerant");
                            break;
                        case 7:
                            // Demander les informations du vendeur
                            db.updateDataTable("vendeur");
                            break;
                        case 8:
                            // Retour
                            return;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }

        /// <summary>
        /// Affiche le menu pour creer une commande.
        /// </summary>
        /// <param name="db">Connexion à la base de données.</param>
        /// <returns>Menu pour creer une commande</returns>
        public static void CreateCommand(DataAccess db)
        {
            string[] options = new string[]
            {
                "Ajouter une commande",
                "Ajouter une pièce à une commande",
                "Ajouter un vélo à une commande",
                "Retour"
            };

            ScrollingMenu menu = new ScrollingMenu(
                "Que voulez-vous faire ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // Ajouter une commande
                            db.createDataTable("commande", false);
                            break;
                        case 1:
                            // Ajouter une pièce à une commande
                            db.askCommmand("stock_piece");
                            break;
                        case 2:
                            // Ajouter un vélo à une commande
                            db.askCommmand("stock_velo");
                            break;
                        case 3:
                            // Retour
                            break;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }

        /// <summary>
        /// Affiche le menu pour creer un client.
        /// </summary>
        /// <param name="db">Connexion à la base de données.</param>
        /// <returns>Menu pour creer un client</returns>
        public static void CreateClient(DataAccess db)
        {
            string[] options = new string[]
            {
                "Ajouter un client",
                "Ajoutee un client a un programme de fidelite",
                "Retour"
            };

            ScrollingMenu menu = new ScrollingMenu(
                "Que voulez-vous faire ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // Ajouter un client
                            db.createDataTable("client", false);
                            break;
                        case 1:
                            // Ajouter un client a un programme de fidelite
                            db.createDataTable("adhere", true);
                            break;
                        case 2:
                            // Retour
                            return;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }

        /// <summary>
        /// Affiche le menu pour creer un velo.
        /// </summary>
        /// <param name="db">Connexion à la base de données.</param>
        /// <returns>Menu pour creer un velo</returns>
        public static void CreateVelo(DataAccess db)
        {
            string[] options = new string[]
            {
                "Ajouter un vélo",
                "Ajouter une pièce à un vélo",
                "Ajouter un vélo à un magasin",
                "Retour"
            };

            ScrollingMenu menu = new ScrollingMenu(
                "Que voulez-vous faire ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // Ajouter un vélo
                            db.createDataTable("velo", false);
                            break;
                        case 1:
                            // Ajouter une pièce à un vélo
                            db.createDataTable("est_compose", true);
                            break;
                        case 2:
                            // Ajouter un vélo à un magasin
                            db.createDataTable("stock_velo", true);
                            break;
                        case 3:
                            // Retour
                            return;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }

        /// <summary>
        /// Affiche le menu pour creer une piece.
        /// </summary>
        /// <param name="db">Connexion à la base de données.</param>
        /// <returns>Menu pour creer une piece</returns>
        public static void CreatePiece(DataAccess db)
        {
            string[] options = new string[]
            {
                "Ajouter une pièce",
                "Ajouter une pièce à un magasin",
                "Retour"
            };

            ScrollingMenu menu = new ScrollingMenu(
                "Que voulez-vous faire ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // Ajouter une pièce
                            db.createDataTable("piece_detachee", false);
                            break;
                        case 1:
                            // Ajouter une pièce à un magasin
                            db.createDataTable("stock_piece", true);
                            break;
                        case 2:
                            // Retour
                            return;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }

        /// <summary>
        /// Affiche le menu pour gerer le stock.
        /// </summary>
        /// <param name="db">Connexion à la base de données.</param>
        /// <returns>Menu pour gerer le stock</returns>
        public static void ManageStock(DataAccess db)
        {
            string[] options = new string[] { "Vélo", "Pièce", };

            ScrollingMenu menu = new ScrollingMenu(
                "Quelle table voulez-vous afficher ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // Afficher les vélos
                            ShowVelos(db);
                            break;
                        case 1:
                            // Afficher les pièces
                            ShowPieces(db);
                            break;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }

        /// <summary>
        /// Affiche les stock des vélos.
        /// </summary>
        /// <param name="db">Connexion à la base de données.</param>
        /// <returns>Stock des vélos</returns>
        public static void ShowVelos(DataAccess db)
        {
            string[] options = new string[] { "par magasin", "par velo", "par marque" };

            ScrollingMenu menu = new ScrollingMenu(
                "Comment voulez-vous afficher le stock des vélos ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            List<string> varPerMag = new List<string>
            {
                "idMagasin",
                "nom_magasin",
                "numDeProdVelo",
                "nom_velo",
                "quantite_stock"
            };
            List<string> varPerVelo = new List<string>
            {
                "numDeProdVelo",
                "nom_velo",
                "quantite_totale"
            };
            List<string> varPerMarque = new List<string> { "marque", "quantite_totale" };

            string varPerMagStr =
                @"SELECT Magasin.idMagasin, Magasin.nom AS nom_magasin, Velo.numDeProdVelo, Velo.nomVelo AS nom_velo, stock_velo.quantite_stock
                                    FROM Magasin
                                    JOIN stock_velo ON Magasin.idMagasin = stock_velo.idMagasin
                                    JOIN Velo ON stock_velo.numDeProdVelo = Velo.numDeProdVelo;";
            string varPerVeloStr =
                @"SELECT Velo.numDeProdVelo, Velo.nomVelo AS nom_velo, SUM(stock_velo.quantite_stock) AS quantite_totale
                        FROM Velo
                        JOIN stock_velo ON Velo.numDeProdVelo = stock_velo.numDeProdVelo
                        GROUP BY Velo.numDeProdVelo, Velo.nomVelo;";

            string varPerMarqueStr =
                @"SELECT marque, SUM(quantite_stock)
                        FROM Velo
                        JOIN stock_velo ON Velo.numDeProdVelo = stock_velo.numDeProdVelo
                        GROUP BY marque";

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // Afficher les vélos par magasin
                            db.showCommand("velo par magasin", varPerMag, varPerMagStr);
                            break;
                        case 1:
                            // Afficher les vélos par velo
                            db.showCommand("velo par modele", varPerVelo, varPerVeloStr);
                            break;
                        case 2:
                            // Afficher les vélos par marque
                            db.showCommand("velo par marque", varPerMarque, varPerMarqueStr);
                            break;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }

        /// <summary>
        /// Affiche les stock des Pieces.
        /// </summary>
        /// <param name="db">Connexion à la base de données.</param>
        /// <returns>Stock des Pieces</returns>
        public static void ShowPieces(DataAccess db)
        {
            string[] options = new string[]
            {
                "par magasin",
                "par pieces",
                "par founisseurs",
                "Critique"
            };

            ScrollingMenu menu = new ScrollingMenu(
                "Comment voulez-vous afficher le stock des pieces ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            List<string> varPerMag = new List<string>
            {
                "idMagasin",
                "nom_magasin",
                "numDeProdPiece",
                "description_piece",
                "quantite_stock"
            };
            List<string> varPerPiece = new List<string>
            {
                "numDeProdPiece",
                "description_piece",
                "quantite_totale"
            };
            List<string> varPerFournisseur = new List<string>
            {
                "idFournisseur",
                "nom_fournisseur",
                "numDeProdPiece",
                "description_piece",
                "quantite_Stock"
            };

            string varPerMagStr =
                @"SELECT Magasin.idMagasin, Magasin.nom AS nom_magasin, Piece_detachee.numDeProdPiece, Piece_detachee.description AS description_piece, stock_piece.quantite_Stock
                    FROM Magasin
                    JOIN stock_piece ON Magasin.idMagasin = stock_piece.idMagasin
                    JOIN Piece_detachee ON stock_piece.numDeProdPiece = Piece_detachee.numDeProdPiece;";

            string varPerPieceStr =
                @"SELECT Piece_detachee.numDeProdPiece, Piece_detachee.description AS description_piece, SUM(stock_piece.quantite_Stock) AS quantite_totale
                    FROM Piece_detachee
                    JOIN stock_piece ON Piece_detachee.numDeProdPiece = stock_piece.numDeProdPiece
                    GROUP BY Piece_detachee.numDeProdPiece, Piece_detachee.description;";

            string varPerFournisseurStr =
                @"SELECT Fournisseur.idFournisseur, Fournisseur.nomEntreprise AS nom_fournisseur, Piece_detachee.numDeProdPiece, Piece_detachee.description AS description_piece, stock_piece.quantite_Stock
                    FROM Fournisseur
                    JOIN Fournit ON Fournisseur.idFournisseur = Fournit.idFournisseur
                    JOIN Piece_detachee ON Fournit.numDeProdPiece = Piece_detachee.numDeProdPiece
                    JOIN stock_piece ON Piece_detachee.numDeProdPiece = stock_piece.numDeProdPiece;";

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // Afficher les vélos par magasin
                            db.showCommand("piece par magasin", varPerMag, varPerMagStr);
                            break;
                        case 1:
                            // Afficher les vélos par velo
                            db.showCommand(
                                "piece par numero de Production",
                                varPerPiece,
                                varPerPieceStr
                            );
                            break;
                        case 2:
                            // Afficher les vélos par marque
                            db.showCommand(
                                "piece par Fournisseur",
                                varPerFournisseur,
                                varPerFournisseurStr
                            );
                            break;
                        case 3:
                            // Afficher les pièces critiques
                            db.showCommand(
                                "PIECES EN MANQUE",
                                new List<string>
                                {
                                    "numDeProdPiece",
                                    "description",
                                    "quantite_stock"
                                },
                                @"SELECT Piece_detachee.numDeProdPiece, description, quantite_stock
                                    FROM Piece_detachee
                                    JOIN stock_piece ON Piece_detachee.numDeProdPiece = stock_piece.numDeProdPiece
                                    WHERE quantite_stock <= 2;"
                            );
                            break;
                        case 4:
                            // Retour
                            return;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }

        /// <summary>
        /// Affiche un menu pour choisir le type de statistique à afficher.
        /// </summary>
        /// <param name="db">Accès aux données.</param>
        /// <returns>Menu pour choisir le type de statistique à afficher</returns>
        public static void Statistiques(DataAccess db)
        {
            string[] options = new string[]
            {
                "Quantité vendue",
                "Clients",
                "Commandes",
                "Salariés",
                "Magasins",
                "Retour"
            };

            ScrollingMenu menu = new ScrollingMenu(
                "Quelle statistique voulez-vous afficher ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // Quantité vendue
                            QuantitéVendue(db);
                            break;
                        case 1:
                            // Clients
                            StatClients(db);
                            break;
                        case 2:
                            // Commandes
                            statCommands(db);
                            break;
                        case 3:
                            // salarié
                            statSalarié(db);
                            break;
                        case 4:
                            // Magasins
                            db.showTable("magasin");
                            break;
                        case 5:
                            // Retour
                            return;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }

        /// <summary>
        /// Affiche les statistiques sur la quantité vendue de vélos ou de pièces détachées.
        /// </summary>
        /// <param name="db">Accès aux données.</param>
        /// <returns>Statistiques sur la quantité vendue de vélos ou de pièces détachées</returns>
        public static void QuantitéVendue(DataAccess db)
        {
            string[] options = new string[] { "Vélo", "Pièce" };

            ScrollingMenu menu = new ScrollingMenu(
                "Quelle table voulez-vous afficher ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // Afficher les vélos
                            VentesVelo(db);
                            break;
                        case 1:
                            // Afficher les pièces
                            VentesPiece(db);
                            break;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }

        /// <summary>
        /// Affiche les statistiques sur les ventes de vélos.
        /// </summary>
        /// <param name="db">Accès aux données.</param>
        public static void VentesVelo(DataAccess db)
        {
            string[] options = new string[]
            {
                "par modele",
                "par magasin",
                "par vendeur",
                "retour"
            };

            ScrollingMenu menu = new ScrollingMenu(
                "Comment voulez-vous afficher les ventes des vélos ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            List<string> venPerVelo = new List<string>
            {
                "numDeProdVelo",
                "nom_velo",
                "quantite_vendue",
            };
            List<string> venPerMag = new List<string> { "Magasin", "nom_velo", "quantite_vendue" };
            List<string> venPerVendeur = new List<string>
            {
                "idVendeur",
                "Vendeur",
                "nom_velo",
                "quantite_vendue"
            };

            string venPerVelostr =
                @"SELECT passe_velo.numDeProdVelo AS id_article, Velo.nomVelo AS nom_article, SUM(passe_velo.quantite_V) AS quantite_vendue
FROM passe_velo
JOIN Velo ON passe_velo.numDeProdVelo = Velo.numDeProdVelo
GROUP BY passe_velo.numDeProdVelo, Velo.nomVelo;";
            string venPerMagStr =
                @"SELECT m.nom AS Magasin, Velo.nomVelo AS nom_article, SUM(pv.quantite_V) AS QuantiteVendue
FROM Magasin m
JOIN Vendeur v ON m.idMagasin = v.idMagasin
JOIN Commande c ON v.idVendeur = c.idVendeur
JOIN passe_velo pv ON c.numeroDeCommande = pv.numeroDeCommande
JOIN velo ON pv.numDeProdVelo = velo.numDeProdVelo
GROUP BY m.idMagasin, velo.numDeProdVelo;";

            string venPerVendeurStr =
                @"SELECT v.idVendeur, v.nom AS Vendeur, velo.nomVelo, SUM(pv.quantite_v) AS QuantiteVendue
FROM Vendeur v
JOIN Commande c ON v.idVendeur = c.idVendeur
JOIN passe_velo pv ON c.numeroDeCommande = pv.numeroDeCommande
JOIN velo ON pv.numDeProdVelo = velo.numDeProdVelo
GROUP BY v.idVendeur, velo.numDeProdVelo;";

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // Afficher les vélos par magasin
                            db.showCommand("velo par modele", venPerVelo, venPerVelostr);
                            break;
                        case 1:
                            //
                            db.showCommand("vente par magasin", venPerMag, venPerMagStr);
                            break;
                        case 2:
                            // Afficher les vélos par marque
                            db.showCommand("vente par vendeur", venPerVendeur, venPerVendeurStr);
                            break;
                        case 3:
                            // Retour
                            return;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }

        /// <summary>
        /// Affiche les statistiques sur les ventes de pièces détachées.
        /// </summary>
        /// <param name="db">Accès aux données.</param>
        public static void VentesPiece(DataAccess db)
        {
            string[] options = new string[]
            {
                "par modele",
                "par magasin",
                "par vendeur",
                "retour"
            };

            ScrollingMenu menu = new ScrollingMenu(
                "Comment voulez-vous afficher les ventes des vélos ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            List<string> venPerPiece = new List<string>
            {
                "numDeProdPiece",
                "piece_desc",
                "quantite_vendue",
            };
            List<string> venPerMag = new List<string> { "Magasin", "nom_velo", "quantite_vendue" };
            List<string> venPerVendeur = new List<string>
            {
                "idVendeur",
                "Vendeur",
                "nom_velo",
                "quantite_vendue"
            };

            string venPerPiecestr =
                @"SELECT passe_piece.numDeProdPiece AS id_piece, piece_detachee.description AS piece_desc, SUM(passe_piece.quantite_P) AS quantite_vendue
FROM passe_piece
JOIN piece_detachee ON passe_piece.numDeProdPiece = Piece_detachee.numDeProdPiece
GROUP BY passe_piece.numDeProdPiece, Piece_detachee.description;";
            string venPerMagStr =
                @"SELECT m.nom AS Magasin, pd.description AS Produit, SUM(pp.quantite_P) AS QuantiteVendue
FROM Magasin m
JOIN Vendeur v ON m.idMagasin = v.idMagasin
JOIN Commande c ON v.idVendeur = c.idVendeur
JOIN passe_piece pp ON c.numeroDeCommande = pp.numeroDeCommande
JOIN Piece_detachee pd ON pp.numDeProdPiece = pd.numDeProdPiece
GROUP BY m.idMagasin, pd.numDeProdPiece;";

            string venPerVendeurStr =
                @"SELECT v.idVendeur, v.nom AS Vendeur, pd.description AS Produit, SUM(pp.quantite_P) AS QuantiteVendue
FROM Vendeur v
JOIN Commande c ON v.idVendeur = c.idVendeur
JOIN passe_piece pp ON c.numeroDeCommande = pp.numeroDeCommande
JOIN Piece_detachee pd ON pp.numDeProdPiece = pd.numDeProdPiece
GROUP BY v.idVendeur, pd.numDeProdPiece;";

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // Afficher les vélos par magasin
                            db.showCommand("velo par modele", venPerPiece, venPerPiecestr);
                            break;
                        case 1:
                            //
                            db.showCommand("vente par magasin", venPerMag, venPerMagStr);
                            break;
                        case 2:
                            // Afficher les vélos par marque
                            db.showCommand("vente par vendeur", venPerVendeur, venPerVendeurStr);
                            break;
                        case 3:
                            // Retour
                            return;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }

        /// <summary>
        /// Affiche les statistiques sur les clients.
        /// </summary>
        /// <param name="db">Accès aux données.</param>
        public static void StatClients(DataAccess db)
        {
            string[] options = new string[]
            {
                "Liste Totale",
                "Liste par Programme fidelio",
                "Date d'expiration des adhesions",
                "Meilleur Clients ",
                "Retour"
            };

            ScrollingMenu menu = new ScrollingMenu(
                "Quelle statistique pour les clients voulez-vous afficher ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // Liste Totale
                            db.showTable("client");
                            break;
                        case 1:
                            // Liste par Programme fidelio
                            db.showCommand(
                                "adhesion",
                                new List<string>
                                {
                                    "Programme_Adhesion",
                                    "nom entreprise",
                                    "nom personne"
                                },
                                @"SELECT f.description AS Programme_Adhesion,GROUP_CONCAT(c.nomCompagnie SEPARATOR ', '), GROUP_CONCAT(c.nomPersonne SEPARATOR ', ')
FROM adhere a
INNER JOIN Client c ON a.idClient = c.idClient
INNER JOIN Fidelio f ON a.noProg = f.noProg
GROUP BY f.noProg;"
                            );
                            break;
                        case 2:
                            // Date d'expiration des adhesions
                            db.showCommand(
                                "date d'expiration des adhesions",
                                new List<string> { "idClient", "noProg", "date_expiration" },
                                @"SELECT a.idClient, f.noProg, DATE_ADD(a.date_debut, INTERVAL f.duree DAY) AS date_expiration
FROM Adhere a
JOIN Fidelio f ON a.noProg = f.noProg;"
                            );
                            break;
                        case 3:
                            // Meilleur Clients
                            MeilleurClients(db);
                            break;
                        case 4:
                            // Retour
                            return;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }

        /// <summary>
        /// Affiche les statistiques sur les meilleurs clients.
        /// </summary>
        /// <param name="db">Accès aux données.</param>
        public static void MeilleurClients(DataAccess db)
        {
            string[] options = new string[]
            {
                "par nombre de pièces achetées",
                "en cumul en euros",
                "Retour"
            };

            ScrollingMenu menu = new ScrollingMenu(
                "Comment voulez-vous afficher les meilleurs clients ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // par nombre de pièces achetées
                            db.showCommand(
                                "meilleur client par nombre de pieces achetees",
                                new List<string>
                                {
                                    "idClient",
                                    "nom_entreprise",
                                    "nom personel",
                                    "quantite_totale_pieces_vendues"
                                },
                                @"SELECT 
                                    c.idClient, 
                                    c.nomCompagnie AS nom_entreprise,
                                    c.nomPersonne AS nom_client, 
                                    SUM(pp.quantite_P) AS quantite_totale_pieces_vendues
                                    FROM Client c
                                    JOIN Commande co ON c.idClient = co.idClient
                                    JOIN passe_piece pp ON co.numeroDeCommande = pp.numeroDeCommande
                                    GROUP BY c.idClient, c.nomCompagnie
                                    ORDER BY quantite_totale_pieces_vendues DESC;"
                            );
                            break;
                        case 1:
                            // en cumul en euros
                            db.showCommand(
                                "meilleur client par cumul en euros",
                                new List<string>
                                {
                                    "idClient",
                                    "nom_entreprise",
                                    "nom_client",
                                    "montant_total_cumule"
                                },
                                @"SELECT 
    c.idClient, 
    c.nomCompagnie AS nom_entreprise,
    c.nomPersonne AS nom_client, 
    SUM(pp.quantite_P * pd.prixUnitairePiece) AS montant_total_cumule
FROM 
    Client c
JOIN 
    Commande co ON c.idClient = co.idClient
JOIN 
    passe_piece pp ON co.numeroDeCommande = pp.numeroDeCommande
JOIN 
    Piece_detachee pd ON pp.numDeProdPiece = pd.numDeProdPiece
GROUP BY 
    c.idClient, c.nomCompagnie
ORDER BY 
    montant_total_cumule DESC;"
                            );
                            break;
                        case 2:
                            // Retour
                            return;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }

        /// <summary>
        /// Affiche les statistiques sur les commandes.
        /// </summary>
        /// <param name="db">Accès aux données.</param>
        public static void statCommands(DataAccess db)
        {
            string[] options = new string[]
            {
                "Liste des commandes",
                "moyenne des montants",
                "moyenne du nombre de pièces",
                "moyenne du nombre de vélo",
                "Retour"
            };

            ScrollingMenu menu = new ScrollingMenu(
                "Quelle statistique pour les commandes voulez-vous afficher ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // Liste des commandes
                            db.showTable("commande");
                            break;
                        case 1:
                            // moyenne des montants
                            db.showCommand(
                                "moyenne des montants",
                                new List<string> { "moyenne_montant" },
                                @"SELECT AVG(total_commande) AS moyenne_montant_commande
FROM (
    SELECT 
        co.numeroDeCommande,
        SUM(pp.quantite_P * pd.prixUnitairePiece) AS total_commande
    FROM 
        Commande co
    JOIN 
        passe_piece pp ON co.numeroDeCommande = pp.numeroDeCommande
    JOIN 
        Piece_detachee pd ON pp.numDeProdPiece = pd.numDeProdPiece
    GROUP BY 
        co.numeroDeCommande
) AS commandes;"
                            );
                            break;
                        case 2:
                            // moyenne du nombre de pièces
                            db.showCommand(
                                "moyenne pieces",
                                new List<string> { "moyenne_pieces" },
                                @"SELECT AVG(nombre_pieces) AS moyenne_nombre_pieces_par_commande
FROM (
    SELECT 
        numeroDeCommande,
        SUM(quantite_P) AS nombre_pieces
    FROM 
        passe_piece
    GROUP BY 
        numeroDeCommande
) AS pieces_par_commande;"
                            );
                            break;
                        case 3:
                            // moyenne du nombre de vélo
                            db.showCommand(
                                "moyenne Velo",
                                new List<string> { "moyenne_velo" },
                                @"SELECT AVG(nombre_velos) AS moyenne_nombre_velos_par_commande
FROM (
    SELECT 
        numeroDeCommande,
        SUM(quantite_V) AS nombre_velos
    FROM 
        passe_velo
    GROUP BY 
        numeroDeCommande
) AS velos_par_commande;"
                            );
                            break;
                        case 4:
                            // Retour
                            return;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }

        /// <summary>
        /// Affiche les statistiques sur les salariés.
        /// </summary>
        /// <param name="db">Accès aux données.</param>
        public static void statSalarié(DataAccess db)
        {
            string[] options = new string[]
            {
                "listes des vendeurs",
                "listes des gérants",
                "bonus des vendeurs",
                "bonus moyen des salariés",
                "Retour"
            };

            ScrollingMenu menu = new ScrollingMenu(
                "Quelle statistique pour les salariés voulez-vous afficher ?",
                0,
                Placement.TopCenter,
                options
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);

            var response = menu.GetResponse();
            switch (response!.Status)
            {
                case Status.Selected:
                    switch (response.Value)
                    {
                        case 0:
                            // listes des vendeurs
                            db.showTable("vendeur");
                            break;
                        case 1:
                            // listes des gérants
                            db.showTable("gerant");
                            break;
                        case 2:
                            // bonus des vendeurs
                            db.showCommand(
                                "bonus des vendeurs",
                                new List<string> { "idVendeur", "nom", "bonus" },
                                @"SELECT v.idVendeur, v.nom, SUM(pv.quantite_V * ve.prixUnitaireVelo) * 0.05 AS Bonus
FROM Vendeur v
JOIN Commande com ON v.idVendeur = com.idVendeur
JOIN passe_velo pv ON com.numeroDeCommande = pv.numeroDeCommande
JOIN Velo ve ON pv.numDeProdVelo = ve.numDeProdVelo
GROUP BY v.idVendeur;"
                            );
                            break;
                        case 3:
                            // bonus moyen des salariés
                            db.showCommand(
                                "bonus moyen des salariés",
                                new List<string> { "moyenne_bonus" },
                                @"SELECT AVG(Bonus) AS BonusMoyen
FROM (SELECT SUM(pv.quantite_V * ve.prixUnitaireVelo) * 0.05 AS Bonus
      FROM Vendeur v
      JOIN Commande com ON v.idVendeur = com.idVendeur
      JOIN passe_velo pv ON com.numeroDeCommande = pv.numeroDeCommande
      JOIN Velo ve ON pv.numDeProdVelo = ve.numDeProdVelo
      GROUP BY v.idVendeur) AS BonusCalculs;"
                            );
                            break;
                        case 4:
                            // Retour
                            return;
                    }
                    break;
                case Status.Escaped:
                case Status.Deleted:
                    // Quit the app anyway
                    break;
            }
        }
    }
}
