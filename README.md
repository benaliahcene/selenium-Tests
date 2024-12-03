Projet : Tests de Connexion, Création de Compte et Gestion des Étudiants avec Selenium et Edge
Description
Ce projet implémente des tests automatisés pour valider les fonctionnalités essentielles d'une application web : la connexion, la création de compte, l'ajout d'un étudiant, et l'attribution d'une note. Les tests utilisent Selenium WebDriver avec Microsoft Edge pour simuler et valider les actions de l'utilisateur sur l'interface.

Prérequis
Logiciels nécessaires :
Microsoft Edge (dernière version)
.NET SDK (version 6.0 ou supérieure)
EdgeDriver (assurez-vous qu'il correspond à la version de Microsoft Edge installée)
Configuration du système :
Téléchargez EdgeDriver depuis le site officiel de Selenium.
Placez EdgeDriver dans un répertoire accessible, par exemple :
C:\Users\<VotreNomUtilisateur>\Desktop\tp3\edgedriver_win64.
Installation
Clonez ou téléchargez ce dépôt.
Ouvrez le projet dans Visual Studio ou tout autre IDE compatible avec .NET.
Restaurez les dépendances nécessaires (par exemple, Selenium WebDriver).
Utilisation
Mettez à jour la variable edgeDriverPath dans le fichier Program.cs pour pointer vers le répertoire contenant EdgeDriver.
Exécutez le projet à partir de votre IDE ou en ligne de commande :
dotnet run  
Fonctionnalités testées
Connexion à l'application :

Accéder à la page de connexion.
Remplir les champs "Nom d'utilisateur" et "Mot de passe".
Cliquer sur le bouton de connexion.
Vérifier que l'utilisateur est correctement authentifié.
Création d'un compte utilisateur :

Remplir les informations nécessaires pour créer un compte (nom, prénom, email, mot de passe).
Soumettre le formulaire.
Valider que le compte est créé avec succès.
Ajout d'un étudiant :

Accéder au formulaire de création d'étudiant.
Remplir les champs (nom, école, niveau, programme).
Soumettre le formulaire.
Vérifier que l'étudiant est ajouté à la liste.
Attribution d'une note à un étudiant :

Sélectionner un étudiant dans la liste déroulante.
Choisir un cours et un groupe.
Entrer une note et soumettre le formulaire.
Valider que la note a été correctement attribuée.
Structure du Projet
Program.cs : Contient les scripts Selenium pour exécuter les différents tests.
Dossier EdgeDriver : Inclut le binaire de Microsoft EdgeDriver.
Exemples d'erreurs communes
NoSuchElementException : Assurez-vous que les sélecteurs utilisés correspondent aux éléments actuels du site.
SessionNotCreatedException : Vérifiez que la version d'EdgeDriver correspond à la version de Microsoft Edge installée.
Aide supplémentaire
Documentation Selenium
Support Microsoft EdgeDriver
Auteur
Ahcene Ben Ali

