-- Gestion des stocks
-- Velo
SELECT Magasin.idMagasin, Magasin.nom AS nom_magasin, Velo.numDeProdVelo, Velo.nomVelo AS nom_velo, stock_velo.quantite_stock
FROM Magasin
JOIN stock_velo ON Magasin.idMagasin = stock_velo.idMagasin
JOIN Velo ON stock_velo.numDeProdVelo = Velo.numDeProdVelo;

SELECT Velo.numDeProdVelo, Velo.nomVelo AS nom_velo, SUM(stock_velo.quantite_stock) AS quantite_totale
FROM Velo
JOIN stock_velo ON Velo.numDeProdVelo = stock_velo.numDeProdVelo
GROUP BY Velo.numDeProdVelo, Velo.nomVelo;
                        
SELECT marque, SUM(quantite_stock)
FROM Velo
JOIN stock_velo ON Velo.numDeProdVelo = stock_velo.numDeProdVelo
GROUP BY marque;

-- Piece
SELECT Magasin.idMagasin, Magasin.nom AS nom_magasin, Piece_detachee.numDeProdPiece, Piece_detachee.description AS description_piece, stock_piece.quantite_Stock
FROM Magasin
JOIN stock_piece ON Magasin.idMagasin = stock_piece.idMagasin
JOIN Piece_detachee ON stock_piece.numDeProdPiece = Piece_detachee.numDeProdPiece;
                    
SELECT Piece_detachee.numDeProdPiece, Piece_detachee.description AS description_piece, SUM(stock_piece.quantite_Stock) AS quantite_totale
FROM Piece_detachee
JOIN stock_piece ON Piece_detachee.numDeProdPiece = stock_piece.numDeProdPiece
GROUP BY Piece_detachee.numDeProdPiece, Piece_detachee.description;
                    
SELECT Fournisseur.idFournisseur, Fournisseur.nomEntreprise AS nom_fournisseur, Piece_detachee.numDeProdPiece, Piece_detachee.description AS description_piece, stock_piece.quantite_Stock
FROM Fournisseur
JOIN Fournit ON Fournisseur.idFournisseur = Fournit.idFournisseur
JOIN Piece_detachee ON Fournit.numDeProdPiece = Piece_detachee.numDeProdPiece
JOIN stock_piece ON Piece_detachee.numDeProdPiece = stock_piece.numDeProdPiece;

-- Statistique
-- Vente Par modele de velo
SELECT passe_velo.numDeProdVelo AS id_article, Velo.nomVelo AS nom_article, SUM(passe_velo.quantite_V) AS quantite_vendue
FROM passe_velo
JOIN Velo ON passe_velo.numDeProdVelo = Velo.numDeProdVelo
GROUP BY passe_velo.numDeProdVelo, Velo.nomVelo;

-- Vente des velos par Magasin
SELECT m.nom AS Magasin, Velo.nomVelo AS nom_article, SUM(pv.quantite_V) AS QuantiteVendue
FROM Magasin m
JOIN Vendeur v ON m.idMagasin = v.idMagasin
JOIN Commande c ON v.idVendeur = c.idVendeur
JOIN passe_velo pv ON c.numeroDeCommande = pv.numeroDeCommande
JOIN velo ON pv.numDeProdVelo = velo.numDeProdVelo
GROUP BY m.idMagasin, velo.numDeProdVelo;

-- Vente des velos par vendeur 

SELECT v.idVendeur, v.nom AS Vendeur, velo.nomVelo, SUM(pv.quantite_v) AS QuantiteVendue
FROM Vendeur v
JOIN Commande c ON v.idVendeur = c.idVendeur
JOIN passe_velo pv ON c.numeroDeCommande = pv.numeroDeCommande
JOIN velo ON pv.numDeProdVelo = velo.numDeProdVelo
GROUP BY v.idVendeur, velo.numDeProdVelo;

-- Ventes des pieces
SELECT passe_piece.numDeProdPiece AS id_piece, piece_detachee.description AS piece_desc, SUM(passe_piece.quantite_P) AS quantite_vendue
FROM passe_piece
JOIN piece_detachee ON passe_piece.numDeProdPiece = Piece_detachee.numDeProdPiece
GROUP BY passe_piece.numDeProdPiece, Piece_detachee.description;

-- Ventes des pieces par magasin
SELECT m.nom AS Magasin, pd.description AS Produit, SUM(pp.quantite_P) AS QuantiteVendue
FROM Magasin m
JOIN Vendeur v ON m.idMagasin = v.idMagasin
JOIN Commande c ON v.idVendeur = c.idVendeur
JOIN passe_piece pp ON c.numeroDeCommande = pp.numeroDeCommande
JOIN Piece_detachee pd ON pp.numDeProdPiece = pd.numDeProdPiece
GROUP BY m.idMagasin, pd.numDeProdPiece;

-- Ventes des pieces par vendeur
SELECT v.idVendeur, v.nom AS Vendeur, pd.description AS Produit, SUM(pp.quantite_P) AS QuantiteVendue
FROM Vendeur v
JOIN Commande c ON v.idVendeur = c.idVendeur
JOIN passe_piece pp ON c.numeroDeCommande = pp.numeroDeCommande
JOIN Piece_detachee pd ON pp.numDeProdPiece = pd.numDeProdPiece
GROUP BY v.idVendeur, pd.numDeProdPiece;

-- Client par Programme
SELECT f.description AS Programme_Adhesion,GROUP_CONCAT(c.nomCompagnie SEPARATOR ', '), GROUP_CONCAT(c.nomPersonne SEPARATOR ', ')
FROM adhere a
INNER JOIN Client c ON a.idClient = c.idClient
INNER JOIN Fidelio f ON a.noProg = f.noProg
GROUP BY f.noProg;

-- Date d'expiration par Client
SELECT a.idClient, f.noProg, DATE_ADD(a.date_debut, INTERVAL f.duree DAY) AS date_expiration
FROM Adhere a
JOIN Fidelio f ON a.noProg = f.noProg;

-- meilleur client par nombre de pieces achetees
SELECT c.idClient, c.nomCompagnie AS nom_entreprise, c.nomPersonne AS nom_client, SUM(pp.quantite_P) AS quantite_totale_pieces_vendues
FROM Client c
JOIN Commande co ON c.idClient = co.idClient
JOIN passe_piece pp ON co.numeroDeCommande = pp.numeroDeCommande
GROUP BY c.idClient, c.nomCompagnie
ORDER BY quantite_totale_pieces_vendues DESC;

-- meilleur client par cumul en euros
SELECT 
    c.idClient, 
    c.nomCompagnie AS nom_entreprise,
    c.nomPersonne AS nom_client, 
    SUM(pp.quantite_P * pd.prixUnitairePiece) AS montant_total_cumule
FROM Client c
JOIN Commande co ON c.idClient = co.idClient
JOIN passe_piece pp ON co.numeroDeCommande = pp.numeroDeCommande
JOIN Piece_detachee pd ON pp.numDeProdPiece = pd.numDeProdPiece
GROUP BY c.idClient, c.nomCompagnie
ORDER BY montant_total_cumule DESC;

-- commande
-- moyenne des montants
SELECT AVG(total_commande) AS moyenne_montant_commande
FROM (
    SELECT 
        co.numeroDeCommande,
        SUM(pp.quantite_P * pd.prixUnitairePiece) AS total_commande
    FROM Commande co
    JOIN passe_piece pp ON co.numeroDeCommande = pp.numeroDeCommande
    JOIN Piece_detachee pd ON pp.numDeProdPiece = pd.numDeProdPiece
    GROUP BY co.numeroDeCommande
) AS commandes;

-- moyenne du nombre de pièce
SELECT AVG(nombre_pieces) AS moyenne_nombre_pieces_par_commande
FROM (
    SELECT 
        numeroDeCommande,
        SUM(quantite_P) AS nombre_pieces
    FROM 
        passe_piece
    GROUP BY 
        numeroDeCommande
) AS pieces_par_commande;

-- Moyenne du nombre de velo
SELECT AVG(nombre_velos) AS moyenne_nombre_velos_par_commande
FROM (
    SELECT 
        numeroDeCommande,
        SUM(quantite_V) AS nombre_velos
    FROM 
        passe_velo
    GROUP BY 
        numeroDeCommande
) AS velos_par_commande;

-- salariés
-- Bonus des salariés
SELECT v.idVendeur, v.nom, SUM(pv.quantite_V * ve.prixUnitaireVelo) * 0.05 AS Bonus
FROM Vendeur v
JOIN Commande com ON v.idVendeur = com.idVendeur
JOIN passe_velo pv ON com.numeroDeCommande = pv.numeroDeCommande
JOIN Velo ve ON pv.numDeProdVelo = ve.numDeProdVelo
GROUP BY v.idVendeur;

-- Moyenne des bonus
SELECT AVG(Bonus) AS BonusMoyen
FROM (SELECT SUM(pv.quantite_V * ve.prixUnitaireVelo) * 0.05 AS Bonus
      FROM Vendeur v
      JOIN Commande com ON v.idVendeur = com.idVendeur
      JOIN passe_velo pv ON com.numeroDeCommande = pv.numeroDeCommande
      JOIN Velo ve ON pv.numDeProdVelo = ve.numDeProdVelo
      GROUP BY v.idVendeur) AS BonusCalculs;
