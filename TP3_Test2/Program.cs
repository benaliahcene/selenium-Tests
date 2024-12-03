using TP3_Test2;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;

//  test d'ajouter un etudiant, lui assigner et lui attribuer une note 

namespace TP3_Test2
{
    internal class Program
    {

        private IWebDriver _driver;
        private string edgeDriverPath = @"C:\Users\Ahcene Benali\Desktop\tp3\edgedriver_win64";
        private string screenshotFolder = @"C:\Users\Ahcene Benali\Desktop\tp3\screenshots\"; // Dossier pour les captures d'écran


        public void Setup()
        {
            // Configuration pour Edge
            var options = new EdgeOptions();
            _driver = new EdgeDriver(edgeDriverPath, options);
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl("https://testselenium.netlify.app/"); // Page de connexion de l'application
        }

        public void CreateUserTest()
        {
            // Créer un utilisateur (comme dans le premier test)
            _driver.Navigate().GoToUrl("https://testselenium.netlify.app/register");
            _driver.FindElement(By.Id("firstName")).SendKeys("John");
            _driver.FindElement(By.Id("lastName")).SendKeys("Doe");
            _driver.FindElement(By.Id("email")).SendKeys("johndoe@example.com");
            _driver.FindElement(By.Id("password")).SendKeys("password123");
            _driver.FindElement(By.Id("dob")).SendKeys("01/01/1990");
            _driver.FindElement(By.Id("registerButton")).Click();

            // Attendre que le message de succès apparaisse
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var successMessageElement = wait.Until(drv => drv.FindElement(By.Id("messageBox")));
            var successMessage = successMessageElement.Text;

            // Vérifier le message de succès avec une condition
            if (successMessage.Contains("Inscription réussie"))
            {
                Console.WriteLine("Test de création d'utilisateur réussi.");
            }
            else
            {
                Console.WriteLine("Échec du test de création d'utilisateur. Message reçu : " + successMessage);
            }
        }

        public void LoginUserTest()
        {
            // Remplir les champs de connexion avec les informations de l'utilisateur
            _driver.FindElement(By.Id("email")).SendKeys("johndoe@example.com");
            _driver.FindElement(By.Id("password")).SendKeys("password123");

            // Soumettre le formulaire de connexion
            _driver.FindElement(By.Id("loginbutton")).Click();

            // Attendre que la page de redirection ou le message de bienvenue apparaisse
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var pageTitle = wait.Until(drv => drv.Title);
            Thread.Sleep(5000);
            // Vérifier si la connexion est réussie, en vérifiant le titre de la page
            if (pageTitle.Contains("Connexion")) // Remplacer par le titre de votre page d'accueil
            {
                Console.WriteLine("Test de connexion réussi.");
            }
            else
            {
                Console.WriteLine("Échec du test de connexion. Titre de page : " + pageTitle);
            }

            // Vérifier la redirection vers la page d'accueil
            if (_driver.Url.Contains("dashboard.html"))
            {
                Console.WriteLine("Test de redirection réussi.");
            }
            else
            {
                Console.WriteLine("Échec de la redirection. URL actuelle : " + _driver.Url);
            }
        }

        public void TestStudentManagement()
        {

            // Vérifier la présence du message de bienvenue
            string welcomeMessage = _driver.FindElement(By.Id("welcomeMessage")).Text;
            Console.WriteLine(welcomeMessage);  // Pour vérifier que le message contient "Bonjour, John Doe"

            // Étape 2 : Création d'un étudiant

            _driver.FindElement(By.Id("studentName")).SendKeys("Alice Dupont");
            _driver.FindElement(By.Id("school")).SendKeys("Université de Paris");
            _driver.FindElement(By.Id("level")).SendKeys("Licence");
            _driver.FindElement(By.Id("program")).SendKeys("Informatique");
            _driver.FindElement(By.Id("studentForm")).Submit();

            // Vérifier que l'étudiant a été ajouté
            var successMessage = _driver.FindElement(By.Id("messageBox")).Text;
            Console.WriteLine(successMessage);  // Vérifier que le message contient "Étudiant ajouté avec succès"
            Thread.Sleep(4000);

            // Étape 3 : Assigner l'étudiant à un cours
            // Sélectionner un étudiant dans le menu déroulant "assignStudent"
            var selectStudent = new SelectElement(_driver.FindElement(By.Id("assignStudent")));
            selectStudent.SelectByText("Alice Dupont");

            // Sélectionner un cours dans le menu déroulant "assignCourse"
            var selectCourse = new SelectElement(_driver.FindElement(By.Id("assignCourse")));
            selectCourse.SelectByText("Mathématiques");

            // Sélectionner un groupe dans le menu déroulant "assignGroup"
            var selectGroup = new SelectElement(_driver.FindElement(By.Id("assignGroup")));
            selectGroup.SelectByText("Groupe A");
            _driver.FindElement(By.Id("assignmentForm")).Submit();
            Thread.Sleep(4000);

            // Vérifier que l'étudiant a été assigné
            var assignSuccessMessage = _driver.FindElement(By.Id("messageBox")).Text;
            Console.WriteLine(assignSuccessMessage);  // Vérifier que le message contient "Étudiant assigné au groupe"

            // Étape 4 : Attribuer une note

            var selectGradeStudent = _driver.FindElement(By.Id("gradeStudent"));
            var gradeOptions = selectGradeStudent.FindElements(By.TagName("option"));
            var gradeStudentOption = gradeOptions.FirstOrDefault(o => o.Text.Contains("Alice Dupont"));
            if (gradeStudentOption != null)
            {
                gradeStudentOption.Click();
            }

            _driver.FindElement(By.Id("gradeCourse")).SendKeys("Mathématiques");
            _driver.FindElement(By.Id("gradeInput")).SendKeys("85");
            _driver.FindElement(By.Id("gradeForm")).Submit();

            // Vérifier que la note a été attribuée
            var gradeSuccessMessage = _driver.FindElement(By.Id("messageBox")).Text;
            Console.WriteLine(gradeSuccessMessage);  // Vérifier que le message contient "Note attribuée avec succès"

            // Étape 5 : Vérifier dans la page students.html
            Thread.Sleep(4000);

            // Vérifier si l'étudiant et sa note sont présents
            bool isStudentInList = _driver.PageSource.Contains("Alice Dupont");
            bool isGradeAssigned = _driver.PageSource.Contains("85");

            Console.WriteLine(isStudentInList ? "Étudiant trouvé dans la liste" : "Étudiant non trouvé dans la liste");
            Console.WriteLine(isGradeAssigned ? "Note attribuée trouvée" : "Note non attribuée trouvée");
        }

        public void TearDown()
        {
            // Fermer le navigateur après le test
            _driver.Quit();
        }
        static void Main(string[] args)
        {
            // Initialiser le programme
            Program programme = new Program();

            // Exécuter les étapes du test
            programme.Setup();
            programme.CreateUserTest();
            Thread.Sleep(4000);
            programme.LoginUserTest();
            Thread.Sleep(4000);
            programme.TestStudentManagement();
            Thread.Sleep(4000);

            programme.TearDown();

            // Attendre une entrée avant de fermer la console
            Console.WriteLine("Test terminé. Appuyez sur une touche pour fermer.");
            Console.ReadKey();
        }
    }
}