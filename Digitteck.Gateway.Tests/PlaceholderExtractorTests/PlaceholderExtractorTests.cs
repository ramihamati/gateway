using Digitteck.Gateway.Service;
using NUnit.Framework;
using System.Collections.Generic;
using System.Web;
#pragma warning disable IDE1006 // Naming Styles

namespace PlaceholderExtractorTests
{
    public class PlaceholderExtractorTests
    {
        public PlaceholderExtractor PlaceholderExtractor { get; private set; }

        [SetUp]
        public void Setup()
        {
            this.PlaceholderExtractor = new PlaceholderExtractor();
        }

        [Test]

        public void GetPlaceholdersFromTemplate_test_1()
        {
            string template = "/movies/{movieName}";
            List<Placeholder> placeholders = this.PlaceholderExtractor.GetPlaceholdersFromTemplate(new TemplatePathAndQueryObject(template));

            Assert.AreEqual(1, placeholders.Count);

            Assert.AreEqual(@"{movieName}", placeholders[0].LongName);
            Assert.AreEqual(@"movieName", placeholders[0].ShortName);
        }

        [Test]
        public void GetPlaceholdersFromTemplate_test_2()
        {
            string template = "/movies/{movieName}?Rating={movieRating}";
            List<Placeholder> placeholders = this.PlaceholderExtractor.GetPlaceholdersFromTemplate(new TemplatePathAndQueryObject(template));

            Assert.AreEqual(2, placeholders.Count);

            Assert.AreEqual(@"{movieName}", placeholders[0].LongName);
            Assert.AreEqual(@"movieName", placeholders[0].ShortName);

            Assert.AreEqual(@"{movieRating}", placeholders[1].LongName);
            Assert.AreEqual(@"movieRating", placeholders[1].ShortName);
        }

        [Test]
        public void GetPlaceholdersFromTemplate_test_3()
        {
            string template = "/movies/{movieName}?Rating={movieName}";
            //duplicate placeholder names
            Assert.Catch(() => this.PlaceholderExtractor.GetPlaceholdersFromTemplate(new TemplatePathAndQueryObject(template)));
        }

        [Test]
        public void GetPlaceholdersFromTemplate_test_4()
        {
            string template = "/movies/{movieName}/{movieRating}";
            List<Placeholder> placeholders = this.PlaceholderExtractor.GetPlaceholdersFromTemplate(new TemplatePathAndQueryObject(template));

            Assert.AreEqual(2, placeholders.Count);

            Assert.AreEqual(@"{movieName}", placeholders[0].LongName);
            Assert.AreEqual(@"movieName", placeholders[0].ShortName);

            Assert.AreEqual(@"{movieRating}", placeholders[1].LongName);
            Assert.AreEqual(@"movieRating", placeholders[1].ShortName);
        }

        [Test]
        public void GetRegexWithNamedGroups_test_1()
        {
            string template = "/movies/{movieName}";
            TemplatePathAndQueryObject templateObject = new TemplatePathAndQueryObject(template);

            List<Placeholder> placeholders = this.PlaceholderExtractor.GetPlaceholdersFromTemplate(templateObject);

            string pattern = this.PlaceholderExtractor.GetRegexWithNamedGroups(templateObject, placeholders);

            Assert.AreEqual(@"\/movies\/(?<movieName>[^{}&\/\?\\]{0,})", pattern);
        }

        [Test]
        public void GetRegexWithNamedGroups_test_2()
        {
            string template = "/movies/{movieName}?Rating={movieRating}";
            TemplatePathAndQueryObject templateObject = new TemplatePathAndQueryObject(template);

            List<Placeholder> placeholders = this.PlaceholderExtractor.GetPlaceholdersFromTemplate(templateObject);

            string pattern = this.PlaceholderExtractor.GetRegexWithNamedGroups(templateObject, placeholders);
            Assert.AreEqual(@"\/movies\/(?<movieName>[^{}&\/\?\\]{0,})\?Rating=(?<movieRating>[^{}&\/\?\\]{0,})", pattern);
        }

        [Test]
        public void GetRegexWithNamedGroups_test_4()
        {
            string template = "/movies/{movieName}/{movieRating}";
            TemplatePathAndQueryObject templateObject = new TemplatePathAndQueryObject(template);

            List<Placeholder> placeholders = this.PlaceholderExtractor.GetPlaceholdersFromTemplate(templateObject);

            string pattern = this.PlaceholderExtractor.GetRegexWithNamedGroups(templateObject, placeholders);
            Assert.AreEqual(@"\/movies\/(?<movieName>[^{}&\/\?\\]{0,})\/(?<movieRating>[^{}&\/\?\\]{0,})", pattern);
        }

        [Test]
        public void GetPlaceholdervaluesFromQuery_test_1()
        {
            const string template = "/movies/{movieName}";
            TemplatePathAndQueryObject templateObject = new TemplatePathAndQueryObject(template);
            const string query = "/movies/AmericanGangster";
            PathAndQueryObject pathObject = new PathAndQueryObject(query);

            List<PlaceholderValue> values = this.PlaceholderExtractor.ExtractPlaceholderValuesFromQuery(templateObject, pathObject);

            Assert.AreEqual(1, values.Count);
            Assert.AreEqual("movieName", values[0].Placeholder.ShortName);
            Assert.AreEqual("AmericanGangster", values[0].Value);
        }

        [Test]
        public void GetPlaceholdervaluesFromQuery_test_3()
        {
            string template = "/movies/{movieName}?Rating={movieRating}";
            string query = "/movies/AmericanGangster?Rating=9.7";
            TemplatePathAndQueryObject templateObject = new TemplatePathAndQueryObject(template);
            PathAndQueryObject pathObject = new PathAndQueryObject(query);

            List<PlaceholderValue> values = this.PlaceholderExtractor.ExtractPlaceholderValuesFromQuery(templateObject, pathObject);

            Assert.AreEqual(2, values.Count);

            Assert.AreEqual("movieName", values[0].Placeholder.ShortName);
            Assert.AreEqual("AmericanGangster", values[0].Value);

            Assert.AreEqual("movieRating", values[1].Placeholder.ShortName);
            Assert.AreEqual("9.7", values[1].Value);
        }

        [Test]
        public void GetPlaceholdervaluesFromQuery_test_4()
        {
            string template = "/movies/{movieName}/{movieRating}";
            string query = "/movies/AmericanGangster/9.7";
            TemplatePathAndQueryObject templateObject = new TemplatePathAndQueryObject(template);
            PathAndQueryObject pathObject = new PathAndQueryObject(query);

            List<PlaceholderValue> values = this.PlaceholderExtractor.ExtractPlaceholderValuesFromQuery(templateObject, pathObject);

            Assert.AreEqual(2, values.Count);

            Assert.AreEqual("movieName", values[0].Placeholder.ShortName);
            Assert.AreEqual("AmericanGangster", values[0].Value);

            Assert.AreEqual("movieRating", values[1].Placeholder.ShortName);
            Assert.AreEqual("9.7", values[1].Value);
        }

        [Test]
        public void ReplacePlaceholdervalues_Test_1()
        {
            const string template = "/movies/{movieName}";
            const string actualPath = "/movies/AmericanGangster";

            TemplatePathAndQueryObject templateObject = new TemplatePathAndQueryObject(template);
            PathAndQueryObject actualPathObject = new PathAndQueryObject(actualPath);

            List<PlaceholderValue> values = this.PlaceholderExtractor.ExtractPlaceholderValuesFromQuery(templateObject, actualPathObject);

            string pathAndQuery = "/api/movies/{movieName}";
            TemplatePathAndQueryObject downstreamTemplate = new TemplatePathAndQueryObject(pathAndQuery);

            string hydrated = this.PlaceholderExtractor.ReplacePlaceholdersWithValues(downstreamTemplate, values);

            Assert.That(hydrated, Is.EqualTo("/api/movies/AmericanGangster"));
        }

        [Test]
        public void ReplacePlaceholdervalues_Test_3()
        {
            string template = "/movies/{movieName}?Rating={movieRating}";
            string query = "/movies/AmericanGangster?Rating=9.7";
            TemplatePathAndQueryObject templateObject = new TemplatePathAndQueryObject(template);
            PathAndQueryObject pathObject = new PathAndQueryObject(query);

            List<PlaceholderValue> values = this.PlaceholderExtractor.ExtractPlaceholderValuesFromQuery(templateObject, pathObject);

            string pathAndQuery = "/api/movies/{movieName}?Rating={movieRating}";
            TemplatePathAndQueryObject downstreamTemplate = new TemplatePathAndQueryObject(pathAndQuery);

            string hydrated = this.PlaceholderExtractor.ReplacePlaceholdersWithValues(downstreamTemplate, values);

            Assert.That(hydrated, Is.EqualTo("/api/movies/AmericanGangster?Rating=9.7"));
        }

        [Test]
        public void ReplacePlaceholdervalues_Test_4()
        {
            string template = "/movies/{movieName}/{movieRating}";
            string query = "/movies/AmericanGangster/9.7";
            TemplatePathAndQueryObject templateObject = new TemplatePathAndQueryObject(template);
            PathAndQueryObject pathObject = new PathAndQueryObject(query);

            List<PlaceholderValue> values = this.PlaceholderExtractor.ExtractPlaceholderValuesFromQuery(templateObject, pathObject);

            string pathAndQuery = "/api/movies/{movieName}/{movieRating}";
            TemplatePathAndQueryObject downstreamTemplate = new TemplatePathAndQueryObject(pathAndQuery);

            string hydrated = this.PlaceholderExtractor.ReplacePlaceholdersWithValues(downstreamTemplate, values);

            Assert.That(hydrated, Is.EqualTo("/api/movies/AmericanGangster/9.7"));
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles