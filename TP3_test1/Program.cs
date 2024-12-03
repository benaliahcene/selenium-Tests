using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;

// test de creer un etudiant

namespace TP3_test1
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
            _driver.Navigate().GoToUrl("https://testselenium.netlify.app/register"); // Remplacez par l'URL de votre application
        }

        public void CreateUserTest()
        {
            // Remplir les champs du formulaire d'inscription
            _driver.FindElement(By.Id("firstName")).SendKeys("John");
            _driver.FindElement(By.Id("lastName")).SendKeys("Doe");
            _driver.FindElement(By.Id("email")).SendKeys("johndoe@example.com");
            _driver.FindElement(By.Id("password")).SendKeys("password123");
            _driver.FindElement(By.Id("dob")).SendKeys("01/01/1990");

            // Soumettre le formulaire
            _driver.FindElement(By.Id("registerButton")).Click();

            // Attendre que le message de succès apparaisse (évite les erreurs dues à un chargement tardif)
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var successMessageElement = wait.Until(drv => drv.FindElement(By.Id("messageBox")));
            var successMessage = successMessageElement.Text;

            // Vérifier le message de succès
            if (successMessage.Contains("Inscription réussie"))
            {
                Console.WriteLine("Test réussi: Inscription réussie.");
                TakeScreenshot("Inscription_réussie");

            }
            else
            {
                Console.WriteLine($"Test échoué: Message reçu : {successMessage}");
                TakeScreenshot("Inscription_Echouée"); // Capture d'écran en cas d'échec
            }
            Thread.Sleep(5000);
            // Vérifier la redirection vers la page d'accueil
            if (_driver.Url.Contains("index.html"))
            {
                Console.WriteLine("Test réussi: Redirection vers la page d'accueil.");
            }
            else
            {
                Console.WriteLine($"Test échoué: La redirection vers index.html a échoué. URL actuelle : {_driver.Url}");
                TakeScreenshot("Redirection_Echouée"); // Capture d'écran en cas d'échec
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
            screenshot.SaveAsFile(screenshotPath); // La méthode SaveAsFile accepte juste le chemin du fichier


            Console.WriteLine($"Capture d'écran enregistrée : {screenshotPath}");
        }

        static void Main(string[] args)
        {
            // Initialiser le programme
            Program programme = new Program();

            // Exécuter les étapes du test
            programme.Setup();
            programme.CreateUserTest();
            Thread.Sleep(5000);

            programme.TearDown();

            // Attendre une entrée avant de fermer la console
            Console.WriteLine("Test terminé. Appuyez sur une touche pour fermer.");
            Console.ReadKey();
        }
        
    }
}