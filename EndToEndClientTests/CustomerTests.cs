using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace EndToEndClientTests
{
    public class CustomerTests
    {
        const string URL = "http://localhost:5090/customers";
        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void TestCustomersDetailsPage(string rowNumber)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--start-maximized");
            WebDriver driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(URL);
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            var customersListFirstName = wait.Until(_ => _.FindElements(By.XPath(string.Format($"/html/body/div[1]/div[2]/main/article/table/tbody/tr[{rowNumber}]/td[1]"))).FirstOrDefault());
            string inListFirstName = customersListFirstName.Text;

            var customerEditButton = driver.FindElements(By.XPath(string.Format($"/html/body/div[1]/div[2]/main/article/table/tbody/tr[{rowNumber}]/td[8]/button/i"))).FirstOrDefault();
            customerEditButton.Click();
            wait.Until(_ => _.FindElements(By.XPath("/html/body/div[1]/div[2]/main/article/h3[1]")).FirstOrDefault());

            var customerFormFirstName = driver.FindElements(By.XPath(string.Format("/html/body/div[1]/div[2]/main/article/form/div[1]/input"))).FirstOrDefault();
            string inFormFirstName = customerFormFirstName.GetAttribute("value");
            Assert.Equal(inListFirstName, inFormFirstName);

            driver.Close();
        }

        [Fact]
        public void TestCustomersDetailsPage_DeleteCustomerButton_Sould_Delete_The_Customer_Successfully()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--start-maximized");
            WebDriver driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(URL);
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            var customersListFirstName = wait.Until(_ => _.FindElements(By.XPath(string.Format($"/html/body/div[1]/div[2]/main/article/table/tbody/tr[1]/td[1]"))).FirstOrDefault());
            string inListFirstName = customersListFirstName.Text;

            var customerEditButton = driver.FindElements(By.XPath(string.Format($"/html/body/div[1]/div[2]/main/article/table/tbody/tr[1]/td[8]/button/i"))).FirstOrDefault();
            customerEditButton.Click();
            wait.Until(_ => _.FindElements(By.XPath("/html/body/div[1]/div[2]/main/article/h3[1]")).FirstOrDefault());

            var customerFormFirstName = driver.FindElements(By.XPath(string.Format("/html/body/div[1]/div[2]/main/article/form/div[1]/input"))).FirstOrDefault();
            string inFormFirstName = customerFormFirstName.GetAttribute("value");
            Assert.Equal(inListFirstName, inFormFirstName);

            var deleteButton = driver.FindElements(By.XPath("/html/body/div[1]/div[2]/main/article/form/button[2]")).FirstOrDefault();
            deleteButton.Click();

            var customerListTableColumn = wait.Until(_ => _.FindElements(By.XPath("/html/body/div[1]/div[2]/main/article/table/thead/tr/th[1]")).FirstOrDefault());
            Assert.Equal("FirstName", customerListTableColumn.Text);

            driver.Close();
        }

        [Fact]
        public void TestCustomersDetailsPage_UpdateCustomerButton_Sould_Update_The_Customer_Successfully()
        {
            string randomName = RandomString(10);
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--start-maximized");
            WebDriver driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(URL);
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            var customersListFirstName = wait.Until(_ => _.FindElements(By.XPath(string.Format($"/html/body/div[1]/div[2]/main/article/table/tbody/tr[1]/td[1]"))).FirstOrDefault());
            string inListFirstName = customersListFirstName.Text;

            var customerEditButton = driver.FindElements(By.XPath(string.Format($"/html/body/div[1]/div[2]/main/article/table/tbody/tr[1]/td[8]/button/i"))).FirstOrDefault();
            customerEditButton.Click();
            wait.Until(_ => _.FindElements(By.XPath("/html/body/div[1]/div[2]/main/article/h3[1]")).FirstOrDefault());

            var customerFormFirstName = driver.FindElements(By.XPath(string.Format("/html/body/div[1]/div[2]/main/article/form/div[1]/input"))).FirstOrDefault();
            string inFormFirstName = customerFormFirstName.GetAttribute("value");
            Assert.Equal(inListFirstName, inFormFirstName);

            var lastNamelInput = driver.FindElements(By.XPath("//input[@id='lastname']")).FirstOrDefault();
            lastNamelInput.Clear();
            lastNamelInput.SendKeys(randomName);

            var updateButton = driver.FindElements(By.XPath("/html/body/div[1]/div[2]/main/article/form/button[1]")).FirstOrDefault();
            updateButton.Click();

            var customerListTableColumn = wait.Until(_ => _.FindElements(By.XPath("/html/body/div[1]/div[2]/main/article/table/thead/tr/th[1]")).FirstOrDefault());
            Assert.Equal("FirstName", customerListTableColumn.Text);

            driver.Close();
        }

        [Fact]
        public void TestCreateCustomerPage_CorrectCustomerInputData_Should_Be_Saved_Successfully()
        {
            string randomName = RandomString(7);
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--start-maximized");
            WebDriver driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(URL);
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            wait.Until(_ => _.FindElements(By.XPath("/html/body/div[1]/div[2]/main/article/h3")).FirstOrDefault());

            var createButton = driver.FindElements(By.XPath("/html/body/div[1]/div[2]/main/article/button")).FirstOrDefault();
            createButton.Click();
            wait.Until(_ => _.FindElements(By.XPath("/html/body/div[1]/div[2]/main/article/h3[2]")).FirstOrDefault());

            var firstNameInput = driver.FindElements(By.XPath("//input[@id='firstname']")).FirstOrDefault();
            firstNameInput.Clear();
            firstNameInput.SendKeys(randomName);

            var lastNamelInput = driver.FindElements(By.XPath("//input[@id='lastname']")).FirstOrDefault();
            lastNamelInput.Clear();
            lastNamelInput.SendKeys(randomName);

            var emailInput = driver.FindElements(By.XPath("//input[@id='email']")).FirstOrDefault();
            emailInput.Clear();
            emailInput.SendKeys($"{randomName}@example.com");

            var dateOfBirthInput = driver.FindElements(By.XPath("//input[@id='dateofbirth']")).FirstOrDefault();
            dateOfBirthInput.Clear();
            dateOfBirthInput.SendKeys("1990-09-09");

            var phoneInput = driver.FindElements(By.XPath("//input[@id='phonenumber']")).FirstOrDefault();
            phoneInput.Clear();
            phoneInput.SendKeys("+989117115755");

            var bankAccountNumberInput = driver.FindElements(By.XPath("//input[@id='bankaccountnumber']")).FirstOrDefault();
            bankAccountNumberInput.Clear();
            bankAccountNumberInput.SendKeys("12345678910");


            var saveButton = driver.FindElements(By.XPath("/html/body/div[1]/div[2]/main/article/form/button[1]")).FirstOrDefault();
            saveButton.Click();

            var customerListTableColumn = wait.Until(_ => _.FindElements(By.XPath("/html/body/div[1]/div[2]/main/article/table/thead/tr/th[1]")).FirstOrDefault());
            Assert.Equal("FirstName", customerListTableColumn.Text);


            driver.Close();
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}