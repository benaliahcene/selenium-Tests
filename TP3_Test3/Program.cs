using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;

// test de se connecter 

namespace TP3_Test3
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

        public void TearDown()
        {
            // Fermer le navigateur
            TakeScreenshot("Test_Fini"); // Capture d'écran après la fin du test
            _driver.Quit();
        }

        // Fonction pour prendre des captures d'écran
        private void TakeScreenshot(string screenshotName)
        {
            // Créer le répertoire s'il n'existe pas
            if (!Directory.Exists(screenshotFolder))
            {
                Directory.CreateDirectory(screenshotFolder);
            }

            // Caster IWebDriver en ITakesScreenshot pour capturer une image
            ITakesScreenshot screenshotDriver = (ITakesScreenshot)_driver;

            // Prendre la capture d'écran
            Screenshot screenshot = screenshotDriver.GetScreenshot();

            // Sauvegarder la capture d'écran avec un nom unique
            string screenshotPath = Path.Combine(screenshotFolder, $"{screenshotName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            screenshot.SaveAsFile(screenshotPath);

            Console.WriteLine($"Capture d'écran enregistrée : {screenshotPath}");
        }

        static void Main(string[] args)
        {
            // Initialiser le programme
            Program programme = new Program();

            // Exécuter les étapes du test
            programme.Setup();
            programme.CreateUserTest();
            Thread.Sleep(4000);
            programme.LoginUserTest();  // Test de la connexion
            programme.TearDown();

            // Attendre une entrée avant de fermer la console
            Console.WriteLine("Test terminé. Appuyez sur une touche pour fermer.");
            Console.ReadKey();
        }
    }
   
}